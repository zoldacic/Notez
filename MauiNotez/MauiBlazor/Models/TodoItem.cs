using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TodoSQLite.Models;

public class Tag
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public ImageSource Image { get; }
    public string Text { get; }
    
    public Tag() {}
    
    public Tag(string text)
    {
        Text = text;
    }
}

public class ItemTag
{
    public int ItemId { get; set; }
    public int TagId { get; set; }
}

public class TodoItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public DateTime AssignedDate { get; set; } = DateTime.Now;
    public string Name { get; set; }
    public string Notes { get; set; }
    public bool Done { get; set; }

    public BindingList<Tag> Tags { get; } = new BindingList<Tag>() { };
}
