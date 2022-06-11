// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software

using System;
using System.Threading.Tasks;

// DSharpPlus namespaces connect.
// These packages installing from NuGet. 
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace Ramen.Commands
{
    [Group("access")]
    [Description("Ramen's commands member access.")]
    [RequirePermissions(Permissions.Administrator)]
    class BlacklistCommands : BaseCommandModule
    {
        [Command("blacklist")]
        [Description("Blocks member access to execute any command.")]
        [Aliases("revoke", "block")]
        public async Task Blacklist(CommandContext ctx, string cmdArgs)
        {
            string[] parsedArgs = Utilities.ParseString.ParseID(ctx.RawArgumentString);

            foreach(string parsedArg in parsedArgs)
            {
                if(ulong.TryParse(parsedArg, out ulong userID))
                {
                    // Checking user's availability.
                    DiscordUser currentUser = null;
                    try 
                    { 
                        currentUser = await ctx.Client.GetUserAsync(userID).ConfigureAwait(false); 
                    }
                    catch (NotFoundException) { }

                    if (currentUser == null)
                    {
                        var availabilityEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Red,
                            Description = "Couldn't find user in this guild."
                        };

                        await ctx.Channel.SendMessageAsync(embed: availabilityEmbed).ConfigureAwait(false);
                        continue;
                    }
                    
                    // Checking if user already blacklisted.
                    try
                    {
                        if (!Database.Blacklist.BlacklistAdd(userID, ctx.User.Id))
                        {
                            var checkEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Red,
                                Description = currentUser.Mention + " has already blacklisted."
                            };

                            await ctx.Channel.SendMessageAsync(embed: checkEmbed).ConfigureAwait(false);
                            continue;
                        }

                        var blacklistedEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Green,
                            Description = "Successfully blacklisted " + currentUser.Mention + "."
                        };
                        await ctx.Channel.SendMessageAsync(embed: blacklistedEmbed).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        var errorEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Red,
                            Description = "Undefined error."
                        };

                        await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);
                        throw;
                    }
                }
            }
        }

        [Command("unblacklist")]
        [Description("Unblocks member from blacklist.")]
        [Aliases("revive", "unblock")]
        public async Task Unblacklist(CommandContext ctx, string cmdArgs)
        {
            string[] parsedArgs = Utilities.ParseString.ParseID(ctx.RawArgumentString);

            foreach (string parsedArg in parsedArgs)
            {
                if (ulong.TryParse(parsedArg, out ulong userID))
                {
                    // Checking user's availability.
                    DiscordUser currentUser = null;
                    try
                    {
                        currentUser = await ctx.Client.GetUserAsync(userID).ConfigureAwait(false);
                    }
                    catch (NotFoundException) { }

                    if (currentUser == null)
                    {
                        var availabilityEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Red,
                            Description = "Couldn't find user in this guild."
                        };

                        await ctx.Channel.SendMessageAsync(embed: availabilityEmbed).ConfigureAwait(false);
                        continue;
                    }

                    // Checking if user isn't blacklisted.
                    try
                    {
                        if (!Database.Blacklist.BlacklistDelete(userID))
                        {
                            var checkEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Red,
                                Description = currentUser.Mention + " is not blacklisted."
                            };

                            await ctx.Channel.SendMessageAsync(embed: checkEmbed).ConfigureAwait(false);
                            continue;
                        }

                        var blacklistedEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Green,
                            Description = "Successfully removed " + currentUser.Mention + " from blacklist."
                        };
                        await ctx.Channel.SendMessageAsync(embed: blacklistedEmbed).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        var errorEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Red,
                            Description = "Undefined error."
                        };

                        await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);
                        throw;
                    }
                }
            }
        }
    }
}