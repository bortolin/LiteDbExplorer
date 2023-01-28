using System;
using System.Diagnostics;
using LiteDB;

namespace LiteDbExplorer.Services
{
    public interface INavigationService
    {
        void NavigateToItemLookupPage(BsonDocument doc);
    }

    /// <summary>
    /// Navigation service used inside ViewModel for open new window or navigate to page
    /// </summary>
    public class NavigationService : INavigationService
    {
        readonly IServiceProvider _services;

        protected INavigation Navigation
        {
            get
            {
                INavigation navigation = Application.Current.MainPage.Navigation;                
                return navigation;
            }
        }

        public NavigationService(IServiceProvider services)
            => _services = services;

        public void NavigateToItemLookupPage(BsonDocument doc)
        {
            var page = _services.GetService<ItemLookupPage>();
            page.ViewModel.LoadData(doc);

            if (page is not null)
            {
                var secondWindow = new Window
                {
                    //Set title not work on Mac :(
                    Title = "Detail",
                    Page = page,
                    // Position and dimension of window not work on Mac
                    Width = 900,
                    Height = 800
                };

                Application.Current.OpenWindow(secondWindow);

#if MACCATALYST
                //Workaround for Mac
                secondWindow.MinimumWidth = 900;
                secondWindow.MaximumWidth = 900;
                secondWindow.MinimumHeight = 800;
                secondWindow.MaximumHeight = 800;
#endif
            }
        }
    }
}