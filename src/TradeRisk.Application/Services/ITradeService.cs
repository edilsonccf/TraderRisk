using TradeRisk.Application.DTOs;

namespace TradeRisk.Application.Services;

public interface ITradeService
{
    Task<ClassificationResponse> ClassifyTradesAsync(IEnumerable<TradeInput> inputs, bool includeSummary);
}