using Microsoft.Extensions.Options;
using OddScrapperService.Scrappers.Core;
using OddScrapperService.Scrappers.Models;
using OddScrapperService.Scrappers.Services.utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace OddScrapperService.Scrappers.Services.BettingSites;
internal class SportingBetScrapper(ILogger<SportingBetScrapper> logger, IConfiguration configuration) : BackgroundService, IScrapper
{

    readonly ILogger<SportingBetScrapper> _logger = logger;
    readonly IConfiguration _configuration = configuration;


    public void Process1x2(ChromeDriver driver)
    {
        var elementsReturned = driver.FindElements(By.ClassName("grid-event"));

        foreach (var element in elementsReturned)
        {
            try
            {
                var participantElements = element.FindElements(By.ClassName("participant"));
                var bettingOddsElments = element.FindElements(By.ClassName("grid-option-group"))[0].FindElements(By.ClassName("option"));

                var startingDateText = element.FindElement(By.ClassName("starting-time")).Text;
                DateOnly finalDate;

                if (startingDateText.Contains("Today"))
                {
                    finalDate = DateOnly.FromDateTime(DateTime.Now);
                } else if (startingDateText.Contains("Tommorow"))
                {
                    finalDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
                } else
                {
                    finalDate = DateOnly.Parse(startingDateText.Split(" ")[0]);
                }
             
                var teamOne = participantElements[0].Text;
                var teamTwo = participantElements[1].Text;
                var teamOneWin = bettingOddsElments[0].Text;
                var drawOdds = bettingOddsElments[1].Text;
                var teamTwoWin = bettingOddsElments[2].Text;
                var matchId = ParsingUtils.GenerateIdentifier(teamOne, teamTwo, finalDate);

                BettingFootballOdds parsedGame = new()
                {
                    MatchIdentifier = matchId,
                    SportsBook = "SportingBet",
                    TeamOne = teamOne,
                    TeamTwo = teamTwo,
                    TeamOneOdds = ParsingUtils.ParseOdds(teamOneWin),
                    TeamTwoOdds = ParsingUtils.ParseOdds(teamTwoWin),
                    DrawOdds = ParsingUtils.ParseOdds(drawOdds),
                    MatchDate = finalDate
                };

                _logger.LogInformation("{matchId}:{teamOneWin}",matchId,teamOneWin);

            } catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong when parsing");
            }
        }
    }

    public async Task<IEnumerable<BettingFootballOdds>> GetFootballOddsAsync()
    {
        try
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");
            chromeOptions.AddArgument("--disable-gpu");

            using var driver = new ChromeDriver();
            await driver.Navigate().GoToUrlAsync(_configuration["SportsBooks:SportingBet:Football:PremierLeague"]);

            await Task.Run(() => Thread.Sleep(int.Parse(_configuration["LoadInTimeout"])));

            Process1x2(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse a game");
            throw;
        }

        return null;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                await GetFootballOddsAsync();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(15000, stoppingToken);
        }
    }
}

