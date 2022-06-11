// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software.

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

// DSharpPlus namespaces connect.
// These packages installing from NuGet. 
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;

using Ramen.Commands;
using Ramen.Database;

namespace Ramen
{
    class Auth
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            var configJson = JsonConvert.DeserializeObject<Utilities.ConfigJson>(json);

            var clientConfig = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };
            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                DmHelp = false,
                EnableMentionPrefix = true
            };
            var interactivityConfig = new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(1)
            };

            Connection.SetConnectionString(configJson.DBHost, 3306, configJson.DBName, configJson.DBUser, configJson.DBPassword);
            Tables.SetupTables();

            Client = new DiscordClient(clientConfig);
            Client.Ready += OnClientReady;
            Client.UseInteractivity(interactivityConfig);

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.SetHelpFormatter<CustomHelpFormatter>();
            Commands.RegisterCommands<BaseCommands>();
            Commands.RegisterCommands<ModCommands>();
            Commands.RegisterCommands<BlacklistCommands>();

            // RunAsync await delay allows to keep bot always working.
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            Client.UpdateStatusAsync(new DiscordActivity("r!help", ActivityType.Watching), UserStatus.DoNotDisturb);
            return Task.CompletedTask;
        }
    }
}