using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB;

namespace LiteDbExplorer
{
	public class MainPageViewModel: ObservableObject
    {
        private LiteDatabase _ldb;
        private string _fileName;
        private string _filePath;
        private string _selectedCollection;
        private INavigationService _navigationService;
        private int _totalRecord;
        private double _fontSize;

        public MainPageViewModel(INavigationService navigationService)
		{
            _navigationService = navigationService;

            LoadCollectionsCommand = new AsyncRelayCommand(LoadCollections);
            OpenDataCommand = new RelayCommand<BsonDocument>(OpenData);
            CopyToClipboardCommand = new AsyncRelayCommand<BsonDocument>(CopyToClipboard);
            ExportCollectionCommand = new AsyncRelayCommand(ExportCollection);
            ExitCommand = new RelayCommand(() => { Application.Current.Quit(); });

        }

        public ICommand LoadCollectionsCommand { get; }
        public ICommand OpenDataCommand { get; }
        public ICommand CopyToClipboardCommand { get; }
        public ICommand ExportCollectionCommand { get; }
        public ICommand ExitCommand { get; }

        public string FileName { get => _fileName; set=> SetProperty(ref _fileName,value); }
        public string FilePath { get => _filePath; set => SetProperty(ref _filePath, value); }
        public int TotalRecord { get => _totalRecord; set => SetProperty(ref _totalRecord,value); }

        public ObservableCollection<string> CollectionsNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<object> CollectionData { get; set; } = new ObservableCollection<object>();

        public string SelectedCollection {
            get => _selectedCollection;
            set {
                SetProperty(ref _selectedCollection, value);
                if (!string.IsNullOrWhiteSpace(_selectedCollection))
                {
                    LoadCollectionData(_selectedCollection);
                }
            }
        }

        private async Task LoadCollections(CancellationToken cancellationToken) {

            var result = await FilePicker.Default.PickAsync();

            if (result != null)
            {
                FileName = result.FileName;
                FilePath = result.FullPath;

                _ldb = new LiteDB.LiteDatabase($"Filename={FilePath}");

                foreach (var coll in _ldb.GetCollectionNames())
                {
                    CollectionsNames.Add(coll);
                }
            }
        }

        private void LoadCollectionData(string collName)
        {
            CollectionData.Clear();

            var c = _ldb.GetCollection(collName);
            var data = c.FindAll();

            foreach (var r in data)
            {
                CollectionData.Add(r);
            }

            TotalRecord = data.Count();
        }

        private void OpenData(BsonDocument doc)
        {
            _navigationService.NavigateToItemLookupPage(doc);
        }

        private async Task CopyToClipboard(BsonDocument bson)
        {
            var jsonstring = bson.ToString();
            await Clipboard.Default.SetTextAsync(jsonstring);
        }

        private async Task SaveFile(Stream stream,string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var fileLocation = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);
                await Toast.Make($"File is saved: {fileLocation}").Show(cancellationToken);
            }
            catch (Exception ex)
            {
                await Toast.Make($"File is not saved, {ex.Message}").Show(cancellationToken);
            }
        }

        private async Task ExportCollection(CancellationToken cancellationToken)
        {
            var col = _ldb.GetCollection(SelectedCollection);
            var count = col.Count();

            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("[");

                foreach (var r in col.FindAll())
                {
                    count -= 1;
                    writer.Write(r.ToString());
                    if (count > 0) writer.Write(",");
                }
                writer.Write("]");
                writer.Flush();
                stream.Position = 0;

                await SaveFile(stream, "Test.json", cancellationToken);
            }
        }
    }
}