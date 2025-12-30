using Stocks_App.DTOs;
using Stocks_App.Models;

namespace Stocks_App.Services;

public class StocksService : IStocksService
{
    private readonly List<BuyOrder> _buyOrders = new();
    private readonly List<SellOrder> _sellOrders = new();
    private static readonly DateTime MinDate = new DateTime(2000, 1, 1);

    public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        // Validation: null check
        if (buyOrderRequest == null)
        {
            throw new ArgumentNullException(nameof(buyOrderRequest));
        }

        // Validation: Stock symbol cannot be null or empty
        if (string.IsNullOrWhiteSpace(buyOrderRequest.StockSymbol))
        {
            throw new ArgumentException("Stock symbol cannot be null or empty", nameof(buyOrderRequest));
        }

        // Validation: Quantity must be between 1 and 100000
        if (buyOrderRequest.Quantity < 1 || buyOrderRequest.Quantity > 100000)
        {
            throw new ArgumentException("Quantity must be between 1 and 100000", nameof(buyOrderRequest));
        }

        // Validation: Price must be between 1 and 10000
        if (buyOrderRequest.Price < 1 || buyOrderRequest.Price > 10000)
        {
            throw new ArgumentException("Price must be between 1 and 10000", nameof(buyOrderRequest));
        }

        // Validation: Date must not be older than Jan 01, 2000
        if (buyOrderRequest.DateAndTimeOfOrder < MinDate)
        {
            throw new ArgumentException("Date and time of order must not be older than Jan 01, 2000", nameof(buyOrderRequest));
        }

        // Create entity
        var buyOrder = new BuyOrder
        {
            BuyOrderID = Guid.NewGuid(),
            StockSymbol = buyOrderRequest.StockSymbol,
            StockName = buyOrderRequest.StockName,
            DateAndTimeOfOrder = buyOrderRequest.DateAndTimeOfOrder,
            Quantity = buyOrderRequest.Quantity,
            Price = buyOrderRequest.Price
        };

        // Add to in-memory list (simulating database)
        _buyOrders.Add(buyOrder);

        // Create response
        var response = new BuyOrderResponse
        {
            BuyOrderID = buyOrder.BuyOrderID,
            StockSymbol = buyOrder.StockSymbol,
            StockName = buyOrder.StockName,
            DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
            Quantity = buyOrder.Quantity,
            Price = buyOrder.Price,
            TradeAmount = buyOrder.Quantity * buyOrder.Price
        };

        return Task.FromResult(response);
    }

    public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        // Validation: null check
        if (sellOrderRequest == null)
        {
            throw new ArgumentNullException(nameof(sellOrderRequest));
        }

        // Validation: Stock symbol cannot be null or empty
        if (string.IsNullOrWhiteSpace(sellOrderRequest.StockSymbol))
        {
            throw new ArgumentException("Stock symbol cannot be null or empty", nameof(sellOrderRequest));
        }

        // Validation: Quantity must be between 1 and 100000
        if (sellOrderRequest.Quantity < 1 || sellOrderRequest.Quantity > 100000)
        {
            throw new ArgumentException("Quantity must be between 1 and 100000", nameof(sellOrderRequest));
        }

        // Validation: Price must be between 1 and 10000
        if (sellOrderRequest.Price < 1 || sellOrderRequest.Price > 10000)
        {
            throw new ArgumentException("Price must be between 1 and 10000", nameof(sellOrderRequest));
        }

        // Validation: Date must not be older than Jan 01, 2000
        if (sellOrderRequest.DateAndTimeOfOrder < MinDate)
        {
            throw new ArgumentException("Date and time of order must not be older than Jan 01, 2000", nameof(sellOrderRequest));
        }

        // Create entity
        var sellOrder = new SellOrder
        {
            SellOrderID = Guid.NewGuid(),
            StockSymbol = sellOrderRequest.StockSymbol,
            StockName = sellOrderRequest.StockName,
            DateAndTimeOfOrder = sellOrderRequest.DateAndTimeOfOrder,
            Quantity = sellOrderRequest.Quantity,
            Price = sellOrderRequest.Price
        };

        // Add to in-memory list (simulating database)
        _sellOrders.Add(sellOrder);

        // Create response
        var response = new SellOrderResponse
        {
            SellOrderID = sellOrder.SellOrderID,
            StockSymbol = sellOrder.StockSymbol,
            StockName = sellOrder.StockName,
            DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
            Quantity = sellOrder.Quantity,
            Price = sellOrder.Price,
            TradeAmount = sellOrder.Quantity * sellOrder.Price
        };

        return Task.FromResult(response);
    }

    public Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        var responses = _buyOrders.Select(bo => new BuyOrderResponse
        {
            BuyOrderID = bo.BuyOrderID,
            StockSymbol = bo.StockSymbol,
            StockName = bo.StockName,
            DateAndTimeOfOrder = bo.DateAndTimeOfOrder,
            Quantity = bo.Quantity,
            Price = bo.Price,
            TradeAmount = bo.Quantity * bo.Price
        }).ToList();

        return Task.FromResult(responses);
    }

    public Task<List<SellOrderResponse>> GetSellOrders()
    {
        var responses = _sellOrders.Select(so => new SellOrderResponse
        {
            SellOrderID = so.SellOrderID,
            StockSymbol = so.StockSymbol,
            StockName = so.StockName,
            DateAndTimeOfOrder = so.DateAndTimeOfOrder,
            Quantity = so.Quantity,
            Price = so.Price,
            TradeAmount = so.Quantity * so.Price
        }).ToList();

        return Task.FromResult(responses);
    }
}

