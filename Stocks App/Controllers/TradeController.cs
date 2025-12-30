using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks_App.Models;
using Stocks_App.Services;

namespace Stocks_App.Controllers;

[Route("[controller]")]
public class TradeController : Controller
{
    private readonly IOptions<TradingOptions> _tradingOptions;
    private readonly IFinnhubService _finnhubService;
    private readonly IStocksService _stocksService;
    private readonly ILogger<TradeController> _logger;
    private readonly IConfiguration _configuration;

    public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IStocksService stocksService, ILogger<TradeController> logger, IConfiguration configuration)
    {
        _tradingOptions = tradingOptions;
        _finnhubService = finnhubService;
        _stocksService = stocksService;
        _logger = logger;
        _configuration = configuration;
    }

    [Route("[action]")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var stockSymbol = _tradingOptions.Value.DefaultStockSymbol ?? "MSFT";
            var token = _configuration["FinnhubToken"] ?? "cc676uaad3i9rj8tb1s0";
            
            ViewData["FinnhubToken"] = token;
            
            var companyProfileTask = _finnhubService.GetCompanyProfile(stockSymbol);
            var stockPriceQuoteTask = _finnhubService.GetStockPriceQuote(stockSymbol);
            
            await Task.WhenAll(companyProfileTask, stockPriceQuoteTask);
            
            var companyProfile = await companyProfileTask;
            var stockPriceQuote = await stockPriceQuoteTask;
            
            if (companyProfile == null || stockPriceQuote == null)
            {
                _logger.LogWarning("Failed to fetch data for {StockSymbol}", stockSymbol);
                return View(new StockTrade
                {
                    StockSymbol = stockSymbol,
                    StockName = "N/A",
                    Price = 0,
                    Quantity = 0
                });
            }
            
            // Helper function to extract value from JsonElement or object
            double GetDoubleValue(Dictionary<string, object> dict, string key)
            {
                if (dict.ContainsKey(key) && dict[key] != null)
                {
                    if (dict[key] is JsonElement jsonElement)
                    {
                        return jsonElement.GetDouble();
                    }
                    return Convert.ToDouble(dict[key]);
                }
                return 0;
            }
            
            string GetStringValue(Dictionary<string, object> dict, string key, string defaultValue = "")
            {
                if (dict.ContainsKey(key) && dict[key] != null)
                {
                    var value = dict[key]?.ToString();
                    return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
                }
                return defaultValue;
            }
            
            var stockTrade = new StockTrade
            {
                StockSymbol = GetStringValue(companyProfile, "ticker", stockSymbol),
                StockName = GetStringValue(companyProfile, "name", "N/A"),
                Price = GetDoubleValue(stockPriceQuote, "c"),
                Quantity = 0
            };
            
            return View(stockTrade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in TradeController.Index");
            return View(new StockTrade
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol ?? "MSFT",
                StockName = "Error loading data",
                Price = 0,
                Quantity = 0
            });
        }
    }
}

