using System.Text;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Infrastructure.Services;

public class EnqueueScrapingJob : IEnqueueScrapingJob
{
    private readonly IConnection _connection;

    public EnqueueScrapingJob(IConfiguration config)
    {
        var factory = new ConnectionFactory()
        {
            // RabbitMQ server info
            HostName = config.GetSection("RabbitMQ")["HostAddress"]?.ToString() ?? "localhost"
        };
        _connection = factory.CreateConnection();
    }

    public Task EnqueueJob(string message, string queue)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: "",
                routingKey: queue,
                basicProperties: properties,
                body: body
            );
        }
        return Task.CompletedTask;
    }
}
