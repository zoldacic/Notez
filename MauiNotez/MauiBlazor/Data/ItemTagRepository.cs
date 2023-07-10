using TodoSQLite.Models;
using static MudBlazor.CategoryTypes;
using static System.Net.Mime.MediaTypeNames;

namespace MauiBlazor.Data;

public class ItemTagRepository
{
    public async Task Init()
    {
        await Repository.InitAsync();
    }

    public async Task<List<ItemTag>> GetItemTagsAsync()
    {
        return await Repository.Database.Table<ItemTag>().ToListAsync();
    }

    public async Task<List<ItemTag>> GetItemTagsForItemAsync(int itemId)
    {
        return await Repository.Database.Table<ItemTag>().Where(it => it.ItemId == itemId).ToListAsync();
    }

    public async Task SaveItemTagAsync(ItemTag itemTag)
    {
        var existingItemTag = await Repository.Database.Table<ItemTag>().Where(it => it.ItemId == itemTag.ItemId && it.TagId == itemTag.TagId).ToListAsync();

        if (!existingItemTag.Any())
        {
            await Repository.Database.InsertAsync(itemTag);
        }
    }

    public async Task<int> DeleteItemTagAsync(ItemTag itemTag)
    {
        return await Repository.Database.ExecuteAsync("DELETE FROM ItemTag WHERE TagId = ? AND ItemId = ?", itemTag.TagId, itemTag.ItemId);
    }
}
