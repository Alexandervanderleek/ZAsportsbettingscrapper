using OddScrapperService.Scrappers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddScrapperService.Scrappers.Core;
internal interface IScrapper
{
    public Task<IEnumerable<BettingFootballOdds>> GetFootballOddsAsync();
}

