namespace LiteDbExplorer;

public partial class App : Application
{
	public App(MainPage mainPage)
	{
		InitializeComponent();

		//On Windows need Navigation Bar for draw top Menu
#if MACCATALYST
		MainPage = mainPage;
#else
		MainPage = new NavigationPage(mainPage);
#endif
    }
}