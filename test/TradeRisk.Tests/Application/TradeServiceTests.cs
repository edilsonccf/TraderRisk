using TradeRisk.Application.Services;
using TradeRisk.Application.DTOs;
using TradeRisk.Domain.Rules;


namespace TradeRisk.Tests.Application;

public class TradeServiceTests
{
    private readonly TradeService _service;

    public TradeServiceTests()
    {
        // Setup com as regras reais para testar a integração da lógica
        var rules = new List<IRiskRule> 
        { 
            new LowRiskRule(), 
            new MediumRiskRule(), 
            new HighRiskRule() 
        };
        _service = new TradeService(rules);
    }

    [Fact]
    public async Task ClassifyTrades_ShouldReturnCorrectOrderOfCategories()
    {
        // Arrange
        var inputs = new List<TradeInput>
        {
            new(2000000, "Private", "C1"), // HIGH
            new(400000, "Public", "C2"),   // LOW
            new(3000000, "Public", "C3")   // MEDIUM
        };

        // Act
        var result = await _service.ClassifyTradesAsync(inputs, false);

        // Assert
        Assert.Equal(3, result.Categories.Count);
        Assert.Equal("HIGHRISK", result.Categories[0]);
        Assert.Equal("LOWRISK", result.Categories[1]);
        Assert.Equal("MEDIUMRISK", result.Categories[2]);
    }

    [Fact]
    public async Task ClassifyTrades_WithSummary_ShouldCalculateCorrectAggregates()
    {
        // Arrange
        var inputs = new List<TradeInput>
        {
            new(500000, "Public", "CLI001"),
            new(400000, "Public", "CLI001"),
            new(2000000, "Private", "CLI002")
        };

        // Act
        var result = await _service.ClassifyTradesAsync(inputs, true);

        // Assert
        Assert.NotNull(result.Summary);
        Assert.Equal(2, result.Summary["LOWRISK"].Count);
        Assert.Equal(900000, result.Summary["LOWRISK"].TotalValue);
        Assert.Equal("CLI001", result.Summary["LOWRISK"].TopClient);
        
        Assert.Equal(2000000, result.Summary["HIGHRISK"].TotalValue);
        Assert.Equal("CLI002", result.Summary["HIGHRISK"].TopClient);
    }

    [Fact]
    public async Task ClassifyTrades_ShouldHandleLargeVolumesEfficiently()
    {
        // Arrange - 100.000 trades
        var inputs = Enumerable.Range(0, 100000)
            .Select(i => new TradeInput(100, "Public", $"CLI{i}"))
            .ToList();

        // Act
        var result = await _service.ClassifyTradesAsync(inputs, true);

        // Assert
        Assert.Equal(100000, result.Categories.Count);
        Assert.True(result.ProcessingTimeMs >= 0);
    }
}