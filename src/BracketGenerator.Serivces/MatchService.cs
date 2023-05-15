using BracketGenerator.Models;
using BracketGenerator.Serivces.Contracts;
using BracketGenerator.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Serivces
{
    public class MatchService : IMatchService
    {
        private readonly IValidationManager _validationManager;

        public MatchService(IValidationManager validationManager)
        {
            _validationManager = validationManager;
        }

        public Team? GetTournamentWinner(Tournament tournament)
        {
            if (_validationManager.IsValidTournamentEnd(tournament))
                return tournament.Winner;

            return null;
        }

        public IList<string> PathToVictory(Tournament tournament)
        {
            List<string> paths = new();
            if (_validationManager.IsValidTournamentEnd(tournament))
            {
                var winner = tournament.Winner;
                var teams = tournament.Teams.Values.SelectMany(x => x.Values);

                if (winner != null)
                {
                    foreach (var round in winner.Rounds)
                    {
                        var opponent = round.Opponent;

                        if (tournament.IsGroupTournament)
                            paths.Add($"Round: {round.RoundId}, Match: {round.MatchId}, Group({winner.GroupName}) {winner.Name} Vs Group({opponent?.GroupName}) {opponent?.Name}");
                        else
                            paths.Add($"Round: {round.RoundId}, Match: {round.MatchId}, {winner.GroupName} {winner.Name} Vs {opponent?.Name}");
                    }

                    if (tournament.IsGroupTournament)
                        paths.Add($"Winner: {winner.Name}");
                    else
                        paths.Add($"Winner: {winner.Name}");
                }
            }

            return paths;
        }
    }
}
