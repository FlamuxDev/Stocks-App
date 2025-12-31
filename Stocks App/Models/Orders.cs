using Stocks_App.DTOs;

namespace Stocks_App.Models;

public class Orders
{
    public List<BuyOrderResponse> BuyOrders { get; set; } = new();
    public List<SellOrderResponse> SellOrders { get; set; } = new();
}

