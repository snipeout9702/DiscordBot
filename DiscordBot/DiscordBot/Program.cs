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
            CService.CreateCommand("Google")
                .Alias("g", "G")
                .Description("Do a Google search for something");
        }

        // Write to console the required log info
        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
        }
    }
}
