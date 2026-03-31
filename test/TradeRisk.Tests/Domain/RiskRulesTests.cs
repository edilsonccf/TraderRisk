using Xunit;
using TradeRisk.Domain.Entities;
using TradeRisk.Domain.Rules;
using TradeRisk.Domain.Enums;

namespace TradeRisk.Tests.Domain;

public class RiskRulesTests
{
    [Theory]
    [InlineData(999999, "Public", RiskCategory.LOWRISK)]
    [InlineData(500000, "Private", RiskCategory.LOWRISK)]
    public void LowRiskRule_ShouldMatch_WhenValueIsLessThanOneMillion(decimal value, string sector, RiskCategory expected)
    {
        var rule = new LowRiskRule();
        var trade = new Trade { Value = value, ClientSector = sector };

        var result = rule.IsMatch(trade);

        Assert.True(result);
        Assert.Equal(expected, rule.Category);
    }

    [Theory]
    [InlineData(1000000, "Public", true)]
    [InlineData(2000000, "Public", true)]
    [InlineData(1000000, "Private", false)]
    public void MediumRiskRule_ShouldMatch_OnlyWhenValueIsHighAndSectorIsPublic(decimal value, string sector, bool expectedMatch)
    {
        var rule = new MediumRiskRule();
        var trade = new Trade { Value = value, ClientSector = sector };

        Assert.Equal(expectedMatch, rule.IsMatch(trade));
    }

    [Theory]
    [InlineData(1000000, "Private", true)]
    [InlineData(5000000, "Private", true)]
    [InlineData(1000000, "Public", false)]
    public void HighRiskRule_ShouldMatch_OnlyWhenValueIsHighAndSectorIsPrivate(decimal value, string sector, bool expectedMatch)
    {
        var rule = new HighRiskRule();
        var trade = new Trade { Value = value, ClientSector = sector };

        Assert.Equal(expectedMatch, rule.IsMatch(trade));
    }
}