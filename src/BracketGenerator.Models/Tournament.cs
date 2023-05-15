using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Models
{
    public class Tournament
    {
        public bool IsGroupTournament { get; set; }

        public SortedDictionary<int, SortedDictionary<int, Team>> Teams { get; set; } = new SortedDictionary<int, SortedDictionary<int, Team>>();

        public Team? Winner { get; set; }
    }
}
