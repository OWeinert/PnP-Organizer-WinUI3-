using Microsoft.UI.Xaml;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Services.Data;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace PnPOrganizer.Services.Interfaces
{
    public interface ISaveDataService
    {
        public event EventHandler<CharacterData>? CharacterSaved;
        public event EventHandler<CharacterData>? CharacterLoaded;

        public event EventHandler<SaveFileInfo>? CharacterSaveInfoCreated;

        public CharacterData? LoadedCharacter { get; }
        public SaveFileInfo? LoadedCharacterSaveInfo { get; }

        public bool IsSaved { get; }

        public CharacterData CreateNewCharacter();

        public Task LoadCharacter(StorageFile file);

        public Task LoadCharacter(string path);

        public Task SaveCharacter(StorageFile file);

        public Task SaveLoadedCharacter();

        public Task<bool> ShowSaveCharacterFilePicker(UIElement? currentUIElement = null);

        public Task<bool> ShowOpenCharacterFilePicker(UIElement? currentUIElement = null);

        public void CreateCharacterSaveFileInfo(StorageFile file);

        public void MarkUnsaved();

        public void MarkSaved();
    }
}
