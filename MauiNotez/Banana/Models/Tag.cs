using SQLite;

namespace MauiBlazor.Models
{
    public class Tag
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public ImageSource Image { get; }
        public string Text { get; set; }
        public string? Value { get; set; }

        public Tag() { }

        public Tag(string text)
        {
            Text = text;
        }
    }
}
