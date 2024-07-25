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

        await FuckTwitch();

        await Task.Delay(-1, stoppingToken);
    }

    /// <summary>
    /// ебучий твич не авторизировал меня сука в этой хуйне блять
    /// отправить смс блять нормально не могут сука
    /// твич нигеры блять
    /// </summary>
    /// <returns></returns>
    private Task FuckTwitch()
    {
        while (true)
        {
            var guild = client.GetGuild(cfg.DiscordGuild);
            if (guild == null) continue;
            cfg.StreamChannels.ForEach(async id =>
            {
                var channel = guild.GetTextChannel(id);
                if (channel != null)
                    await channel.SendMessageAsync("твич пидарасы");
            });
            Thread.Sleep(5000);
        }
    }

    private Task Client_Log(LogMessage arg)
    {
        logger.LogInformation(arg.Message);
        return Task.CompletedTask;
    }
}
