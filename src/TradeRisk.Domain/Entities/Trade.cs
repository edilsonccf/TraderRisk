using TradeRisk.Domain.Enums;

namespace TradeRisk.Domain.Entities;

public class Trade
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ClientId { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string ClientSector { get; set; } = string.Empty;
    public RiskCategory? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}