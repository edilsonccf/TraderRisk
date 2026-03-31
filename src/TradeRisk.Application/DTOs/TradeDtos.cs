namespace TradeRisk.Application.DTOs;

public record TradeInput(decimal Value, string ClientSector, string ClientId);

public record RiskSummaryItem(int Count, decimal TotalValue, string TopClient);

public record ClassificationResponse(
    List<string> Categories, 
    Dictionary<string, RiskSummaryItem>? Summary = null, 
    long? ProcessingTimeMs = null);