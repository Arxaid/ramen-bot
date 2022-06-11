// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software

using System.Collections.Generic;

// DSharpPlus namespaces connect.
// These packages installing from NuGet. 
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace Ramen.Commands
{
    public class CustomHelpFormatter : BaseHelpFormatter
    {
        protected DiscordEmbedBuilder helpEmbed;

        public CustomHelpFormatter(CommandContext ctx) : base(ctx) 
        {
            // Embedded message content
            helpEmbed = new DiscordEmbedBuilder
            {
                Title = "Ramen Help",
                Color = DiscordColor.Black,
            };
        }
        public override BaseHelpFormatter WithCommand(Command cmd)
        {
            helpEmbed.AddField(cmd.Name, cmd.Description);
            return this;
        }
        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
        {
            helpEmbed.AddField("default prefix", "r!");
            foreach (var cmd in cmds)
            {
                helpEmbed.AddField(cmd.Name, cmd.Description);
            }
            return this;
        }
        public override CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: helpEmbed);
        }
    }
}