using BracketGenerator.Models;

namespace BracketGenerator.Providers.Contracts
{
    public interface IBaseProvider
    {
        string? ReadSeed(string tournamentName);

        string? ReadEvents(string tournamentName);
    }
}