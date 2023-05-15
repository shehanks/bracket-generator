using AutoMapper;
using BracketGenerator.Models;
using BracketGenerator.Providers.Contracts;
using BracketGenerator.Serivces;
using BracketGenerator.Serivces.Contracts;
using BracketGenerator.Utilities.Helpers;
using FakeItEasy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace BracketGenerator.Tests;

public class GroupBracketTest
{
    [Fact]
    public void SeedGroupTeamsTest()
    {
        // Arrange
        var tournamentName = "FIFA2022";
        string workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        var jsonData = File.ReadAllText(@$"{projectDirectory}\Data\{tournamentName}\Seed.json");
        var participants = JsonConvert.DeserializeObject<Participants>(jsonData);
        var groupTournament = true;
        var groupCount = 0;
        var totalTeams = participants?.Countries.Count();
        SortedDictionary<int, SortedDictionary<int, Team>> dic = new();

        if (participants != null)
        {
            groupTournament = participants.Countries.All(c => c.Group != null);
            groupCount = participants.Countries.Select(g => g.Group).Distinct().Count();

            foreach (var c in participants.Countries)
            {
                dic.Add(c.Id, A.Dummy<SortedDictionary<int, Team>>());
            }
        }
        var teamsPerGroup = (totalTeams / (double)groupCount);

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
        Assert.True(groupTournament);
        Assert.True(groupCount.IsPowerOfTwo());
        Assert.True(totalTeams?.IsPowerOfTwo());
        Assert.True(teamsPerGroup?.IsPowerOfTwo());
        Assert.NotNull(tournament.Teams);
        Assert.Null(tournament.Winner);
        Assert.True(tournament.Teams.Count() == totalTeams);
    }
}