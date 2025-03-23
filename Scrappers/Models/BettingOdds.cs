using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddScrapperService.Scrappers.Models;

internal class BettingOdds
{
    public required string SportsBook;
    public required string MatchIdentifier;
    public required string TeamOne;
    public required string TeamTwo;
    public float TeamOneOdds;
    public float TeamTwoOdds;
    public DateOnly MatchDate;
}

