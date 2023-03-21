using TodoSQLite.Models;

namespace TodoSQLite.Data
{
    public class TagRepository 
    {
        public TagRepository() 
        {
            Repository.InitAsync().GetAwaiter().GetResult();
        }

        public async Task<List<Tag>> GetTags()
        {
            return await Repository.Database.Table<Tag>().ToListAsync();
        }

        public async Task<Tag?> GetTagWithTextAsync(string text)
        {
            var tags = await Repository.Database.Table<Tag>().Where(t => t.Text == text).ToListAsync();
            return tags.FirstOrDefault();
        }
    }
}
