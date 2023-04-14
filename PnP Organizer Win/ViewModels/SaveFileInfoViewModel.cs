using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Converters;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using PnPOrganizer.Core;
using PnPOrganizer.Services.Data;
using PnPOrganizer.ViewModels.Interfaces;

namespace PnPOrganizer.ViewModels
{
    public partial class SaveFileInfoViewModel : ObservableObject, ISelfBindable<SaveFileInfoViewModel>
    {

        [ObservableProperty]
        private SaveFileInfoViewModel? _observableSelf;

        public SaveFileInfoViewModel BindableInstance => this;

        [ObservableProperty]
        private SaveFileInfo? _saveFileInfo;

        [ObservableProperty]
        private bool _hasSaveFileInfo;

        [ObservableProperty]
        private BitmapImage? _characterImage;

        [ObservableProperty]
        private MainPageViewModel? _parent;

        public SaveFileInfoViewModel(MainPageViewModel parent)
        {
            Parent = parent;
        }

        public SaveFileInfoViewModel(MainPageViewModel parent, SaveFileInfo? saveFileInfo) : this(parent)
        {
            ObservableSelf = BindableInstance;
            SaveFileInfo = saveFileInfo;
        }

        partial void OnSaveFileInfoChanged(SaveFileInfo? value)
        {
            HasSaveFileInfo = value != null;
            if (value != null)
            {
                CharacterImage = Utils.BitmapFromBytes(value.CharacterImage!);
            }
            ObservableSelf = BindableInstance;
        }

        public static Visibility IsValidAndVisible(SaveFileInfoViewModel thisSaveFileInfoVM, SaveFileInfoViewModel selectedSaveFileInfoVM)
        {
            if (thisSaveFileInfoVM == null || !thisSaveFileInfoVM.HasSaveFileInfo)
                return Visibility.Collapsed;

            if (selectedSaveFileInfoVM != null && selectedSaveFileInfoVM == thisSaveFileInfoVM)
                return Visibility.Visible;
            

            return Visibility.Collapsed;
        }
    }
}
