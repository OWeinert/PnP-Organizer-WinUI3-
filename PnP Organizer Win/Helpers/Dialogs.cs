using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PnPOrganizer.Helpers
{
    public static class Dialogs
    {
        private static ContentDialog? _loadCharacterConfirmDialog;
        public static ContentDialog GetLoadCharacterConfirmDialog(XamlRoot root, ElementTheme theme)
        {
            if (_loadCharacterConfirmDialog == null)
                {
                    _loadCharacterConfirmDialog = new ContentDialog
                    {
                        XamlRoot = root,
                        Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                        Title = "You have unsaved Changes!",
                        PrimaryButtonText = "Save",
                        SecondaryButtonText = "Don't Save",
                        CloseButtonText = "Cancel",
                        DefaultButton = ContentDialogButton.Primary,
                        Content = "Do you want to save before loading another Character?",
                        RequestedTheme = theme
                    };
            }
            return _loadCharacterConfirmDialog;
        }

        private static ContentDialog? _newCharacterConfirmDialog;
        public static ContentDialog GetNewCharacterConfirmDialog(XamlRoot root, ElementTheme theme)
        {
            if (_newCharacterConfirmDialog == null)
            {
                _newCharacterConfirmDialog = new ContentDialog
                {
                    XamlRoot = root,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "You have unsaved Changes!",
                    PrimaryButtonText = "Save",
                    SecondaryButtonText = "Don't Save",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "Do you want to save before creating a new Character?",
                    RequestedTheme = theme
                };
            }
            return _newCharacterConfirmDialog;
        }
    }
}
