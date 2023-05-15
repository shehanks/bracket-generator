using BracketGenerator.Models;
using BracketGenerator.Providers.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BracketGenerator.Utilities.Helpers;

namespace BracketGenerator.Providers
{
    public abstract class ProviderBase : IBaseProvider
    {
        public string? ReadSeed(string? tournamentName)
        {
            return Read(tournamentName, "Seed");
        }

        public string? ReadEvents(string? tournamentName)
        {
            return Read(tournamentName, "Events");
        }

        private string? Read(string? tournamentName, string name)
        {
            if (tournamentName == null)
                return null;

            string workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;

            var filePath = @$"{projectDirectory}\Bracket\{tournamentName}\{name}.json";

            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(jsonData))
                    return jsonData;
            }

            return null;
        }
    }
}
