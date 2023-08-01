using MauiBlazor.Models;

namespace MauiBlazor.Data;

public class TagRepository
{
    public async Task Init()
    {
        await Repository.InitAsync();
    }

    public async Task<List<Tag>> GetTagsAsync()
    {
        await Init();
        return await Repository.Database.Table<Tag>().ToListAsync();
    }

    public async Task<Tag> GetTagWithTextAsync(string text)
    {
        await Init();
        var tags = await Repository.Database.Table<Tag>().Where(t => t.Text == text).ToListAsync();
        return tags.FirstOrDefault();
    }

    public async Task<Tag> GetTagWithValueAsync(string value)
    {
        await Init();
        var tags = await Repository.Database.Table<Tag>().Where(t => t.Value == value).ToListAsync();
        return tags.FirstOrDefault();
    }

    public async Task SaveTagAsync(Tag tag)
    {
        var otherTagWithSameText = await GetTagWithTextAsync(tag.Text);
        
        if (otherTagWithSameText != null && otherTagWithSameText.ID != tag.ID)
        {
            throw new Exception($"Tag with text {tag.Text} already exists");
        }

        if (tag.ID != 0)
        {
            await Repository.Database.UpdateAsync(tag);
        }
        else
        {
            await Repository.Database.InsertAsync(tag);
        }
    }

    public async Task<int> DeleteTagAsync(Tag tag)
    {
        return await Repository.Database.DeleteAsync(tag);
    }
}
