using BracketGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Builder
{
    public interface IBuilder
    {
        void SetTournamentName(string name);

        void SeedTeams();

        void AddEvents();

        public void SetWinner();

        void BuildPathToVictory();

        Bracket GetBracket();
    }
}
