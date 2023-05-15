using BracketGenerator.Models;
using BracketGenerator.Serivces;
using BracketGenerator.Serivces.Contracts;
using BracketGenerator.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Builder
{
    public class BracketBuilder : IBuilder
    {
        private Bracket _bracket = new Bracket();
        private Tournament _tournament;


        private readonly ITeamService _teamService;
        private readonly IMatchService _matchService;
        private readonly IEventService _eventService;
        private readonly IValidationManager _validationManager;

        public BracketBuilder(
            ITeamService teamService,
            IMatchService matchService,
            IEventService eventService,
            IValidationManager validationManager)
        {
            _teamService = teamService;
            _matchService = matchService;
            _eventService = eventService;
            _validationManager = validationManager;

            _tournament = new Tournament();

            Reset();
        }

        protected void Reset()
        {
            _bracket = new Bracket();
        }

        public void SetTournamentName(string name)
        {
            _bracket.SetName(name);
        }

        public void SeedTeams()
        {
            var tournament = _teamService.SeedTeams(_bracket.TournamentName);
            _tournament = tournament;
            _bracket.Teams = tournament.Teams;
        }

        public void AddEvents()
        {
            var events = _eventService.FetchEvents(_bracket.TournamentName);
            if (events != null)
            {
                var teamsPerGroup = _tournament.Teams.First().Value.Count;
                var totalTeams = teamsPerGroup * _tournament.Teams.Count;

                if (_validationManager.IsValidEvents(events, totalTeams))
                {
                    foreach (var group in events.Winners)
                    {
                        foreach (var winner in group)
                        {
                            _teamService.AdvanceTeam(winner);
                        }
                    }

                    if (events.Champion != null)
                        _teamService.AdvanceTeam(events.Champion);
                }
            }
        }

        public void SetWinner()
        {
            _bracket.Champion = _matchService.GetTournamentWinner(_tournament);
        }

        public void BuildPathToVictory()
        {
            _bracket.PathToVictorty = _matchService.PathToVictory(_tournament);
        }

        public Bracket GetBracket()
        {
            Bracket result = _bracket;
            Reset();
            return result;
        }
    }
}
