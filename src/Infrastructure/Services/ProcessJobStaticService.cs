using System.Text.Json;
using AngleSharp;
using AngleSharp.Dom;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Infrastructure.Services;

public static class ProcessJobStaticService
{
    public static async Task<(string, bool)> ProcessPageSource(
        bool scrapeAllElements,
        string pageSource
    )
    {
        // Initialize AngleSharp to parse the loaded content
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(req => req.Content(pageSource));

        // Extract the body
        var body = document.Body;
        if (body == null || string.IsNullOrEmpty(body.TextContent))
        {
            return (JsonSerializer.Serialize(new { Error = "Failed to parse page body." }), false);
        }

        var jsonRoot = new Dictionary<string, object>();

        var bodyElementsOfInterest = body.Children.Where(
            el => el.LocalName != "script" && !string.IsNullOrEmpty(el.TextContent)
        );
        foreach (var element in bodyElementsOfInterest)
        {
            var jsonNode = BuildJsonDFS(element, scrapeAllElements);
            if (jsonNode != null)
            {
                jsonRoot[element.LocalName] = jsonNode;
            }
        }

        return (JsonSerializer.Serialize(jsonRoot), true);
    }

    private static Dictionary<string, object> BuildJsonDFS(IElement element, bool scrapeAllElements)
    {
        if (element == null)
            return null!;

        var jsonNode = new Dictionary<string, object>();

        string directTextContent = element.ChildNodes
            .OfType<IText>()
            .Select(m => m.Text)
            .FirstOrDefault()
            ?.Trim()!;

        if (!string.IsNullOrWhiteSpace(directTextContent))
        {
            jsonNode["text"] = directTextContent;
        }
        if (!scrapeAllElements)
            goto BuildJsonDFSChildren;

        if (element.LocalName == "a")
        {
            var href = element.GetAttribute("href");
            if (!string.IsNullOrWhiteSpace(href))
            {
                jsonNode["link"] = href;
            }
        }
        if (element.LocalName == "img")
        {
            var src = element.GetAttribute("src");
            if (!string.IsNullOrWhiteSpace(src))
            {
                jsonNode["imgSrc"] = src;
            }
        }

        BuildJsonDFSChildren:
        var childrenJson = new List<Dictionary<string, object>>();
        foreach (var child in element.Children)
        {
            var childJson = BuildJsonDFS(child, scrapeAllElements);
            if (childJson == null || childJson.Count == 0)
                continue;
            childJson["tagName"] = child.LocalName;
            childrenJson.Add(childJson);
        }

        if (childrenJson.Count == 1)
        {
            return childrenJson[0];
        }

        if (childrenJson.Count > 1)
        {
            jsonNode["children"] = childrenJson;
        }

        return jsonNode;
    }

    public static string GetPageContents(string pageUrl, string remoteWebDriverUrl)
    {
        // Use Remote Selenium to load the page
        var chromeOptions = new ChromeOptions();
        using var driver = new RemoteWebDriver(new Uri(remoteWebDriverUrl), chromeOptions);
        // Navigate to the page
        driver.Navigate().GoToUrl(pageUrl);
        return driver.PageSource;
    }
}
