using CommunityToolkit.WinUI.Helpers;
using PnPOrganizer.Services.Data;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PnPOrganizer.Services
{
    public class SaveFileInfoService : ISaveFileInfoService
    {
        public const string LocalDataFileName = "localdata.xml";

        private List<SaveFileInfo?> _saveFileInfos;
        public List<SaveFileInfo?> SaveFileInfos => _saveFileInfos;

        private readonly StorageFolder _localFolder;

        public SaveFileInfoService()
        {
            _localFolder = ApplicationData.Current.LocalFolder;
            if (!_localFolder.FileExistsAsync(LocalDataFileName).Result)
            {
                _ = _localFolder.CreateFileAsync(LocalDataFileName).GetResults();
            }
            _saveFileInfos = new();
        }

        public void AddSaveFileInfo(SaveFileInfo saveFileInfo)
        {
            var fileExists = SaveFileInfos.Any(sfi => sfi!.FilePath == saveFileInfo.FilePath) || SaveFileInfos.Contains(saveFileInfo);
            if(fileExists)
                return;

            SaveFileInfos.Add(saveFileInfo);
            if (SaveFileInfos.Count > MainPageViewModel.MAX_SAVE_FILE_COUNT)
                SaveFileInfos.RemoveAt(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SaveFileInfo?>?> LoadSaveFileInfos()
        {
            try
            {
                var file = await _localFolder.GetFileAsync(LocalDataFileName);
                using var stream = await file.OpenStreamForReadAsync();
                if(stream.Length > 0)
                {
                    _saveFileInfos = Core.Utils.ReadAndDeserializeFromXml<List<SaveFileInfo?>>(stream);
                    stream.Close();
                    return _saveFileInfos;
                }
                Log.Warning("{0} is empty...", LocalDataFileName);
                stream.Close();
                return null;
            }
            catch (FileNotFoundException e)
            {
                Log.Error(e, "localdata.xml was not found!");
                throw;
            }
            catch (IOException e)
            {
                if (e.Source != null)
                {
                    Log.Error(e, "IOException while accessing \"{0}\": {1}", LocalDataFileName, e.Source);
                }
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SaveSaveFileInfos()
        {
            try
            {
                var file = await _localFolder.GetFileAsync(LocalDataFileName);
                using var stream = await file.OpenStreamForWriteAsync();
                Core.Utils.SerializeAndWriteToXml(SaveFileInfos, stream);
                stream.Close();
            }
            catch (FileNotFoundException e)
            {
                Log.Error(e, "\"{0}\" was not found!", LocalDataFileName);
                throw;
            }
            catch (IOException e)
            {
                if (e.Source != null)
                {
                    Log.Error(e, "IOException while accessing \"{0}\": {1}", LocalDataFileName, e.Source);
                }
                throw;
            }
        }
    }
}
