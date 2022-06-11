// This file is part of the Ramen project.
//
// Copyright (c) 2020-2021 Vladislav Sosedov
// Copyright (c) 2021 INVE Studios
// 
// Ramen is open-source Discord bot application based on DSharpPlus.
// Feel free to use, copy, modify, merge, and publish this software.

using MySql.Data.MySqlClient;

namespace Ramen.Database
{
    public static class Tables
    {
        public static void SetupTables()
        {
            using (MySqlConnection connection = Connection.GetConnection())
            {
                MySqlCommand createBlacklist = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS blacklist(" +
                    "id INT UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT," +
                    "BlacklistedTime DATETIME NOT NULL," +
                    "DiscordMemberID BIGINT UNSIGNED NOT NULL," +
                    "DiscordModeratorID BIGINT UNSIGNED NOT NULL)",
                    connection);

                MySqlCommand createBan = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS ban(" +
                    "id INT UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT," +
                    "BanTime DATETIME NOT NULL," +
                    "DiscordMemberID BIGINT UNSIGNED NOT NULL," +
                    "DiscordModeratorID BIGINT UNSIGNED NOT NULL)",
                    connection);

                connection.Open();
                createBlacklist.ExecuteNonQuery();
                createBan.ExecuteNonQuery();
            }
        }
    }
}
