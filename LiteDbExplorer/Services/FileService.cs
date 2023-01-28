using System;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using LiteDB;

namespace LiteDbExplorer.Services
{
    public interface IFileService
    {
        Task<FileResult?> OpenFile();
        Task SaveFile(Stream stream, string fileName, CancellationToken cancellationToken);
    }

    public class FileService: IFileService
	{
		public FileService()
		{
		}

        public Task<FileResult?> OpenFile()
        {
            return FilePicker.Default.PickAsync();
        }

        /// <summary>
        /// Save file dialog using FileSaver from CommunityToolkit
        /// </summary>
        public async Task SaveFile(Stream stream, string fileName, CancellationToken cancellationToken)
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
    }
}