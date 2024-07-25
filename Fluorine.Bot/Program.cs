using Discord.WebSocket;
using Fluorine.Bot.Cfg;
using Fluorine.Bot.Workers;
using TwitchLib.Api;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(Config.ReadFromFile("bin\\BotConfig.json"));

builder.Services.AddSingleton(new DiscordSocketClient());
builder.Services.AddSingleton(new TwitchAPI());

builder.Services.AddHostedService<DiscordWorker>();
builder.Services.AddHostedService<TwitchWorker>();

var host = builder.Build();
host.Run();
