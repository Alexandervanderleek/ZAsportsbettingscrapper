using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddScrapperService.Scrappers.Core
{
    internal interface IScrapperFactory
    {
        IScrapper GetScrapper(String scrapperName);
        IEnumerable<IScrapper> GetAllScrappers();
    }
}
