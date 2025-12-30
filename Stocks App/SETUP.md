# Stocks App Setup Guide

## Prerequisites
- .NET 8.0 SDK
- A Finnhub.io account (or use the provided token)

## Configuration Steps

### 1. Configure User Secrets (Recommended)

To store your Finnhub API token securely, use .NET User Secrets:

```powershell
cd "Stocks App"
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "your-token-here"
```

**Note:** Replace `"your-token-here"` with your actual Finnhub API token.

If you don't have a token, you can:
- Register at https://finnhub.io/login
- Get your API key from https://finnhub.io/dashboard
- Or use the provided token: `cc676uaad3i9rj8tb1s0`

### 2. Configure Default Stock Symbol

The default stock symbol is configured in `appsettings.json`:

```json
"TradingOptions": {
  "DefaultStockSymbol": "MSFT"
}
```

You can change this to any valid stock symbol (e.g., "AAPL", "GOOGL", "TSLA").

### 3. Run the Application

```powershell
dotnet run
```

Navigate to: `https://localhost:5001/Trade/Index` (or the URL shown in the console)

## Architecture

The application follows n-layer architecture:
- **Models**: Data models (StockTrade, TradingOptions)
- **Services**: Business logic (IFinnhubService, FinnhubService)
- **Controllers**: Request handling (TradeController)
- **Views**: UI presentation (Trade/Index.cshtml)

## Features

- Real-time stock price updates via WebSocket
- Company profile display
- Modern, responsive UI
- Automatic reconnection on WebSocket disconnect

