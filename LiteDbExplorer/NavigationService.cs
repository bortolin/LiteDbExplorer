using System;
using System.Diagnostics;
using LiteDB;

namespace LiteDbExplorer
{
    public interface INavigationService
    {
        void NavigateToItemLookupPage(BsonDocument doc);
    }

    public class NavigationService : INavigationService
    {
        readonly IServiceProvider _services;

        protected INavigation Navigation
        {
            get
            {
                INavigation navigation = Application.Current?.MainPage?.Navigation;                
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
                    Title = "Detail",
                    Page = page
                };

                Application.Current.OpenWindow(secondWindow);
            }
        }
    }
}