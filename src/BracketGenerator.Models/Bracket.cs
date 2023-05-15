using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Models
{
    public class Bracket
    {
        public string? TournamentName { get; set; }

        public Team? Champion { get; set; }

        public SortedDictionary<int, SortedDictionary<int, Team>>? Teams { get; set; }

        public IList<string>? PathToVictorty { get; set; }


        public void SetName(string tournamentName)
        {
            TournamentName = tournamentName;
        }
    }
}
