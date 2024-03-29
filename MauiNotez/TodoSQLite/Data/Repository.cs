﻿using SQLite;
using TodoSQLite.Models;

namespace TodoSQLite.Data
{
    public static class Repository
    {
        public static SQLiteAsyncConnection Database;

        public static async Task InitAsync()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            await Database.CreateTableAsync<TodoItem>();
            await Database.CreateTableAsync<Tag>();
            await Database.CreateTableAsync<ItemTag>();
        }
    }
}
