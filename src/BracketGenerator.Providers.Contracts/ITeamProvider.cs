using BracketGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Providers.Contracts
{
    public interface ITeamProvider : IBaseProvider
    {
        Participants? InitiateParticipants(string? tournamentName);
    }
}
