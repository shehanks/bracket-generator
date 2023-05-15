using BracketGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Serivces.Contracts
{
    public interface IMatchService
    {
        Team? GetTournamentWinner(Tournament tournament);

        public IList<string> PathToVictory(Tournament tournament);
    }
}
