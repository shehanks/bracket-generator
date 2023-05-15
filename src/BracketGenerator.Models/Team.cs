using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Models
{
    public class Team
    {
        public int TeamId { get; set; }

        public string? Name { get; set; }

        public int GroupId { get; set; }

        public string? GroupName { get; set; }

        public int SeedNo { get; set; }

        public bool IsElemenated { get; set; }

        public Round LastRound { get; set; } = new Round();

        public IList<Round> Rounds { get; set; } = new List<Round>();
    }
}
