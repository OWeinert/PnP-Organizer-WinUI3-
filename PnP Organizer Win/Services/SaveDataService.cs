using Microsoft.UI.Xaml;
using PnPOrganizer.Core;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace PnPOrganizer.Services
{
    public class SaveDataService : ISaveDataService
    {
        private CharacterData? _loadedCharacter;
        public CharacterData? LoadedCharacter => _loadedCharacter;

        public CharacterData CreateNewCharacter()
        {
            return new CharacterData();
        }

        public IAsyncAction LoadCharacter(StorageFile file)
        {
            return Task.Run(async () =>
            {
                using var stream = await file.OpenStreamForReadAsync();
                if (stream is FileStream fs)
                {
                    _loadedCharacter = Utils.ReadAndDeserializeFromXml<CharacterData>(fs);
                }
                else
                    throw new IOException("Invalid Stream when trying to load Character!");
            }).AsAsyncAction();
        }

        public IAsyncAction SaveCharacter(StorageFile file)
        {
            return Task.Run(async () =>
            {
                using var stream = await file.OpenStreamForWriteAsync();
                if (stream is FileStream fs)
                {
                    Utils.SerializeAndWriteToXml(LoadedCharacter, fs);
                }
                else
                    throw new IOException("Invalid Stream when trying to save Character!");
            }).AsAsyncAction();
        }

        public async Task<bool> ShowSaveCharacterFilePicker(UIElement? currentUIElement = null)
        {
            var savePicker = new FileSavePicker();

            var window = currentUIElement != null ? WindowHelper.GetWindowForElement(currentUIElement) : Window.Current;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Chararacter File", new List<string>() { ".cha" });
            var enteredFileName = LoadedCharacter?.FileName;
            savePicker.SuggestedFileName = enteredFileName ?? string.Empty;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await SaveCharacter(file);

                // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                var status = await CachedFileManager.CompleteUpdatesAsync(file);

                return true;
            }
            return false;
        }

        public async Task<bool> ShowOpenCharacterFilePicker(UIElement? currentUIElement = null)
        {
            var openPicker = new FileOpenPicker();

            var window = currentUIElement != null ? WindowHelper.GetWindowForElement(currentUIElement) : Window.Current;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".cha");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                await LoadCharacter(file);
                return true;
            }
            return false;
        }
    }
}
