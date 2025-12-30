using System.ComponentModel.DataAnnotations;

namespace Stocks_App.DTOs;

public class BuyOrderRequest
{
    [Required(ErrorMessage = "Stock symbol is mandatory")]
    public string StockSymbol { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Stock name is mandatory")]
    public string StockName { get; set; } = string.Empty;
    
    [DataType(DataType.DateTime)]
    public DateTime DateAndTimeOfOrder { get; set; }
    
    [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
    public uint Quantity { get; set; }
    
    [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
    public double Price { get; set; }
}

