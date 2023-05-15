using BracketGenerator.Models;
using BracketGenerator.Utilities.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Builder
{
    public class Generator : IGenerator
    {
        private IBuilder? _builder;
        IValidationManager _validationManager;
        private readonly ILogger<Generator> _logger;

        public Generator(IBuilder? builder, IValidationManager validationManager, ILogger<Generator> logger)
        {
            _builder = builder;
            _logger = logger;
            _validationManager = validationManager;
        }

        public Bracket? BuildBracket(string tournamentName)
        {
            try
            {
                if (tournamentName != null)
                {
                    _builder?.SetTournamentName(tournamentName);
                    _builder?.SeedTeams();
                    _builder?.AddEvents();
                    _builder?.SetWinner();
                    _builder?.BuildPathToVictory();

                    var bracket = _builder?.GetBracket();

                    LogBracket(bracket);

                    return bracket;
                }

                _logger.LogError($"Bad request. Please check and add the correct tournament.");
                return null;
                
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Message: {ex.Message} \n " +
                    $"Inner exception: {ex.InnerException} \n " +
                    $"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        private void LogBracket(Bracket? bracket)
        {
            if (_validationManager.IsValidBracket(bracket))
            {
                var allTeams = bracket?.Teams?.Values.SelectMany(x => x.Values).ToList();

                if (bracket?.PathToVictorty != null)
                    _logger.LogInformation("\n" +
                        $"================== {bracket.TournamentName} ==================" +
                        "\n" +
                        $"Teams - {allTeams?.Count}" +
                        $"\n" +
                        $"Champion - {bracket.Champion?.Name}" +
                        $"\n\n" +
                        $"----- Path to Victory -----" +
                        $"\n" +
                        $"{string.Join("\n", bracket.PathToVictorty)}" +
                        $"\n" +
                        $"==============================================================" +
                        $"\n\n");
            }
            else
            {
                _logger.LogError($"Invalid tournament {bracket?.TournamentName} or error file on seed/advance data feed.");
            }
        }
    }
}
