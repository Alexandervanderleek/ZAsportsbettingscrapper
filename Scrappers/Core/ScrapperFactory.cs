using OddScrapperService.Scrappers.Services.BettingSites;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddScrapperService.Scrappers.Core
{
    internal class ScrapperFactory : IScrapperFactory
    {

        IServiceProvider _serviceProvider;

        public ScrapperFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IScrapper GetScrapper(string scrapperName)
        {
            return (scrapperName) switch
            {
                "SportingBet" => _serviceProvider.GetRequiredService<SportingBetScrapper>(),
                _ => throw new ArgumentNullException()
            }; 
        }

        public IEnumerable<IScrapper> GetAllScrappers()
        {
            throw new NotImplementedException();
        }


    }
}
