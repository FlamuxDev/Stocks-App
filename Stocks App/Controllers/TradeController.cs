using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks_App.DTOs;
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
            
            var defaultQuantity = _tradingOptions.Value.DefaultOrderQuantity;
            
            var stockTrade = new StockTrade
            {
                StockSymbol = GetStringValue(companyProfile, "ticker", stockSymbol),
                StockName = GetStringValue(companyProfile, "name", "N/A"),
                Price = GetDoubleValue(stockPriceQuote, "c"),
                Quantity = defaultQuantity > 0 ? defaultQuantity : 100
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
                Quantity = _tradingOptions.Value.DefaultOrderQuantity > 0 ? _tradingOptions.Value.DefaultOrderQuantity : 100
            });
        }
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> BuyOrder([FromForm] BuyOrderRequest buyOrderRequest)
    {
        if (buyOrderRequest == null)
        {
            return RedirectToAction("Index");
        }

        buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

        if (!ModelState.IsValid)
        {
            // If validation fails, redirect back to Index with error
            TempData["Error"] = "Validation failed. Please check your input.";
            return RedirectToAction("Index");
        }

        try
        {
            await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction("Orders");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating buy order");
            TempData["Error"] = "Failed to create buy order. Please try again.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SellOrder([FromForm] SellOrderRequest sellOrderRequest)
    {
        if (sellOrderRequest == null)
        {
            return RedirectToAction("Index");
        }

        sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

        if (!ModelState.IsValid)
        {
            // If validation fails, redirect back to Index with error
            TempData["Error"] = "Validation failed. Please check your input.";
            return RedirectToAction("Index");
        }

        try
        {
            await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction("Orders");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sell order");
            TempData["Error"] = "Failed to create sell order. Please try again.";
            return RedirectToAction("Index");
        }
    }

    [Route("[action]")]
    public async Task<IActionResult> Orders()
    {
        try
        {
            var buyOrdersTask = _stocksService.GetBuyOrders();
            var sellOrdersTask = _stocksService.GetSellOrders();

            await Task.WhenAll(buyOrdersTask, sellOrdersTask);

            var buyOrders = await buyOrdersTask;
            var sellOrders = await sellOrdersTask;

            var orders = new Orders
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return View(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching orders");
            return View(new Orders());
        }
    }
}

