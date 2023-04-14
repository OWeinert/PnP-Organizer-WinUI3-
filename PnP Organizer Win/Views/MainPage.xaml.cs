using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Media.Geometry;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using PnPOrganizer.Services.Data;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PnPOrganizer.Views
{
    [INotifyPropertyChanged]
    public sealed partial class MainPage : Page, IViewFor<MainPageViewModel>
    {
        public MainPageViewModel ViewModel { get; }

        public double BackgroundBaseOpacity => 0.4;

        public MainPage()
        {
            InitializeComponent();
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
            var saveDataService = Ioc.Default.GetRequiredService<ISaveDataService>();
            await saveDataService.ShowOpenCharacterFilePicker();
        }

        private async void CharacterCardOpenButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSave = ViewModel.SelectedSave!.SaveFileInfo;
            if(selectedSave != null)
            {
                try
                {
                    var saveDataService = Ioc.Default.GetRequiredService<ISaveDataService>();
                    var file = await StorageFile.GetFileFromPathAsync(selectedSave.FilePath);
                    if (file != null)
                        await saveDataService.LoadCharacter(file);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Exception thrown while trying to load Character!");
                    throw;
                }
            }   
        }
    }
}