using Infrastructure.Services;

namespace Infrastructure.FunctionalTests.Services;

public class ProcessJobStaticServiceTests
{
    [Test]
    public async Task ProcessPageSource_WhenPageSourceIsInvalid_ReturnsError()
    {
        // Arrange
        var pageSource = "<html><body></body></html>";
        var scrapeAllElements = true;

        // Act
        var (resultJson, resultBool) = await ProcessJobStaticService.ProcessPageSource(
            scrapeAllElements,
            pageSource
        );

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultBool, Is.False);
            Assert.That(resultJson, Does.Contain("Failed to parse page body."));
        });
    }

    [Test]
    public async Task ProcessPageSource_WhenPageSourceIsValid_ReturnsJson()
    {
        // Arrange
        var pageSource = File.ReadAllText("testPage.html");
        var scrapeAllElements = true;

        // Act
        var (resultJson, resultBool) = await ProcessJobStaticService.ProcessPageSource(
            scrapeAllElements,
            pageSource
        );

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultBool, Is.True);
            Assert.That(resultJson, Does.Not.Contain("Failed to parse page body."));
        });
    }
}
