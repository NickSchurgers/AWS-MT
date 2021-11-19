using Amazon.Lambda;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using MainTrade.DiscordBot.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MainTrade.DiscordBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
            });

            var commands = discord.UseSlashCommands(new SlashCommandsConfiguration { 
                Services = GetServices()
            });

            commands.RegisterCommands<ExchangeCommand>();
            commands.RegisterCommands<PortfolioCommand>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static IServiceProvider GetServices()
        {
            var services = new ServiceCollection()
                .AddSingleton<AmazonLambdaClient>()
                .AddSingleton<CommandResultParser>();

            return services.BuildServiceProvider();
        }
    }
}
