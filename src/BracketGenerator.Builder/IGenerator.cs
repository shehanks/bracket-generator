using BracketGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Builder
{
    public interface IGenerator
    {
        Bracket? BuildBracket(string tournamentName);
    }
}
