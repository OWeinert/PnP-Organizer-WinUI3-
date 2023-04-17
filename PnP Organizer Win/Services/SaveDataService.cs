using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using PnPOrganizer.Core;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.Services.Data;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace PnPOrganizer.Services
{
    public class SaveDataService : ISaveDataService
    {
        public event EventHandler<CharacterData>? CharacterSaved;
        public event EventHandler<CharacterData>? CharacterLoaded;
        public event EventHandler<CharacterData>? CharacterCreated;

        public event EventHandler<SaveFileInfo>? CharacterSaveInfoCreated;

        private CharacterData? _loadedCharacter;
        public CharacterData? LoadedCharacter => _loadedCharacter;

        private SaveFileInfo? _loadedCharacterSaveInfo;
        public SaveFileInfo? LoadedCharacterSaveInfo => _loadedCharacterSaveInfo;

        private bool _isSaved;
        public bool IsSaved => _isSaved;

        private readonly ISkillsService _skillsService;
        private readonly IInventoryService _inventoryService;
        private readonly IPearlService _pearlService;
        private readonly IAttributeService _attributeService;
        private readonly IAttributeCheckService _attributeCheckService;

        public SaveDataService(ISkillsService skillsService, IInventoryService inventoryService, IPearlService pearlService,
            IAttributeService attributeService, IAttributeCheckService attributeCheckService)
        {
            _skillsService = skillsService;
            _inventoryService = inventoryService;
            _pearlService = pearlService;
            _attributeService = attributeService;
            _attributeCheckService = attributeCheckService;
        }

        public CharacterData CreateNewCharacter()
        {
            _loadedCharacter = new CharacterData();
            _loadedCharacterSaveInfo = null;

            _inventoryService.ResetData();
            _skillsService.ResetData();
            _pearlService.ResetData();
            _attributeService.ResetData();
            _attributeCheckService.ResetData();
            
            MarkSaved();
            CharacterCreated?.Invoke(this, _loadedCharacter);
            return LoadedCharacter!;
        }

        public async Task LoadCharacter(StorageFile file)
        {
            var window = Ioc.Default.GetRequiredService<MainWindow>();
            await window.DispatcherQueue.EnqueueAsync(async () =>
            {
                using var stream = await file.OpenStreamForReadAsync();
                try
                {
                    _loadedCharacter = Utils.ReadAndDeserializeFromXml<CharacterData>(stream);

                    _inventoryService.LoadFromCharacter(_loadedCharacter);
                    _skillsService.LoadFromCharacter(_loadedCharacter);
                    _pearlService.LoadFromCharacter(_loadedCharacter);
                    _attributeService.LoadFromCharacter(_loadedCharacter);
                    _attributeCheckService.LoadFromCharacter(_loadedCharacter);

                    MarkSaved();
                    CharacterLoaded?.Invoke(this, _loadedCharacter);
                    CreateCharacterSaveFileInfo(file);
                }
                catch (IOException e)
                {
                    Log.Error(e, "Error while loading Character Save Data!");
                }
            }, DispatcherQueuePriority.High);
        }

        public async Task LoadCharacter(string path)
        {
            if(Utils.TryGetFile(path, out var file))
            {
                await LoadCharacter(file!);
            }
        }

        public async Task SaveCharacter(StorageFile file)
        {
            var window = Ioc.Default.GetRequiredService<MainWindow>();
            await window.DispatcherQueue.EnqueueAsync(async() =>
            {
                using var stream = await file.OpenStreamForWriteAsync();
                stream.SetLength(0);
                try
                {
                    try
                    {
                        _inventoryService.SaveToCharacter(ref _loadedCharacter!);
                        _skillsService.SaveToCharacter(ref _loadedCharacter);
                        _pearlService.SaveToCharacter(ref _loadedCharacter);
                        _attributeService.SaveToCharacter(ref _loadedCharacter);
                        _attributeCheckService.SaveToCharacter(ref _loadedCharacter);

                        Utils.SerializeAndWriteToXml(LoadedCharacter, stream);
                        MarkSaved();
                        CreateCharacterSaveFileInfo(file);
                        CharacterSaved?.Invoke(this, LoadedCharacter!);
                    }
                    catch (ArgumentNullException e) when (_loadedCharacter == null)
                    {
                        Log.Error(e, "Loaded Character is NULL!");
                    }
                }
                catch (IOException e)
                {
                    Log.Error(e, "Error loading Character Save Data!");
                }
            }, DispatcherQueuePriority.High);
        }

        public async Task SaveLoadedCharacter()
        {
            if(LoadedCharacter != null && LoadedCharacterSaveInfo != null)
            {
                var file = await StorageFile.GetFileFromPathAsync(LoadedCharacterSaveInfo.FilePath);
                await SaveCharacter(file);
            }
        }

        public async Task<bool> ShowSaveCharacterFilePicker(UIElement? currentUIElement = null)
        {
            var savePicker = new FileSavePicker();

            var hWnd = WindowHelper.GetCurrentProcMainWindowHandle();
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
                _ = await CachedFileManager.CompleteUpdatesAsync(file);

                return true;
            }
            return false;
        }

        public async Task<bool> ShowOpenCharacterFilePicker(UIElement? currentUIElement = null)
        {
            var openPicker = new FileOpenPicker();

            var hWnd = WindowHelper.GetCurrentProcMainWindowHandle();
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

        public void CreateCharacterSaveFileInfo(StorageFile file)
        {
            if (LoadedCharacter != null)
            {
                _loadedCharacterSaveInfo = new SaveFileInfo(file.Path, "", "", LoadedCharacter!.CharacterImage);
                CharacterSaveInfoCreated?.Invoke(this, _loadedCharacterSaveInfo);
            }
        }

        public void MarkUnsaved() => _isSaved = false;

        public void MarkSaved() => _isSaved = true;
    }
}
