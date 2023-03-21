using DevExpress.Maui.Editors;
using System.ComponentModel;
using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

[QueryProperty("Item", "Item")]
public partial class TodoItemPage : ContentPage
{
	TodoItem _item;
    TodoItemRepository _itemRepository;
    TagRepository _tagRepository;

    public TodoItem Item
	{
		get => BindingContext as TodoItem;
		set => BindingContext = value;
	}

    public TodoItemPage(TodoItemRepository itemRepository, TagRepository tagRepository)
    {
        InitializeComponent();
        _itemRepository = itemRepository;
        _tagRepository = tagRepository;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
            return;
        }

        await _itemRepository.SaveItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Item.ID == 0)
            return;
        await _itemRepository.DeleteItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    async Task tags_Completed(object sender, CompletedEventArgs e)
    {
        var chipGroup = sender as InputChipGroup;
        Console.WriteLine(chipGroup.EditorText);

        BindingList<Tag> list = chipGroup.ItemsSource as BindingList<Tag>;

        var tag = await _tagRepository.GetTagWithTextAsync(chipGroup.EditorText);

        if (tag != null)
        {
            list.Add(tag);
        }
        else
        {
            list.Add(new Tag(chipGroup.EditorText));
        }
        
        chipGroup.Chips.Last().IsRemoveIconVisible = true;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void tags_ChipRemoveIconClicked(object sender, ChipEventArgs e)
    {
        Console.WriteLine("Sender is:" + sender.GetType().Name);    
        //BindingList<ChipDataObject> list = chipGroup.ItemsSource as BindingList<ChipDataObject>;
        //list.Remove(e.Item as ChipDataObject);
    }
}