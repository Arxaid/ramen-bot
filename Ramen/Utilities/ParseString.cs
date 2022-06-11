// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software.

namespace Ramen.Utilities
{
    public static class ParseString
    {
        public static string[] ParseID(string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                return new string[0];
            }

            return args.Trim().Replace("<@!", "").Replace("<@", "").Replace(">", "").Split();
        }
    }
}