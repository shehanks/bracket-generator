using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Models
{
    public class Round : ICloneable
    {
        public int RoundId { get; set; }

        public int? MatchId { get; set; }

        public Team? Opponent { get; set; }

        public object Clone()
        {
            return new Round { RoundId = RoundId, MatchId = MatchId, Opponent = Opponent };
        }
    }
}
