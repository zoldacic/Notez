using MauiBlazor.Data;
using MauiBlazor.Models;
using SQLite;
using System.Collections.Generic;
using System.ComponentModel;

namespace TodoSQLite.Data;
public class TodoItemRepository
{
    public async Task Init()
    {
        await Repository.InitAsync();
    }

    public async Task<List<TodoItem>> GetItemsAsync()
    {
        return await Repository.Database.Table<TodoItem>().ToListAsync();
    }

    public async Task<List<TodoItem>> GetItemsNotDoneAsync()
    {
        return await Repository.Database.Table<TodoItem>().Where(t => t.Done).ToListAsync();
    }

    public async Task<TodoItem> GetItemAsync(int id)
    {
        return await Repository.Database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task SaveItemsAsync(List<TodoItem> items, IList<Tag> tags)
    {
        foreach (var item in items) 
        {
            await SaveItemAsync(item, tags);
        }
    }

    public async Task SaveItemAsync(TodoItem item, IList<Tag> tags)
    { 
        if (item.ID != 0)
        {
            await Repository.Database.UpdateAsync(item);
        }
        else
        {
            await Repository.Database.InsertAsync(item);
        }

        var itemTagRepo = new ItemTagRepository();

        var currentTags = await itemTagRepo.GetItemTagsForItemAsync(item.ID);
        var tagsToDelete = currentTags.Where(ct => !tags.Any(t => t.ID == ct.TagId)).ToList();

        foreach (var tagToDelete in tagsToDelete)
        {
            await itemTagRepo.DeleteItemTagAsync(tagToDelete);
        }

        foreach (var tag in tags)
        {
            var itemTag = new ItemTag() { ItemId = item.ID, TagId = tag.ID };
            await itemTagRepo.SaveItemTagAsync(itemTag);
        }
    }

    public async Task<int> DeleteItemAsync(TodoItem item)
    {
        return await Repository.Database.DeleteAsync(item);
    }

    public async Task<int> DeleteAllTemporaryItemsAsync()
    {
        var items = (await GetItemsAsync()).Where(i => i.IsTemporary);

        var deleted = 0;
        foreach(var item in items)
        {
            deleted = deleted + await DeleteItemAsync(item);
        }

        return deleted;
    }

    public async Task<int> SetTemporaryItemsPermanentAsync()
    {
        var items = (await GetItemsAsync()).Where(i => i.IsTemporary);

        var deleted = 0;
        foreach (var item in items)
        {
            item.IsTemporary = false;
            await Repository.Database.UpdateAsync(item);
        }

        return deleted;
    }
}