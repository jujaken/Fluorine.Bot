using System.Text.Json;

namespace Fluorine.Bot.Cfg
{
    public class Config
    {
        public string BotDiscordToken { get; set; } = string.Empty;
        public List<ulong> StreamChannels { get; set; } = [];
        public ulong DiscordGuild{ get; set; }

        public string TwitchId { get; set; } = string.Empty;
        public string TwitchToken { get; set; } = string.Empty;
        public List<string> TwitchChannels { get; set; } = [];

        public static Config ReadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                var def = new Config();
                var json = JsonSerializer.Serialize(def);
                File.WriteAllText(filename, json);
                return def;
            }

            return JsonSerializer.Deserialize<Config>(File.ReadAllText(filename)) ?? throw new Exception("ебучий конфиг");
        }
    }
}
