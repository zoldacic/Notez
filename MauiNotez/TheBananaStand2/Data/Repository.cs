using SQLite;
using TheBananaStand2.Models;


namespace TheBananaStand2.Data;

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

        try
        {
            await Database.CreateTableAsync<ItemTag>();
            //await FileHelper.CopyDatabaseToPublicFolderAsync();

        }
        catch (Exception ex) { }
    }
}

public static class FileHelper
{
    public static async Task CopyDatabaseToPublicFolderAsync()
    {
        string sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.DatabaseFilename);
        string destinationPath;

        // On Android, use the external storage directory
        destinationPath = "/storage/emulated/0/Documents/MauiBlazor.db";//Path.Combine(FileSystem.AppDataDirectory, Constants.DatabaseFilename);

        if (!File.Exists(destinationPath))
        {
            using var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
            await sourceStream.CopyToAsync(destinationStream);
        }
    }
}
