using Discord;
using Discord.WebSocket;
using Fluorine.Bot.Cfg;

namespace Fluorine.Bot.Workers;

public class DiscordWorker(DiscordSocketClient client, ILogger<DiscordWorker> logger, Config cfg) : BackgroundService
{
    private readonly DiscordSocketClient client = client;
    private readonly ILogger<DiscordWorker> logger = logger;
    private readonly Config cfg = cfg;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await client.LoginAsync(TokenType.Bot,cfg.BotDiscordToken);
        await client.StartAsync();

        client.Log += Client_Log;

        await Task.Delay(-1, stoppingToken);
    }

    private Task Client_Log(LogMessage arg)
    {
        logger.LogInformation(arg.Message);
        return Task.CompletedTask;
    }
}
