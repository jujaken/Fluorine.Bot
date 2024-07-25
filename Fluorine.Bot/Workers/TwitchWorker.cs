using Discord.WebSocket;
using Fluorine.Bot.Cfg;
using TwitchLib.Api;
using TwitchLib.Api.Services;

namespace Fluorine.Bot.Workers;

public class TwitchWorker(TwitchAPI api, DiscordSocketClient client, Config cfg) : BackgroundService
{
    private readonly TwitchAPI api = api;
    private readonly DiscordSocketClient client = client;
    private readonly Config cfg = cfg;

    private readonly LiveStreamMonitorService monitor = new(api, 60);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        api.Settings.ClientId = cfg.TwitchId;
        api.Settings.AccessToken = cfg.TwitchToken;

        monitor.SetChannelsById(cfg.TwitchChannels);
        monitor.OnStreamOnline += Monitor_OnStreamOnline;
        monitor.Start();
        
        await Task.Delay(-1, stoppingToken);
    }

    private void Monitor_OnStreamOnline(object? sender, TwitchLib.Api.Services.Events.LiveStreamMonitor.OnStreamOnlineArgs e)
    {
        var tittle = e.Stream.UserName;
        cfg.StreamChannels.ForEach(async id =>
        {
            var channel = client.GetGuild(cfg.DiscordGuild).GetTextChannel(id);
            if (channel != null)
            {
                await channel.SendMessageAsync(tittle);
            }
        });
    }
}
