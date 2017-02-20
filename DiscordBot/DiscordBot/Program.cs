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
                x.LogHandler = ClientLog;
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
            // Ping command to test the bot is working
            CService.CreateCommand("ping")
                .Description("Check the bot is in channel")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("pong");
                    Log("[PING]...[PONG]");
                });
            // Send a message to the entire server
            #region BroadcastCommand
            CService.CreateCommand("broadcast")
                .Alias("Broadcast", "bc", "BC", "Announce")
                .Description("Broadcast/Announce something to all channels")
                .Parameter("Message", ParameterType.Required)
                .Do(async (e) =>
                {
                    // If the user is not an admin the alert them to the matter and deny them access
                    if (!isAdmin(e.User))
                    {
                        await e.Channel.SendMessage($"@{e.User.Name} You do not have access to that command!");
                        return;
                    }
                    //Console.WriteLine($"Broadcasting: \"{e.Args[0]}\" to channel: \"{e.Channel.Name}\"");
                    //await e.Channel.SendMessage($"[BROADCAST] {e.Args[0]}");
                    for (int i = 0; i < e.Server.ChannelCount; i++)
                    {
                        var ch = e.Server.TextChannels.ToArray()[i];
                        await ch.SendMessage($"[BROADCAST] {e.Args[0]}");
                        Log($"[BROADCASTING] {e.Args[0]} to channel: {ch.Name}");
                    }
                });
            #endregion
            // TODO:
            //  +Make warnings save >> Have an action after 3
            CService.CreateCommand("warn")
                .Description("Applies a warning to the users name, 3 strikes and they're out!")
                .Parameter("Target", ParameterType.Required)
                .Parameter("Reason", ParameterType.Optional)
                .Do(async (e) => 
                {
                    var Username = e.Args[0];
                    var Reason = "No reason supplied";
                    if (e.Args[1]!= null)
                    {
                        Reason = e.Args[1];
                    }
                    await e.Channel.SendMessage($"@{Username} You have been warned for: {Reason}");
                });
        }

        // Write to console the required log info
        public void ClientLog(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
        }

        public void Log(string Message)
        {
            Console.WriteLine(Message);
        }

        // Perform a check to see if the user is illegable to run certian admin commands
        // May require a seperate function for mod commands later on, for now this is fine.
        public bool isAdmin(User user)
        {
            string[] AccessRoles = { "Admin", "Co-Owner", "Owner", "Developer" };
            Role[] Roles = user.Roles.ToArray();
            // For every role see if it is in the AccessRoles
            for (int i = 0; i < Roles.Length; i++)
            {
                for (int x = 0; x < AccessRoles.Length; x++)
                {
                    if (AccessRoles[x] == Roles[i].ToString())
                    {
                        return true;
                    }
                }
            }
            // If i!=x not admin
            return false;
        }
    }
}
