using AutoMapper;
using BracketGenerator.Models;
using BracketGenerator.Providers.Contracts;
using BracketGenerator.Serivces;
using BracketGenerator.Serivces.Contracts;
using FakeItEasy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace BracketGenerator.Tests;

public class GrouplessBracketTest
{
    [Fact]
    public void SeedGrouplessTeamsTest()
    {
        // Arrange
        //var tournamentName = "FACup2020";
        var tournamentName = "LaLiga2023";
        string workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        var jsonData = File.ReadAllText(@$"{projectDirectory}\Data\{tournamentName}\Seed.json");
        var participants = JsonConvert.DeserializeObject<Participants>(jsonData);
        var groupTournament = true;
        SortedDictionary<int, SortedDictionary<int, Team>> dic = new();

        if (participants != null)
        {
            groupTournament = participants.Countries.All(c => c.Group != null);

            foreach (var c in participants.Countries)
            {
                dic.Add(c.Id, A.Dummy<SortedDictionary<int, Team>>());
            }
        }
            
        var teamProvider = A.Fake<ITeamProvider>();
        var mapper = A.Fake<IMapper>();
        var teamService = A.Fake<ITeamService>();
        
        A.CallTo(() => teamProvider.ReadSeed(tournamentName)).Returns(jsonData);
        A.CallTo(() => teamProvider.InitiateParticipants(tournamentName)).Returns(participants);
        A.CallTo(() => teamService.SeedTeams(tournamentName)).Returns(new Tournament()
        {
            IsGroupTournament = groupTournament,
            Teams = dic,
        });

        // Call
        var tournament = teamService.SeedTeams(tournamentName);

        // Assert
        Assert.IsType<Tournament>(tournament);
        Assert.NotNull(participants);
        Assert.NotNull(tournament);
        Assert.False(groupTournament);
        Assert.NotNull(tournament.Teams);
        Assert.Null(tournament.Winner);
        Assert.True(tournament.Teams.Count() == participants?.Countries.Count());
    }
}