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
    public static class Blacklist
    {
        public static bool IsBlacklisted(ulong userID)
        {
            using (MySqlConnection connection = Connection.GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM blacklist WHERE DiscordMemberID=@DiscordMemberID", connection);
                cmd.Parameters.AddWithValue("@DiscordMemberID", userID);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();
                if (result.Read()) { return true; }
                result.Close();
            }

            return false;
        }

        public static bool BlacklistAdd(ulong userID, ulong modID)
        {
            using (MySqlConnection connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(@"INSERT INTO blacklist(BlacklistedTime, DiscordMemberID, DiscordModeratorID) VALUES(UTC_TIMESTAMP(), @DiscordMemberID, @DiscordModeratorID);", connection);
                    cmd.Parameters.AddWithValue("@DiscordMemberID", userID);
                    cmd.Parameters.AddWithValue("@DiscordModeratorID", modID);
                    cmd.Prepare();

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
        }

        public static bool BlacklistDelete(ulong userID)
        {
            using (MySqlConnection connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(@"DELETE FROM blacklist WHERE DiscordMemberID=@DiscordMemberID", connection);
                    cmd.Parameters.AddWithValue("@DiscordMemberID", userID);
                    cmd.Prepare();

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
        }
    }
}