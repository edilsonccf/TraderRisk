using Microsoft.AspNetCore.Mvc;
using TradeRisk.Application.DTOs;
using TradeRisk.Application.Services;

namespace TradeRisk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TradesController : ControllerBase
{
    private readonly ITradeService _tradeService;

    public TradesController(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpPost("classify")]
    public async Task<IActionResult> Classify([FromBody] List<TradeInput> trades)
    {
        var response = await _tradeService.ClassifyTradesAsync(trades, false);
        return Ok(new { categories = response.Categories });
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze([FromBody] List<TradeInput> trades)
    {
        if (trades.Count > 100000) return BadRequest("Limit exceeded (100k).");
        var response = await _tradeService.ClassifyTradesAsync(trades, true);
        return Ok(response);
    }
}