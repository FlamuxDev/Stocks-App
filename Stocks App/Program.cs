using Stocks_App.Models;
using Stocks_App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure TradingOptions from appsettings.json
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

// Register HttpClientFactory for FinnhubService
builder.Services.AddHttpClient();

// Register FinnhubService
builder.Services.AddScoped<IFinnhubService, FinnhubService>();

// Register StocksService as Singleton to maintain in-memory data across requests
builder.Services.AddSingleton<IStocksService, StocksService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Trade}/{action=Index}/{id?}");

app.Run();
