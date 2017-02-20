using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }

        DiscordClient Client;

        public void Start()
        {
            Client = new DiscordClient(x =>
            {
                x.AppName = "DiscordBot";
                x.AppUrl = "";
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            Client.UsingCommands(x =>
            {
                x.PrefixChar = char.Parse("!");
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Public;
            });

            string Token = "MjgyOTkwMzg4NjkwNTUwNzg1.C4vVxg.5mGFuAKxUkEBknIRfZDBfD4Y_-U";

            CreateCommands();

            Client.ExecuteAndWait(async () =>
            {
                await Client.Connect(Token, TokenType.Bot);
            });
        }

        public void CreateCommands()
        {
            var CService = Client.GetService<CommandService>();
            CService.CreateCommand("ping")
                .Description("Check the bot is in channel")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("pong");
                });
            CService.CreateCommand("Broadcast")
                .Alias("bc", "BC", "Announce")
                .Description("Broadcast/Announce something to all channels")
                .Parameter("Message", ParameterType.Required)
                .Do(async (e) =>
                {
                    //Console.WriteLine($"Broadcasting: \"{e.Args[0]}\" to channel: \"{e.Channel.Name}\"");
                    //await e.Channel.SendMessage($"[BROADCAST] {e.Args[0]}");
                    for (int i = 0; i < e.Server.ChannelCount; i++)
                    {
                        var ch = e.Server.TextChannels.ToArray()[i];
                        await ch.SendMessage($"[BROADCAST] {e.Args[0]}");
                        Console.WriteLine($"[BROADCASTING] {e.Args[0]} to channel: {ch.Name}");
                    }
                });
        }

        // Write to console the required log info
        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
        }
    }
}
