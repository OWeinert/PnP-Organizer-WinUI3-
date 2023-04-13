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
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using Serilog;
using System;
using System.Linq;
using Windows.Foundation;

namespace PnPOrganizer.Views
{
    [INotifyPropertyChanged]
    public sealed partial class MainPage : Page, IViewFor<MainPageViewModel>
    {
        public MainPageViewModel ViewModel { get; }

        public double BackgroundBaseOpacity => 0.4;

        [ObservableProperty]
        private SaveFileInfo? _selectedSave;

        public event EventHandler<SaveFileInfo?>? SelectedSaveChanged;

        public MainPage()
        {
            InitializeComponent();
            ViewModel = Ioc.Default.GetRequiredService<MainPageViewModel>();
        }

        partial void OnSelectedSaveChanged(SaveFileInfo? value) => SelectedSaveChanged?.Invoke(this, value);

        private async void LastCharacterCarousel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.AddedItems.First();
            if (selectedItem is SaveFileInfo selectedSave)
            {
                SelectedSave = selectedSave;
                await Ioc.Default.GetRequiredService<MainWindow>().DispatcherQueue.EnqueueAsync(async () =>
                {
                    await FadeOutAnimation.StartAsync();
                    CharacterImageBackground.Source = SelectedSave.CharacterImage;
                    await FadeInAnimation.StartAsync();
                });
            }
        }

        private void LastCharactersCarousel_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedSave = ViewModel.SaveFileInfos![LastCharactersCarousel.SelectedIndex];
            CharacterImageBackground.Source = SelectedSave.CharacterImage;
        }

        private void ScrollDownButton_Click(object sender, RoutedEventArgs e)
        {
            ContentScrollViewer.ChangeView(null, ContentScrollViewer.ScrollableHeight, null);
        }
    }
}