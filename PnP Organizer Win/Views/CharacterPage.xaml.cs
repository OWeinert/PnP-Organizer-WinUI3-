using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using PnPOrganizer.Core.Attributes;
using PnPOrganizer.Helpers;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Globalization.NumberFormatting;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace PnPOrganizer.Views
{
    public sealed partial class CharacterPage : Page, IViewFor<CharacterPageViewModel>
    {
        public CharacterPageViewModel ViewModel { get; }

        private AdvancedCollectionView? _attributeChecksView;
        public AdvancedCollectionView AttributeChecksView 
        {
            get
            {
                _attributeChecksView ??= new AdvancedCollectionView(ViewModel.AttributeChecks!);
                return _attributeChecksView;
            }
        }

        public CharacterPage()
        {
            ViewModel = Ioc.Default.GetRequiredService<CharacterPageViewModel>();
            InitializeComponent();
            SetNumberBoxNumberFormatter(HeightNumberBox);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel?.RefreshAttributeCheckBoni();
        }

        private void SetNumberBoxNumberFormatter(NumberBox numberBox)
        {
            var rounder = new IncrementNumberRounder
            {
                Increment = 0.01,
                RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp
            };

            var formatter = new DecimalFormatter
            {
                IntegerDigits = 1,
                FractionDigits = 2,
                NumberRounder = rounder
            };
            numberBox.NumberFormatter = formatter;
        }

        private void CharacterImage_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            var resourceLoader = ResourceLoader.GetForViewIndependentUse();
            e.DragUIOverride.Caption = resourceLoader.GetString("CharacterPage_DragDropCaption");
        }

        private async void CharacterImage_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    var storageFile = items[0] as StorageFile;
                    var bitmapImage = new BitmapImage();
                    if(storageFile != null)
                    {
                        await bitmapImage.SetSourceAsync(await storageFile.OpenAsync(FileAccessMode.Read));
                        CharacterImage.Source = bitmapImage;
                        ViewModel.CharacterData!.CharacterImage = await Core.Utils.BitmapToBytesAsync(bitmapImage);
                    }
                }
            }
        }

        private void RemoveImageButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            CharacterImage.Source = null;
            ViewModel.CharacterData!.CharacterImage = Array.Empty<byte>();
        }

        private async void SelectImageButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
                CharacterImage.Source = bitmapImage;
                ViewModel.CharacterData!.CharacterImage = await Core.Utils.BitmapToBytesAsync(bitmapImage);
            }
        }

        private void AttributeCheckChoiceListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(e.ClickedItem is AttributeCheck attributeCheck)
            {
                ViewModel.CreateProfession(attributeCheck);
            }
        }

        private void RemoveProfessionButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var button = (Button)sender;
            if(button.DataContext is Profession profession)
            {
                ViewModel.RemoveProfession(profession);
            }
        }
    }
}
