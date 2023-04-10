using Microsoft.UI.Xaml;
using PnPOrganizer.Core.Character;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace PnPOrganizer.Services.Interfaces
{
    public interface ISaveDataService
    {
        public CharacterData? LoadedCharacter { get; }

        public CharacterData CreateNewCharacter();

        public IAsyncAction LoadCharacter(StorageFile file);

        public IAsyncAction SaveCharacter(StorageFile file);

        public Task<bool> ShowSaveCharacterFilePicker(UIElement? currentUIElement = null);

        public Task<bool> ShowOpenCharacterFilePicker(UIElement? currentUIElement = null);
    }
}
