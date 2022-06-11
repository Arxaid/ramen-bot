// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software

using System;
using System.Linq;
using System.Threading.Tasks;

// DSharpPlus namespaces connect.
// These packages installing from NuGet. 
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Ramen.Commands
{
    [Group("mod")]
    [Description("Commands requires Moderator access.")]
    [RequirePermissions(Permissions.Administrator)]
    public class ModCommands : BaseCommandModule
    {
        [Command("join")]
        [Description("Sends embed w/ role by react ineractivity.")]
        public async Task Join(CommandContext ctx)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Welcome aboard",
                ImageUrl = "https://i.imgur.com/1ugyHpX.png",
                Description = "If you want to join us, react w/ stov emoji",
                Color = DiscordColor.Black
            };
            var joinMessage = await ctx.Channel.SendMessageAsync(embed: joinEmbed).ConfigureAwait(false);

            var stovEmoji = DiscordEmoji.FromName(ctx.Client, ":stovlogo:");
            await joinMessage.CreateReactionAsync(stovEmoji).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();
            var reactionResult = await interactivity.WaitForReactionAsync(result =>
                (result.Message == joinMessage) &&
                (result.User == ctx.User) &&
                (result.Emoji == stovEmoji)).ConfigureAwait(false);

            if (reactionResult.Result.Emoji == stovEmoji)
            {
                var addRole = ctx.Guild.GetRole(741681957506777129);
                var deleteRole = ctx.Guild.GetRole(741682039971119206);

                await ctx.Member.GrantRoleAsync(addRole).ConfigureAwait(false);
                await ctx.Member.RevokeRoleAsync(deleteRole).ConfigureAwait(false);
                await ctx.Channel.DeleteMessageAsync(joinMessage).ConfigureAwait(false);
            }
        }

        [Command("mail")]
        [Description("Sends mail to all guild members.")]
        public async Task Mail(CommandContext ctx)
        {
            var confirmEmbed = new DiscordEmbedBuilder
            {
                Title = "Confirm DM",
                Description = "`r!mail` will send DM to everyone in this Guild",
                Color = DiscordColor.Black
            };
            var contentEmbed = new DiscordEmbedBuilder
            {
                Title = "Test Message",
                Description = "Roses are red,\n My name is Dave.\n This poem has no sense.\n Microwave.",
                Color = DiscordColor.Black
            };

            var mailMessage = await ctx.Channel.SendMessageAsync(embed: confirmEmbed).ConfigureAwait(false);

            var confirmEmoji = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
            await mailMessage.CreateReactionAsync(confirmEmoji).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();
            var reactionResult = await interactivity.WaitForReactionAsync(result =>
                (result.Message == mailMessage) &&
                (result.User == ctx.User) &&
                (result.Emoji == confirmEmoji)).ConfigureAwait(false);

            if (reactionResult.Result.Emoji == confirmEmoji)
            {
                var confirmRole = ctx.Guild.GetRole(741681957506777129);
                var members = await ctx.Guild.GetAllMembersAsync().ConfigureAwait(false);

                foreach (var member in members.Where(member => member.Roles.Contains(confirmRole)))
                {
                    var memberChannel = await member.CreateDmChannelAsync().ConfigureAwait(false);
                    await memberChannel.SendMessageAsync(embed: contentEmbed).ConfigureAwait(false);
                }
            }
        }

        [Command("sudo")]
        [Description("Executes any command as another user.")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Sudo(CommandContext ctx, DiscordMember member, string command)
        {
            var cmdNext = ctx.CommandsNext;
            var cmd = cmdNext.FindCommand(command, out var customArgs);
            var fakeContext = cmdNext.CreateFakeContext(member, ctx.Channel, command, ctx.Prefix, cmd, customArgs);

            await cmdNext.ExecuteCommandAsync(fakeContext).ConfigureAwait(false);
        }

        [Command("ban")]
        [Description("Ban member from this guild.")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Ban(CommandContext ctx, DiscordMember member)
        {
            if (member == ctx.User)
            {
                var safetyEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "You can not ban yourself."
                };
                await ctx.Channel.SendMessageAsync(embed: safetyEmbed).ConfigureAwait(false);
                return;
            }

            try
            {
                if (!Database.Ban.BanAdd(member.Id, ctx.User.Id))
                {
                    var checkEmbed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Red,
                        Description = "User has already banned."
                    };

                    await ctx.Channel.SendMessageAsync(embed: checkEmbed).ConfigureAwait(false);
                }

                var banEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Green,
                    Description = "Successfully banned " + member.Mention + "."
                };

                await ctx.Guild.BanMemberAsync(member, 0).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync(embed: banEmbed).ConfigureAwait(false);
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

        [Command("unban")]
        [Description("Unban member from this guild.")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Unban(CommandContext ctx, DiscordUser member)
        {
            try
            {
                if (!Database.Ban.BanDelete(member.Id))
                {
                    var checkEmbed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Red,
                        Description = "User wasn't banned."
                    };

                    await ctx.Channel.SendMessageAsync(embed: checkEmbed).ConfigureAwait(false);

                }

                var banEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Green,
                    Description = "Successfully revoked " + member.Mention + " ban."
                };

                await ctx.Guild.UnbanMemberAsync(member, "unban").ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync(embed: banEmbed).ConfigureAwait(false);
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