using Stocks_App.DTOs;
using Stocks_App.Services;

namespace Stocks_App.Tests;

public class StocksServiceTests
{
    private readonly IStocksService _stocksService;

    public StocksServiceTests()
    {
        _stocksService = new StocksService();
    }

    #region CreateBuyOrder Tests

    [Fact]
    public async Task CreateBuyOrder_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _stocksService.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    public async Task CreateBuyOrder_QuantityIsZero_ThrowsArgumentException()
    {
        // Arrange
        var buyOrderRequest = new BuyOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 0,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateBuyOrder(buyOrderRequest));
        
        Assert.Contains("Quantity must be between 1 and 100000", exception.Message);
    }

    [Fact]
    public async Task CreateBuyOrder_QuantityIs100001_ThrowsArgumentException()
    {
        // Arrange
        var buyOrderRequest = new BuyOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 100001,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateBuyOrder(buyOrderRequest));
        
        Assert.Contains("Quantity must be between 1 and 100000", exception.Message);
    }

    [Fact]
    public async Task CreateBuyOrder_PriceIsZero_ThrowsArgumentException()
    {
        // Arrange
        var buyOrderRequest = new BuyOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 0
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateBuyOrder(buyOrderRequest));
        
        Assert.Contains("Price must be between 1 and 10000", exception.Message);
    }

    [Fact]
    public async Task CreateBuyOrder_PriceIs10001_ThrowsArgumentException()
    {
        // Arrange
        var buyOrderRequest = new BuyOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 10001
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateBuyOrder(buyOrderRequest));
        
        Assert.Contains("Price must be between 1 and 10000", exception.Message);
    }

    [Fact]
    public async Task CreateBuyOrder_StockSymbolIsNull_ThrowsArgumentException()
    {
        // Arrange
        var buyOrderRequest = new BuyOrderRequest
        {
            StockSymbol = null!,
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateBuyOrder(buyOrderRequest));
        
        Assert.Contains("Stock symbol cannot be null or empty", exception.Message);
    }

    [Fact]
    public async Task CreateBuyOrder_DateAndTimeOfOrderIs1999_12_31_ThrowsArgumentException()
    {
        // Arrange
        var buyOrderRequest = new BuyOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(1999, 12, 31),
            Quantity = 10,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateBuyOrder(buyOrderRequest));
        
        Assert.Contains("Date and time of order must not be older than Jan 01, 2000", exception.Message);
    }

    [Fact]
    public async Task CreateBuyOrder_ValidValues_ReturnsBuyOrderResponseWithGuid()
    {
        // Arrange
        var buyOrderRequest = new BuyOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 100
        };

        // Act
        var result = await _stocksService.CreateBuyOrder(buyOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.BuyOrderID);
        Assert.Equal("MSFT", result.StockSymbol);
        Assert.Equal("Microsoft Corp", result.StockName);
        Assert.Equal(new DateTime(2024, 1, 1), result.DateAndTimeOfOrder);
        Assert.Equal(10u, result.Quantity);
        Assert.Equal(100, result.Price);
        Assert.Equal(1000, result.TradeAmount); // 10 * 100
    }

    #endregion

    #region CreateSellOrder Tests

    [Fact]
    public async Task CreateSellOrder_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _stocksService.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    public async Task CreateSellOrder_QuantityIsZero_ThrowsArgumentException()
    {
        // Arrange
        var sellOrderRequest = new SellOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 0,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateSellOrder(sellOrderRequest));
        
        Assert.Contains("Quantity must be between 1 and 100000", exception.Message);
    }

    [Fact]
    public async Task CreateSellOrder_QuantityIs100001_ThrowsArgumentException()
    {
        // Arrange
        var sellOrderRequest = new SellOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 100001,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateSellOrder(sellOrderRequest));
        
        Assert.Contains("Quantity must be between 1 and 100000", exception.Message);
    }

    [Fact]
    public async Task CreateSellOrder_PriceIsZero_ThrowsArgumentException()
    {
        // Arrange
        var sellOrderRequest = new SellOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 0
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateSellOrder(sellOrderRequest));
        
        Assert.Contains("Price must be between 1 and 10000", exception.Message);
    }

    [Fact]
    public async Task CreateSellOrder_PriceIs10001_ThrowsArgumentException()
    {
        // Arrange
        var sellOrderRequest = new SellOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 10001
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateSellOrder(sellOrderRequest));
        
        Assert.Contains("Price must be between 1 and 10000", exception.Message);
    }

    [Fact]
    public async Task CreateSellOrder_StockSymbolIsNull_ThrowsArgumentException()
    {
        // Arrange
        var sellOrderRequest = new SellOrderRequest
        {
            StockSymbol = null!,
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateSellOrder(sellOrderRequest));
        
        Assert.Contains("Stock symbol cannot be null or empty", exception.Message);
    }

    [Fact]
    public async Task CreateSellOrder_DateAndTimeOfOrderIs1999_12_31_ThrowsArgumentException()
    {
        // Arrange
        var sellOrderRequest = new SellOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(1999, 12, 31),
            Quantity = 10,
            Price = 100
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _stocksService.CreateSellOrder(sellOrderRequest));
        
        Assert.Contains("Date and time of order must not be older than Jan 01, 2000", exception.Message);
    }

    [Fact]
    public async Task CreateSellOrder_ValidValues_ReturnsSellOrderResponseWithGuid()
    {
        // Arrange
        var sellOrderRequest = new SellOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 100
        };

        // Act
        var result = await _stocksService.CreateSellOrder(sellOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.SellOrderID);
        Assert.Equal("MSFT", result.StockSymbol);
        Assert.Equal("Microsoft Corp", result.StockName);
        Assert.Equal(new DateTime(2024, 1, 1), result.DateAndTimeOfOrder);
        Assert.Equal(10u, result.Quantity);
        Assert.Equal(100, result.Price);
        Assert.Equal(1000, result.TradeAmount); // 10 * 100
    }

    #endregion

    #region GetBuyOrders Tests

    [Fact]
    public async Task GetBuyOrders_ByDefault_ReturnsEmptyList()
    {
        // Arrange - Create a new service instance to ensure empty state
        var stocksService = new StocksService();

        // Act
        var result = await stocksService.GetBuyOrders();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetBuyOrders_AfterAddingBuyOrders_ReturnsAllBuyOrders()
    {
        // Arrange
        var stocksService = new StocksService();
        
        var buyOrderRequest1 = new BuyOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 100
        };

        var buyOrderRequest2 = new BuyOrderRequest
        {
            StockSymbol = "AAPL",
            StockName = "Apple Inc",
            DateAndTimeOfOrder = new DateTime(2024, 1, 2),
            Quantity = 5,
            Price = 150
        };

        // Act
        await stocksService.CreateBuyOrder(buyOrderRequest1);
        await stocksService.CreateBuyOrder(buyOrderRequest2);
        var result = await stocksService.GetBuyOrders();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.StockSymbol == "MSFT");
        Assert.Contains(result, r => r.StockSymbol == "AAPL");
    }

    #endregion

    #region GetSellOrders Tests

    [Fact]
    public async Task GetSellOrders_ByDefault_ReturnsEmptyList()
    {
        // Arrange - Create a new service instance to ensure empty state
        var stocksService = new StocksService();

        // Act
        var result = await stocksService.GetSellOrders();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetSellOrders_AfterAddingSellOrders_ReturnsAllSellOrders()
    {
        // Arrange
        var stocksService = new StocksService();
        
        var sellOrderRequest1 = new SellOrderRequest
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
            DateAndTimeOfOrder = new DateTime(2024, 1, 1),
            Quantity = 10,
            Price = 100
        };

        var sellOrderRequest2 = new SellOrderRequest
        {
            StockSymbol = "AAPL",
            StockName = "Apple Inc",
            DateAndTimeOfOrder = new DateTime(2024, 1, 2),
            Quantity = 5,
            Price = 150
        };

        // Act
        await stocksService.CreateSellOrder(sellOrderRequest1);
        await stocksService.CreateSellOrder(sellOrderRequest2);
        var result = await stocksService.GetSellOrders();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.StockSymbol == "MSFT");
        Assert.Contains(result, r => r.StockSymbol == "AAPL");
    }

    #endregion
}

