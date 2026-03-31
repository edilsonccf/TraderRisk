using Xunit;
using TradeRisk.Application.Services;
using TradeRisk.Application.DTOs;
using TradeRisk.Domain.Rules;
using System.Diagnostics;

namespace TradeRisk.Tests.Application;

public class LoadTests
{
    private readonly TradeService _service;

    public LoadTests()
    {
        var rules = new List<IRiskRule> { new LowRiskRule(), new MediumRiskRule(), new HighRiskRule() };
        _service = new TradeService(rules);
    }

    [Fact]
    public async Task ProcessLargeVolume_ShouldCompleteUnderThreshold()
    {
        // ARRANGE - Gerando 100.000 trades sintéticos
        int count = 100000;
        var inputs = new List<TradeInput>(count);
        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            inputs.Add(new TradeInput(
                Value: random.Next(500000, 5000000), 
                ClientSector: i % 2 == 0 ? "Public" : "Private",
                ClientId: $"CLI{i % 1000}" // 1000 clientes únicos para testar agrupamento
            ));
        }

        // ACT
        var stopwatch = Stopwatch.StartNew();
        var result = await _service.ClassifyTradesAsync(inputs, true);
        stopwatch.Stop();

        // ASSERT
        Assert.Equal(count, result.Categories.Count);
        Assert.NotNull(result.Summary);
        
        // Threshold: 100k registros devem ser processados em menos de 500ms 
        // (Ajuste conforme o hardware, mas LINQ em memória é sub-100ms geralmente)
        Assert.True(stopwatch.ElapsedMilliseconds < 500, 
            $"Performance degradada: {stopwatch.ElapsedMilliseconds}ms para {count} registros");
    }
}