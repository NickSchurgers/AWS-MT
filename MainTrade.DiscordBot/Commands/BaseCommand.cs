﻿using Amazon.Lambda;
using Amazon.Lambda.Model;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System.Text.Json;
using System.Threading.Tasks;

namespace MainTrade.DiscordBot.Commands
{
    internal abstract class BaseCommand : ApplicationCommandModule
    {
        public AmazonLambdaClient Client { protected get; set; }

        public CommandResultParser Parser { protected get; set; }

        protected async Task InvokeCommand(InteractionContext ctx, string[] command)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var request = new InvokeRequest { FunctionName = "CommandProcessor", Payload = JsonSerializer.Serialize(command) };
            var response = await Client.InvokeAsync(request);
            var parsedResult = await Parser.ParseResult(response.Payload);

            await ctx.EditResponseAsync(parsedResult);
        }
    }
}
