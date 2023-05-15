using AutoMapper;
using BracketGenerator;
using BracketGenerator.Builder;
using BracketGenerator.Models;
using BracketGenerator.Providers;
using BracketGenerator.Providers.Contracts;
using BracketGenerator.Serivces;
using BracketGenerator.Serivces.Contracts;
using BracketGenerator.Utilities.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Type registration for DI.
        services.AddTransient<IValidationManager, ValidationManager>();
        services.AddTransient<ITeamProvider, TeamProvider>();
        services.AddTransient<IEventProvider, EventProvider>();
        services.AddTransient<IMatchService, MatchService>();
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<IEventService, EventService>();
        services.AddTransient<IBuilder, BracketBuilder>();
        services.AddTransient<IGenerator, Generator>();

        // Configure automapper.
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

    }).Build();

// Bracket generator.
var generator = host.Services.GetService<IGenerator>();

var bracket = generator?.BuildBracket("FACup2020");
bracket = generator?.BuildBracket("FederationCup2022");
bracket = generator?.BuildBracket("FIFA2022");
bracket = generator?.BuildBracket("LaLiga2023");
bracket = generator?.BuildBracket("UEFA2022");
