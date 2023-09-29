using System.Text;
using System.Text.Json;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Services;

public class RabbitMQWorkerService : BackgroundService
{
    private readonly ILogger<RabbitMQWorkerService> _logger;
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public RabbitMQWorkerService(
        IConfiguration config,
        ILogger<RabbitMQWorkerService> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IConnection connection;
        IModel channel;
        try
        {
            var factory = new ConnectionFactory()
            {
                // RabbitMQ server info
                HostName = _config.GetSection("RabbitMQ")["HostAddress"]?.ToString() ?? "localhost"
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }
        catch (Exception ex)
        {
            _logger.LogError($"RabbitMQ Worker failed to connect to RabbitMQ server: {ex.Message}");
            return;
        }

        stoppingToken.ThrowIfCancellationRequested();

        RunConsumer(channel, stoppingToken);

        // Keep the service running
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void RunConsumer(IModel channel, CancellationToken stoppingToken)
    {
        channel.QueueDeclare(
            queue: "task_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(queue: "task_queue", autoAck: false, consumer: consumer);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received Queued Message: {messageJson}");

            var scrapedUrl = JsonSerializer.Deserialize<ScrapedUrl>(messageJson)!;

            string remoteWebDriverUrl =
                _config.GetSection("SeleniumWebDriver")["HostAddress"]?.ToString()
                ?? "http://localhost:4444/wd/hub";
            // get the page source
            string pageSource;
            try
            {
                pageSource = ProcessJobStaticService.GetPageContents(
                    scrapedUrl.Url,
                    remoteWebDriverUrl
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load page source: {ex.Message}");
                scrapedUrl.JsonResult = JsonSerializer.Serialize(
                    new { Error = "Failed to load page source" }
                );

                await _mediator.Publish(new ScrapedUrlCompletedEvent(scrapedUrl), stoppingToken);

                // TODO : Add a retry mechanism
                channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                return;
            }
            // Process message here.
            var (resultJson, resultBool) = await ProcessJobStaticService.ProcessPageSource(
                scrapedUrl.ScrapeAllElements,
                pageSource
            );

            // Update the scrapedUrl object
            scrapedUrl.JsonResult = resultJson;
            scrapedUrl.IsScraped = resultBool;

            await _mediator.Publish(new ScrapedUrlCompletedEvent(scrapedUrl), stoppingToken);

            // Acknowledge the message
            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
    }
}
