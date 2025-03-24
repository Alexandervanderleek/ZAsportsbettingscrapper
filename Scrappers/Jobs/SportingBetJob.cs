using OddScrapperService.Scrappers.Core;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddScrapperService.Scrappers.Jobs
{
    internal class SportingBetJob(IScrapperFactory scrapperFactory, ILogger<SportingBetJob> logger) : IJob
    {

        IScrapperFactory _scrapperFactory = scrapperFactory;
        ILogger<SportingBetJob> _logger = logger;

        public async Task Execute(IJobExecutionContext context)
        {
            var sportsBookName = context.JobDetail.JobDataMap.GetString("sportsBookName");

            try
            {
                _logger.LogInformation("Starting the sporting bet scraping job at {time}", DateTime.Now);
                
                var scrapper = _scrapperFactory.GetScrapper(sportsBookName);
                var odds = await scrapper.GetFootballOddsAsync();
                foreach(var odd in odds)
                {
                    Console.WriteLine($"{odd.TeamOne} vs {odd.TeamTwo}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in the sportingbet scraping job");
            }
        }
    }
}
