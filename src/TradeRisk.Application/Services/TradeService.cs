using System.Diagnostics;
using TradeRisk.Application.DTOs;
using TradeRisk.Domain.Entities;
using TradeRisk.Domain.Rules;

namespace TradeRisk.Application.Services;

public class TradeService : ITradeService
{
    private readonly IEnumerable<IRiskRule> _rules;

    public TradeService(IEnumerable<IRiskRule> rules)
    {
        _rules = rules.OrderByDescending(r => r.Priority);
    }

    public async Task<ClassificationResponse> ClassifyTradesAsync(IEnumerable<TradeInput> inputs, bool includeSummary)
    {
        var sw = Stopwatch.StartNew();
        var results = new List<Trade>();

        foreach (var input in inputs)
        {
            var trade = new Trade { Value = input.Value, ClientSector = input.ClientSector, ClientId = input.ClientId };
            trade.Category = _rules.FirstOrDefault(r => r.IsMatch(trade))?.Category;
            results.Add(trade);
        }

        var categories = results.Select(r => r.Category.ToString()!).ToList();
        
        if (!includeSummary) return new ClassificationResponse(categories);

        var summary = results
            .GroupBy(r => r.Category!.Value)
            .ToDictionary(
                g => g.Key.ToString(),
                g => new RiskSummaryItem(
                    g.Count(),
                    g.Sum(t => t.Value),
                    g.GroupBy(t => t.ClientId)
                     .OrderByDescending(cg => cg.Sum(t => t.Value))
                     .First().Key
                )
            );

        sw.Stop();
        return new ClassificationResponse(categories, summary, sw.ElapsedMilliseconds);
    }
}