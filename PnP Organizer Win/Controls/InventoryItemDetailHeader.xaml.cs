using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PnPOrganizer.Helpers;
using PnPOrganizer.Models;
using System;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PnPOrganizer.Controls
{
    public sealed partial class InventoryItemDetailHeader : UserControl
    {
        public static readonly DependencyProperty ItemViewModelProperty = DependencyProperty.Register(
            nameof(ItemViewModel),
            typeof(InventoryItemViewModel),
            typeof(InventoryItemHeader),
            new PropertyMetadata(null)
        );

        public InventoryItemViewModel ItemViewModel
        {
            get => (InventoryItemViewModel)GetValue(ItemViewModelProperty);
            set => SetValue(ItemViewModelProperty, value);
        }

        public event RoutedEventHandler? CancelButtonClick;
        public event RoutedEventHandler? AcceptButtonClick;

        public InventoryItemDetailHeader()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => CancelButtonClick?.Invoke(sender, e);

        private void AcceptButton_Click(object sender, RoutedEventArgs e) => AcceptButtonClick?.Invoke(sender, e);

        private async void ItemImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker();

            var hWnd = WindowHelper.GetCurrentProcMainWindowHandle();
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".gif");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var itemImage = new BitmapImage();

                using var stream = await file.OpenReadAsync();
                stream.Seek(0);
                await itemImage.SetSourceAsync(stream);

                if (ItemViewModel != null)
                    ItemViewModel.ItemImage = itemImage;
            }
        }
    }
}
