// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software.

namespace Ramen
{
    class Client
    {
        static void Main(string[] args)
        {
            var RamenBot = new Auth();
            RamenBot.RunAsync().GetAwaiter().GetResult();
        }
    }
}