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
    public static class Ban
    {
        public static bool BanAdd(ulong userID, ulong modID)
        {
            using (MySqlConnection connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(@"INSERT INTO ban(BanTime, DiscordMemberID, DiscordModeratorID) VALUES(UTC_TIMESTAMP(), @DiscordMemberID, @DiscordModeratorID);", connection);
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

        public static bool BanDelete(ulong userID)
        {
            using (MySqlConnection connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(@"DELETE FROM ban WHERE DiscordMemberID=@DiscordMemberID", connection);
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