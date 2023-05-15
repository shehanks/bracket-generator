using BracketGenerator.Models;
using BracketGenerator.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Utilities.Validation
{
    public class ValidationManager : IValidationManager
    {
        public bool IsValidTournamentStart(Tournament tournament)
        {
            if (tournament != null && tournament.Teams.Any() && tournament.Winner == null)
            {
                var counter = 1;

                if (!tournament.IsGroupTournament)
                {
                    var teams = tournament.Teams.Values.SelectMany(x => x.Values).OrderBy(y => y.TeamId);
                    foreach (var team in teams)
                    {
                        if (!IsValidTeam(team, counter, tournament.IsGroupTournament))
                            return false;

                        ++counter;
                    }

                    return true;
                }
                else
                {
                    var teamsPerGroup = tournament.Teams.First().Value.Count;

                    foreach (var group in tournament.Teams.Values)
                    {
                        if (group.Count != teamsPerGroup)
                            return false;

                        foreach (var team in group.Values)
                        {
                            if (!IsValidTeam(team, counter, tournament.IsGroupTournament))
                                return false;
                            ++counter;
                        }

                        counter = 1;
                    }

                    return true;
                }
            }

            return false;
        }

        public bool IsValidTournamentEnd(Tournament tournament)
        {
            if (tournament != null && tournament.Teams.Any() && tournament.Winner != null)
            {
                var teams = tournament.Teams.Values.SelectMany(x => x.Values).OrderBy(y => y.TeamId);

                if (teams.Count(x => !x.IsElemenated) != 1 ||
                    tournament.Winner.LastRound.MatchId != 1 ||
                    tournament.Winner.LastRound.RoundId == Math.Log2(teams.Count()))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsValidParticipants(Participants? participants)
        {
            if (participants != null && participants.Countries != null)
            {
                var groupCount = 0;
                var teamCount = participants.Countries.Count;
                var isNotGroupTournament = participants.Countries.All(x => x.Group == null);
                var isGroupTournament = participants.Countries.All(x => x.Group != null);

                if (isNotGroupTournament)
                    groupCount = 1;
                else if (isGroupTournament)
                {
                    var groupedParticipants = participants.Countries.GroupBy(o => o.GroupId).ToDictionary(g => g.Key, g => g.ToList());
                    groupCount = groupedParticipants.Count;
                }

                if (teamCount > 0 &&
                teamCount.IsPowerOfTwo() && groupCount.IsPowerOfTwo() && teamCount > groupCount &&
                (teamCount / (double)groupCount).IsPowerOfTwo())
                {
                    return true;
                }
            }
             
            return false;
        }

        public bool IsValidEvents(Events? events, int totalTeams)
        {
            var isValid = true;
            if (events != null && events.Winners.Any() && events.Champion != null)
            {
                var counter = 1;
                foreach (var item in events.Winners)
                {
                    if (item == null || item?.Count() != totalTeams / Math.Pow(2, counter))
                    {
                        isValid = false; 
                        break;
                    }
                    ++counter;
                }

                return isValid;
            }

            return false;
        }

        private bool IsValidTeam(Team team, int counter, bool isGroupTournament)
        {
            if (team.SeedNo != counter ||
                team.IsElemenated ||
                team.LastRound.RoundId != 1 ||
                team.LastRound.MatchId != (team.SeedNo / 2) + (team.SeedNo % 2) ||
                (isGroupTournament && team.GroupName == null))
            {
                return false;
            }
            return true;
        }

        public bool IsValidBracket(Bracket? bracket)
        {
            return bracket != null && 
                bracket.PathToVictorty != null && bracket.PathToVictorty.Any() && 
                bracket.Teams != null && bracket.Teams.Any();
        }
    }
}
