using System.Data;
using Microsoft.Maui;
using Microsoft.Maui.Storage;

namespace LiteDbExplorer;

public partial class MainPage : ContentPage
{
    private double _fontSize;

	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

        _fontSize = 24;
        this.Resources["dataFont"] = _fontSize;
    }

    public MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;

    void IncraseFontButton_Clicked(System.Object sender, System.EventArgs e)
    {
        _fontSize += 4;
        this.Resources["dataFont"] = _fontSize;
    }

    void DecraseFontButton_Clicked(System.Object sender, System.EventArgs e)
    {
        _fontSize -= 4;
        this.Resources["dataFont"] = _fontSize;
    }
}