using System.Text.Json;

namespace Stocks_App.Services;

public class FinnhubService : IFinnhubService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FinnhubService> _logger;

    public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<FinnhubService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var token = _configuration["FinnhubToken"] ?? throw new InvalidOperationException("FinnhubToken not found in configuration");
            var url = $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={token}";
            
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching company profile for {StockSymbol}", stockSymbol);
            return null;
        }
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var token = _configuration["FinnhubToken"] ?? throw new InvalidOperationException("FinnhubToken not found in configuration");
            var url = $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={token}";
            
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stock price quote for {StockSymbol}", stockSymbol);
            return null;
        }
    }
}

