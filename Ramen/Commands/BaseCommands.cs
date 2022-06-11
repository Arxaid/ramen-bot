// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software

using System.Linq;
using System.Threading.Tasks;

// DSharpPlus namespaces connect.
// These packages installing from NuGet. 
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Ramen.Commands
{
    public class BaseCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Default alive check.")]
        [Aliases("pong")]
        public async Task Ping(CommandContext ctx)
        {
            if (Database.Blacklist.IsBlacklisted(ctx.User.Id))
            {
                await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);
                return;
            }
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("permission")]
        [Description("Mod commands permission check.")]
        [Hidden]
        public async Task Test(CommandContext ctx)
        {
            if (Database.Blacklist.IsBlacklisted(ctx.User.Id))
            {
                await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);
                return;
            }

            if (ctx.Member.Roles.Any(Role => Role.Name == "/access Developer"))
            {
                await ctx.Channel.SendMessageAsync("Permission granted.").ConfigureAwait(false);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("You don't have permission to execute Mod commands.").ConfigureAwait(false);
            }
        }

        public DiscordEmbedBuilder errorEmbed = new DiscordEmbedBuilder()
        {
            Color = DiscordColor.Red,
            Description = "You have been banned from using Ramen."
        };
    }
}