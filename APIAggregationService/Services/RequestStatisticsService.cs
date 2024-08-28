using System.Collections.Concurrent;

public class RequestStatisticsService
{
    private readonly ConcurrentDictionary<string, RequestStats> _stats = new();

    public void LogRequest(string endpoint, long responseTime)
    {
        var stats = _stats.GetOrAdd(endpoint, new RequestStats());
        stats.TotalRequests++;
        stats.TotalResponseTime += responseTime;

        // You can categorize by response time if you want buckets (e.g., fast, average, slow)
        if (responseTime < 100)
            stats.FastRequests++;
        else if (responseTime < 200)
            stats.AverageRequests++;
        else
            stats.SlowRequests++;
    }

    public IEnumerable<KeyValuePair<string, RequestStats>> GetStatistics()
    {
        return _stats;
    }
}

public class RequestStats
{
    public int TotalRequests { get; set; }
    public long TotalResponseTime { get; set; }
    public int FastRequests { get; set; }
    public int AverageRequests { get; set; }
    public int SlowRequests { get; set; }

    public double GetAverageResponseTime()
    {
        return TotalRequests == 0 ? 0 : (double)TotalResponseTime / TotalRequests;
    }
}
