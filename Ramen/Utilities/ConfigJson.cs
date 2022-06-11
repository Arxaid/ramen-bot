// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software.

using Newtonsoft.Json;

namespace Ramen.Utilities
{
    public struct ConfigJson
    {
        [JsonProperty("botToken")]
        public string Token { get; private set; }

        [JsonProperty("botPrefix")]
        public string Prefix { get; private set; }

        [JsonProperty("databaseHost")]
        public string DBHost { get; private set; }

        [JsonProperty("databaseName")]
        public string DBName { get; private set; }

        [JsonProperty("databaseUser")]
        public string DBUser { get; private set; }

        [JsonProperty("databasePassword")]
        public string DBPassword { get; private set; }
    }
}