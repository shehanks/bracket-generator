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
    public class EventProvider : ProviderBase, IEventProvider
    {
        public Events? GetEvents(string? tournamentName)
        {
            var jsonData = ReadEvents(tournamentName);

            try
            {
                if (jsonData != null)
                {
                    return JsonConvert.DeserializeObject<Events>(jsonData);
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
