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
using LiteDbExplorer.Services;

namespace LiteDbExplorer
{
	public class MainPageViewModel: ObservableObject
    {
        private LiteDatabase _ldb;
        private string _fileName;
        private string _filePath;
        private string _selectedCollection;
        private INavigationService _navigationService;
        private IFileService _fileService;
        private int _totalRecord;
        private double _fontSize;

        public MainPageViewModel(INavigationService navigationService, IFileService fileService)
		{
            _navigationService = navigationService;
            _fileService = fileService;

            LoadCollectionsCommand = new AsyncRelayCommand(LoadCollections);
            OpenDataCommand = new RelayCommand<BsonDocument>(OpenDetailData);
            ExportCollectionCommand = new AsyncRelayCommand(ExportCollection);
            CopyToClipboardCommand = new AsyncRelayCommand<BsonDocument>(async (bson) => { await Clipboard.Default.SetTextAsync(bson.ToString()); });
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

        /// <summary>
        /// Loads list of collection from litedb database
        /// </summary>
        private async Task LoadCollections(CancellationToken cancellationToken) {

            //Open file dialog
            var result = await _fileService.OpenFile();

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

        /// <summary>
        /// Load list of objects from LiteDb collection
        /// </summary>
        /// <param name="collName"></param>
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

        /// <summary>
        /// Open windows with details of object
        /// </summary>
        private void OpenDetailData(BsonDocument doc)
        {
            _navigationService.NavigateToItemLookupPage(doc);
        }

        /// <summary>
        /// Export collection to json file
        /// </summary>
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

                //Save file dialog
                await _fileService.SaveFile(stream, "Test.json", cancellationToken);
            }
        }
    }
}