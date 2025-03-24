using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OddScrapperService.Scrappers.Services.utils;
static class ParsingUtils
{
    public static string GenerateIdentifier(string teamOne, string teamTwo, DateOnly gameDay)
    {
       var t1 = Regex.Replace(teamOne, "[^a-zA-Z]", "").ToLowerInvariant()[..3];
       var t2 = Regex.Replace(teamTwo, "[^a-zA-Z]", "").ToLowerInvariant()[..3];
        
       return t1.CompareTo(t2) > 0 ? t2 + t1 + gameDay : t1 + t2 + gameDay;
    }

    public static float ParseOdds(string odds)
    {
        return float.Parse(odds, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}

