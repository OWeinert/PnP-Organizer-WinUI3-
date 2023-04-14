using CommunityToolkit.Mvvm.ComponentModel;

namespace PnPOrganizer.Services.Data
{
    public partial class SaveFileInfo : ObservableObject
    {
        [ObservableProperty]
        private string _filePath = string.Empty;

        [ObservableProperty]
        private string _characterFirstName = string.Empty;
        [ObservableProperty]
        private string _characterLastName = string.Empty;

        [ObservableProperty]
        private byte[]? _characterImage;

        public SaveFileInfo(string filePath, string characterFirstName, string characterLastName, byte[]? characterImage)
        {
            FilePath = filePath;
            CharacterFirstName = characterFirstName;
            CharacterLastName = characterLastName;
            CharacterImage = characterImage;
        }

        public SaveFileInfo() { }

        public static SaveFileInfo Empty => new("_empty", string.Empty, string.Empty, null);
    }
}
