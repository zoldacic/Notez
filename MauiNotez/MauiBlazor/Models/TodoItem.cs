using SQLite;

namespace MauiBlazor.Models;

public class TodoItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public DateTime? AssignedDate { get; set; } = DateTime.Now;
    public string Name { get; set; }
    public string Notes { get; set; }
    public bool Done { get; set; }
    public double? TransactionAmount { get; set; }
}
