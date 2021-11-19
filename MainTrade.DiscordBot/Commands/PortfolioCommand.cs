﻿using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;

namespace Maintrade.DiscordBot.Commands
{
    public class PortfolioCommand : ApplicationCommandModule
    {
        [SlashCommand("portfolio", "Generate a portfolio")]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            await ctx.EditResponseAsync(new DSharpPlus.Entities.DiscordWebhookBuilder().WithContent($"I deferred this message, bro!{Environment.NewLine}This C# lib seems to be working without bs. Just need the redo the ec2 lol."));
        }
    }
}
