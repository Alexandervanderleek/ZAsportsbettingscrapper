using Microsoft.Extensions.DependencyInjection;
using OddScrapperService;
using OddScrapperService.Scrappers.Services.BettingSites;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<SportingBetScrapper>();
var host = builder.Build();
host.Run();
