using SecretSipsServer.DAOs;
using SecretSipsServer.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGameDAO, MemoryGameDAO>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseWebSockets();
app.MapGraphQL();

app.Run();
