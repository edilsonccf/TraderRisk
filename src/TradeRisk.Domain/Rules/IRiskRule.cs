using TradeRisk.Domain.Entities;
using TradeRisk.Domain.Enums;

namespace TradeRisk.Domain.Rules;

public interface IRiskRule
{
    bool IsMatch(Trade trade);
    RiskCategory Category { get; }
    int Priority { get; }
}