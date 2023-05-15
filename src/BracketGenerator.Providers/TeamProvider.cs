using BracketGenerator.Models;
using BracketGenerator.Providers;
using BracketGenerator.Providers.Contracts;
using BracketGenerator.Utilities.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Providers
{
    public class TeamProvider : ProviderBase, ITeamProvider
    {
        private readonly IValidationManager _validationManager;

        public TeamProvider(IValidationManager validationManager)
        {
            _validationManager = validationManager;
        }

        public Participants? InitiateParticipants(string? tournamentName)
        {
            var jsonData = ReadSeed(tournamentName);

            try
            {
                if (jsonData != null)
                {
                    var data = JsonConvert.DeserializeObject<Participants>(jsonData);

                    if (_validationManager.IsValidParticipants(data))
                        return data;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
    }
}
