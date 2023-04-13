using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace PnPOrganizer.Services.Data
{
    public partial class SaveFileInfo : ObservableObject
    {
        [ObservableProperty]
        private Uri? _filePath;

        [ObservableProperty]
        private string _characterFirstName = string.Empty;
        [ObservableProperty]
        private string _characterLastName = string.Empty;

        [ObservableProperty]
        private BitmapImage? _characterImage;

        public SaveFileInfo(Uri? filePath, string characterFirstName, string characterLastName, BitmapImage? characterImage)
        {
            FilePath = filePath;
            CharacterFirstName = characterFirstName;
            CharacterLastName = characterLastName;
            CharacterImage = characterImage;
        }

        public static SaveFileInfo Dummy
        {
            get
            {
                var defaultCharacterImage = new BitmapImage();
                var uriCreationOptions = new UriCreationOptions();
                if (Uri.TryCreate("ms-appx:///Assets/Artom.jpg", in uriCreationOptions, out var result))
                {
                    defaultCharacterImage.UriSource = result;
                }
                var dummy = new SaveFileInfo(null, "Parica", "Artom", defaultCharacterImage);
                return dummy;
            }
        }
    }
}
