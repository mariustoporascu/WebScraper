using BenchmarkDotNet.Attributes;
using Infrastructure.Services;

namespace Benchmarks;

[MemoryDiagnoser]
public class ProcessJobStaticServiceBenchmarks
{
    private string _pageSource = null!;
    private readonly bool _scrapeAllElements = true;

    [GlobalSetup]
    public void Setup()
    {
        // Initialize your data here
        _pageSource = File.ReadAllText("testPage.html");
    }

    [Benchmark]
    public async Task BenchmarkProcessPageSource()
    {
        var (resultJson, resultBool) = await ProcessJobStaticService.ProcessPageSource(
            _scrapeAllElements,
            _pageSource
        );
    }
}
