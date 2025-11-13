using Cat_Bot.Commands.Slash;
using Cat_Bot.config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;

namespace Cat_Bot
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        static async Task Main(string[] args)
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(discordConfig);

            Client.UseInteractivity(new DSharpPlus.Interactivity.InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            //Task Handler Ready event
            Client.Ready += Client_Ready;
            /*
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDefaultHelp = false,
                EnableDms = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            */
            // Set up slash commands
            var slashCommandsConfig = Client.UseSlashCommands();
            slashCommandsConfig.RegisterCommands<BasicSC>();

            //Lavalink Configuration (necessary for the bot to play audio)
            /*
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "lava-v3.ajieblogs.eu.org",
                Port = 443,
                Secured = true
            };

            //Lavalink Config that will allow us to connect to the host above
            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "https://dsc.gg/ajidevserver",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };

            var lavalink = Client.UseLavalink();

            */
            // Connect to get the Bot online
            await Client.ConnectAsync();
            //await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
