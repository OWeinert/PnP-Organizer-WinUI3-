using Microsoft.UI.Xaml;
using PnPOrganizer.Core.Character;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace PnPOrganizer.Services.Interfaces
{
    public interface ISaveDataService
    {
        public event EventHandler<CharacterData>? CharacterSaved;
        public event EventHandler<CharacterData>? CharacterLoaded;

        public CharacterData? LoadedCharacter { get; }

        public CharacterData CreateNewCharacter();

        public Task LoadCharacter(StorageFile file);

        public Task SaveCharacter(StorageFile file);

        public Task<bool> ShowSaveCharacterFilePicker(UIElement? currentUIElement = null);

        public Task<bool> ShowOpenCharacterFilePicker(UIElement? currentUIElement = null);
    }
}
