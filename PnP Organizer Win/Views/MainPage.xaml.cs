using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PnPOrganizer.Helpers;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PnPOrganizer.Views
{
    public sealed partial class MainPage : Page, IViewFor<MainPageViewModel>
    {
        public MainPageViewModel ViewModel { get; }

        public double BackgroundBaseOpacity => 0.4;

        private ISaveDataService _saveDataService;

        public MainPage()
        {
            InitializeComponent();
            _saveDataService = Ioc.Default.GetRequiredService<ISaveDataService>();
            ViewModel = Ioc.Default.GetRequiredService<MainPageViewModel>();
            ViewModel.SelectedSaveChanged += (sender, e) => Bindings.Update();
        }

        private async void LastCharacterCarousel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.AddedItems.First();
            if (selectedItem is SaveFileInfoViewModel selectedSave)
            {
                ViewModel.SelectedSave = selectedSave;
                await Ioc.Default.GetRequiredService<MainWindow>().DispatcherQueue.EnqueueAsync(async () =>
                {
                    await TrySetBackgroundImage();
                });
            }
        }

        private async void LastCharactersCarousel_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedSave = ViewModel.SaveFileInfos![0];
            LastCharactersCarousel.SelectedItem = ViewModel.SelectedSave;
            foreach (var saveFileInfoVM in ViewModel.SaveFileInfos)
            {
                saveFileInfoVM.PropertyChanged += async (sender, e) =>
                {
                    if (e.PropertyName is nameof(SaveFileInfoViewModel.SaveFileInfo))
                    {
                        LastCharactersCarousel.SelectedItem = (SaveFileInfoViewModel)sender!;
                        await TrySetBackgroundImage();
                    }
                };
            }
            await TrySetBackgroundImage();
        }

        private void ScrollDownButton_Click(object sender, RoutedEventArgs e)
        {
            ContentScrollViewer.ChangeView(null, ContentScrollViewer.ScrollableHeight, null);
        }

        private async Task<bool> TrySetBackgroundImage()
        {
            return await Ioc.Default.GetRequiredService<MainWindow>().DispatcherQueue.EnqueueAsync(async () =>
            {
                var result = false;
                await FadeOutAnimation.StartAsync();
                if (ViewModel.SelectedSave?.SaveFileInfo != null)
                {
                    CharacterImageBackground.Source = ViewModel.SelectedSave.CharacterImage;
                    result = true;
                }
                else
                    CharacterImageBackground.Source = null;
                await FadeInAnimation.StartAsync();
                return result;
            });
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_saveDataService.IsSaved)
            {
                var result = await Dialogs.GetLoadCharacterConfirmDialog(XamlRoot, ActualTheme).ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await _saveDataService.ShowSaveCharacterFilePicker();
                    await ShowFilePicker();
                }
                else if (result == ContentDialogResult.Secondary)
                    await ShowFilePicker();
            }
            else
                await ShowFilePicker();

            async Task ShowFilePicker()
            {
                await _saveDataService.ShowOpenCharacterFilePicker();
            }
        }

        private async void CharacterCardOpenButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSave = ViewModel.SelectedSave!.SaveFileInfo;
            if(selectedSave != null)
            {
                if (!_saveDataService.IsSaved)
                {
                    var result = await Dialogs.GetLoadCharacterConfirmDialog(XamlRoot, ActualTheme).ShowAsync();
                    if(result == ContentDialogResult.Primary)
                    {
                        await _saveDataService.ShowSaveCharacterFilePicker();
                        await LoadCharacter();
                    }
                    else if(result == ContentDialogResult.Secondary)
                        await LoadCharacter();
                }
                else
                    await LoadCharacter();
            }   

            async Task LoadCharacter()
            {
                try
                {
                    var file = await StorageFile.GetFileFromPathAsync(selectedSave!.FilePath);
                    if (file != null)
                        await _saveDataService.LoadCharacter(file);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Exception thrown while trying to load Character!");
                    throw;
                }
            }
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_saveDataService.IsSaved)
            {
                var result = await Dialogs.GetNewCharacterConfirmDialog(XamlRoot, ActualTheme).ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await _saveDataService.ShowSaveCharacterFilePicker();
                    CreateNewCharacter();
                }
                else if (result == ContentDialogResult.Secondary)
                    CreateNewCharacter();
            }
            else
                CreateNewCharacter();

            void CreateNewCharacter()
            {
                _saveDataService.CreateNewCharacter();
                var navigationService = Ioc.Default.GetRequiredService<INavigationViewService>();
                navigationService.NavigateTo(typeof(CharacterPage), new CommonNavigationTransitionInfo());
            }
        }
    }
}