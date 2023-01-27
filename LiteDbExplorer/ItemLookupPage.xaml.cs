namespace LiteDbExplorer;

public partial class ItemLookupPage : ContentPage
{
	public ItemLookupPage(ItemLookupViewModel itemLookupViewModel)
	{
		InitializeComponent();
        BindingContext = itemLookupViewModel;
    }

	public ItemLookupViewModel ViewModel => (ItemLookupViewModel)BindingContext;
}