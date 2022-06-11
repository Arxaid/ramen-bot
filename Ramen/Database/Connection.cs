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
    public static class Connection
    {
        private static string connectionString = "";
        public static void SetConnectionString(string host, int port, string database, string userid, string password)
        {
            connectionString = "server=" + host + 
                               ";database=" + database + 
                               ";port=" + port +
                               ";userid=" + userid + 
                               ";password=" + password;
        }
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}