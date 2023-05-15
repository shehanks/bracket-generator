using AutoMapper;
using BracketGenerator.Models;
using BracketGenerator.Providers.Contracts;
using BracketGenerator.Serivces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Serivces
{
    public class TeamService : ITeamService
    {
        private readonly ITeamProvider _teamProvider;
        private readonly IMapper _mapper;
        private Tournament _tournament;

        public TeamService(ITeamProvider teamProvider, IMapper mapper)
        {
            _teamProvider = teamProvider;
            _mapper = mapper;
            _tournament = new Tournament();
        }

        public Tournament SeedTeams(string? tournamentName)
        {
            var allTeams = new SortedDictionary<int, SortedDictionary<int, Team>>();
            var participants = _teamProvider.InitiateParticipants(tournamentName);

            if (participants != null)
            {
                foreach (var country in participants.Countries)
                {
                    SeedTeam(allTeams, country);
                }
            }

            var tournament = new Tournament()
            {
                IsGroupTournament = allTeams.Count() > 1,
                Teams = allTeams
            };

            _tournament = tournament;

            return tournament;
        }

        public bool AdvanceTeam(string teamName)
        {
            if (_tournament.Teams.Any())
            {
                var team = _tournament.Teams
                .SelectMany(from => from.Value)
                .Select(team => team.Value).First(x => x.Name == teamName);

                if (team != null)
                    return AdvanceTeam(team);
            }

            return false;
        }

        private void SeedTeam(SortedDictionary<int, SortedDictionary<int, Team>> allTeams, Country country)
        {
            if (allTeams != null)
            {
                var team = _mapper.Map(country, new Team());
                var seed = team.SeedNo;
                var groupId = country.GroupId;

                team.LastRound.RoundId = 1;
                team.LastRound.MatchId = (seed / 2) + (seed % 2);

                if (allTeams.ContainsKey(groupId))
                    allTeams[groupId].Add(seed, team);
                else
                {
                    var el = new SortedDictionary<int, Team>
                    {
                        { seed, team }
                    };
                    allTeams.Add(groupId, el);
                }
            }
        }

        private bool AdvanceTeam(Team team)
        {
            if (!team.IsElemenated && _tournament.Winner == null)
            {
                var teamsPerGroup = _tournament.Teams.First().Value.Count;
                var totalTeams = teamsPerGroup * _tournament.Teams.Count;
                var totalRounds = Math.Log(totalTeams, 2);
                var currentRoundId = team.LastRound.RoundId;

                if (currentRoundId <= totalRounds)
                {
                    var groupTeam = _tournament.Teams.Where(x => x.Key == team.GroupId).First().Value.Select(x => x.Value).ToList();

                    if (groupTeam.Any() && groupTeam.Count(x => !x.IsElemenated) > 1)
                    {
                        ElemenateOpponent(groupTeam, team);
                        SetNextMatch(team);
                    }
                    else
                    {
                        var teams = _tournament.Teams.Values.SelectMany(group => group.Values).Where(x => !x.IsElemenated).ToList();

                        if (teams.Any() && teams.Count(x => !x.IsElemenated) > 1)
                        {
                            ElemenateOpponent(teams, team);
                            SetNextMatch(team);
                        }
                    }

                    ResetMatches(team);

                    if (currentRoundId == totalRounds)
                        _tournament.Winner = team;
                    else
                        team.LastRound.RoundId = currentRoundId + 1;

                    return true;
                }
            }

            return false;
        }

        private void ResetMatches(Team team)
        {
            var teamsPerGroup = _tournament.Teams.First().Value.Count;
            var totalTeams = teamsPerGroup * _tournament.Teams.Count;
            var currentRoundId = team.LastRound.RoundId;

            if (_tournament.IsGroupTournament &&
                        currentRoundId == Math.Log2(teamsPerGroup) &&
                        _tournament.Teams.Values.SelectMany(group => group.Values).Where(x => !x.IsElemenated).Count() == totalTeams / teamsPerGroup)
            {
                var groupWinners = _tournament.Teams.Values.SelectMany(group => group.Values).Where(x => !x.IsElemenated).ToList();
                for (int i = 0; i < totalTeams / teamsPerGroup / 2; i++)
                {
                    var matchId = i + 1;
                    groupWinners.ElementAt(i * 2).LastRound.MatchId = matchId;
                    groupWinners.ElementAt(i * 2 + 1).LastRound.MatchId = matchId;
                }
            }
        }

        private void SetNextMatch(Team team)
        {
            var currentMatchId = team.LastRound.MatchId;
            team.LastRound.MatchId = (currentMatchId / 2) + (currentMatchId % 2);
        }

        private void ElemenateOpponent(IList<Team> teams, Team team)
        {
            var opponent = teams.Where(x =>
                            x.LastRound.MatchId == team.LastRound.MatchId && x.TeamId != team.TeamId && !x.IsElemenated).First();
            opponent.IsElemenated = true;

            var r1 = (Round)team.LastRound.Clone();
            r1.Opponent = opponent;
            team.Rounds.Add(r1);

            var r2 = (Round)opponent.LastRound.Clone();
            r2.Opponent = team;
            opponent.Rounds.Add(r2);
        }
    }
}
