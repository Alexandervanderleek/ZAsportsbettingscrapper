using Microsoft.Extensions.DependencyInjection;
using OddScrapperService;
using OddScrapperService.Scrappers.Core;
using OddScrapperService.Scrappers.Jobs;
using OddScrapperService.Scrappers.Services.BettingSites;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddQuartz(q =>
//{
//    q.AddJob<SportingBetJob>(j => j.WithIdentity("sportingBet","scrapers").StoreDurably());
//});

builder.Services.AddQuartz();

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddTransient<SportingBetScrapper>();
builder.Services.AddTransient<IScrapper, SportingBetScrapper>();
builder.Services.AddSingleton<IScrapperFactory, ScrapperFactory>();

builder.Services.AddHostedService<ScrapperWorker>();

var host = builder.Build();
host.Run();
