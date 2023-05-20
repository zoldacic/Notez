using MauiBlazor.Data;
using SQLite;
using System.ComponentModel;
using TodoSQLite.Models;

namespace TodoSQLite.Data;
public class TodoItemRepository
{
    public TodoItemRepository() 
    {
    }

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
        
        // SQL queries are also possible
        //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    }

    public async Task<TodoItem> GetItemAsync(int id)
    {
        return await Repository.Database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(TodoItem item)
    {
        if (item.ID != 0)
        {
            await Repository.Database.UpdateAsync(item);
        }
        else
        {
            await Repository.Database.InsertAsync(item);
        }

        foreach (var tag in item.Tags)
        {
            if (tag.ID == 0)
            {
                await Repository.Database.InsertAsync(tag);
            }

            if (await Repository.Database.Table<ItemTag>().CountAsync(it => it.ItemId == item.ID && it.TagId == tag.ID) == 0)
            {
                var itemTag = new ItemTag() { ItemId = item.ID, TagId = tag.ID };
                await Repository.Database.InsertAsync(itemTag);
            }
        }

        return 0;
    }

    public async Task<int> DeleteItemAsync(TodoItem item)
    {
        return await Repository.Database.DeleteAsync(item);
    }
}