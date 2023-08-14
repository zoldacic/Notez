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

    public async Task<List<TodoItem>> GetTemporaryItemsAsync()
    {
        return await Repository.Database.Table<TodoItem>().Where(t => t.IsTemporary).ToListAsync();
    }

    public async Task<TodoItem> GetItemAsync(int id)
    {
        return await Repository.Database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task SaveItemAsync(TodoItem item, IList<Tag> tags)
    {
        var itemTagRepo = new ItemTagRepository();

        if (item.ID != 0)
        {
            await Repository.Database.UpdateAsync(item);

            var currentTags = await itemTagRepo.GetItemTagsForItemAsync(item.ID);
            var tagsToDelete = currentTags.Where(ct => !tags.Any(t => t.ID == ct.TagId)).ToList();

            foreach (var tagToDelete in tagsToDelete)
            {
                await itemTagRepo.DeleteItemTagAsync(tagToDelete);
            }
        }
        else
        {
            await Repository.Database.InsertAsync(item);

            // Read back with Id set
            item = await Repository.Database.Table<TodoItem>().OrderByDescending(t => t.ID).FirstOrDefaultAsync();
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
        var sql = "DELETE FROM TODOITEM WHERE ISTEMPORARY = TRUE";
        return await Repository.Database.ExecuteAsync(sql);

        //var items = (await GetItemsAsync()).Where(i => i.IsTemporary);

        //var deleted = 0;
        //foreach(var item in items)
        //{
        //    deleted = deleted + await DeleteItemAsync(item);
        //}

        //return deleted;
    }

    public async Task CreateTemporaryItemsAsync(List<TodoItem> items, IList<Tag> tags)
    {
        await Repository.Database.InsertAllAsync(items);

        items = (await GetItemsAsync()).Where(i => i.IsTemporary).ToList();

        var itemTags = new List<ItemTag>();
        foreach (var tag in tags)
        {
            itemTags.AddRange(items.Select(i => new ItemTag() { ItemId = i.ID, TagId = tag.ID }));
        }

        await Repository.Database.InsertAllAsync(itemTags);
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