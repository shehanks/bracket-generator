using BracketGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Utilities.Validation
{
    public interface IValidationManager
    {
        bool IsValidParticipants(Participants? participants);

        bool IsValidTournamentStart(Tournament tournament);

        bool IsValidTournamentEnd(Tournament tournament);

        bool IsValidEvents(Events? events, int totalTeams);

        bool IsValidBracket(Bracket? bracket);
    }
}
