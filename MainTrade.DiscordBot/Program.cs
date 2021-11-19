using DSharpPlus;
using DSharpPlus.SlashCommands;
using Maintrade.DiscordBot.Commands;
using System;
using System.Threading.Tasks;

namespace Maintrade.DiscordBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var commands = discord.UseSlashCommands();

            commands.RegisterCommands<ExchangeCommand>();
            commands.RegisterCommands<PortfolioCommand>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
