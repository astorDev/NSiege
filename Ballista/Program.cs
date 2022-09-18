var http = new HttpClient() { BaseAddress = new("http://localhost:5000") };
var client = new Castle.Client(http);

var random = new Random();

var names = new [] { "Alex", "George", "Angela", "Basil", "Bill" };
var namesFeed = Feed.CreateRandom("names", names);

var postCitizen = Step.Create("createCitizen",
    timeout: TimeSpan.FromMilliseconds(20000),
    feed: namesFeed,
    execute: async context => {
        var citizen = await client.PostCitizen(new (context.FeedItem));
        context.Data["citizenId"] = citizen.Id;
        return Response.Ok(citizen.Id);
    }
);

var craft = Step.Create("craft",
    timeout: TimeSpan.FromMilliseconds(1500),
    execute: async context => {
        var amount = random.Next(0, 1000);
        var citizenId = (string)context.Data["citizenId"];
        await client.PatchCitizenWood(citizenId, new Castle.WoodModification(Craft: amount));
        return Response.Ok();
    }
);

var burn = Step.Create("burn",
    timeout: TimeSpan.FromMilliseconds(500),
    execute: async context => {
        var amount = random.Next(0, 300);
        var citizenId = (string)context.Data["citizenId"];
        await client.PatchCitizenWood(citizenId, new Castle.WoodModification(Burn: amount));
        return Response.Ok();
    }   
);

var scenario = ScenarioBuilder
    .CreateScenario("wooding", postCitizen, craft, burn, craft, burn, craft, burn)
    .WithoutWarmUp()
    .WithLoadSimulations(
        Simulation.InjectPerSec(rate: 100, TimeSpan.FromSeconds(30))
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();