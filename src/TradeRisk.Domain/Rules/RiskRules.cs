using TradeRisk.Domain.Enums;

namespace TradeRisk.Domain.Rules;

public class LowRiskRule : IRiskRule
{
    public RiskCategory Category => RiskCategory.LOWRISK;
    public int Priority => 1;
    public bool IsMatch(Entities.Trade trade) => trade.Value < 1000000;
}

public class MediumRiskRule : IRiskRule
{
    public RiskCategory Category => RiskCategory.MEDIUMRISK;
    public int Priority => 2;
    public bool IsMatch(Entities.Trade trade) => 
        trade.Value >= 1000000 && trade.ClientSector.Equals("Public", StringComparison.OrdinalIgnoreCase);
}

public class HighRiskRule : IRiskRule
{
    public RiskCategory Category => RiskCategory.HIGHRISK;
    public int Priority => 2;
    public bool IsMatch(Entities.Trade trade) => 
        trade.Value >= 1000000 && trade.ClientSector.Equals("Private", StringComparison.OrdinalIgnoreCase);
}