using System;
using Xunit;

public class RequestStatisticsServiceTests
{
    [Fact]
    public void LogRequest_ShouldCategorizeRequestsByResponseTime()
    {
        // Arrange
        var service = new RequestStatisticsService();

        // Act
        service.LogRequest("GetCats", 50);  // Fast
        service.LogRequest("GetCats", 150); // Average
        service.LogRequest("GetCats", 250); // Slow

        // Assert
        var stats = service.GetStatistics();
        var getCatsStats = stats.First(stat => stat.Key == "GetCats").Value;

        Assert.Equal(3, getCatsStats.TotalRequests);
        Assert.Equal(1, getCatsStats.FastRequests);
        Assert.Equal(1, getCatsStats.AverageRequests);
        Assert.Equal(1, getCatsStats.SlowRequests);
    }

    [Fact]
    public void GetAverageResponseTime_ShouldReturnCorrectAverage()
    {
        // Arrange
        var service = new RequestStatisticsService();
        service.LogRequest("GetCats", 100);
        service.LogRequest("GetCats", 200);

        // Act
        var stats = service.GetStatistics();
        var getCatsStats = stats.First(stat => stat.Key == "GetCats").Value;

        // Assert
        Assert.Equal(2, getCatsStats.TotalRequests);
        Assert.Equal(150, getCatsStats.GetAverageResponseTime());
    }
}
