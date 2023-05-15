using BracketGenerator.Models;
using BracketGenerator.Providers.Contracts;
using BracketGenerator.Serivces.Contracts;
using BracketGenerator.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Serivces
{
    public class EventService : IEventService
    {
        private readonly IEventProvider _eventProvider;

        public EventService(IEventProvider eventProvider, IValidationManager validationManager)
        { 
            _eventProvider = eventProvider;
        }

        public Events? FetchEvents(string? tournamentName)
        {
            return _eventProvider.GetEvents(tournamentName);
        }
    }
}
