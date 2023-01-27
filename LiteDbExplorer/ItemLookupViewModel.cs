using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB;

namespace LiteDbExplorer
{
	public class ItemLookupViewModel: ObservableObject
    {
		private BsonDocument _doc;

        public ItemLookupViewModel()
		{ 
        }

        public void LoadData(BsonDocument doc)
		{
			_doc = doc;

			foreach( var d in _doc)
			{
                DocData.Add(new Tuple<string, string>(d.Key, d.Value.ToString()));
			}
        }

        public ObservableCollection<Tuple<string,string>> DocData { get; set; } = new ObservableCollection<Tuple<string, string>>();
	}
}

