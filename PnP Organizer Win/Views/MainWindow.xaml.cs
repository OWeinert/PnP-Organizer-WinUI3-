using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PnPOrganizer.Helpers;
using PnPOrganizer.Interfaces;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using System;
using System.Threading.Tasks;

namespace PnPOrganizer.Views
{
    public sealed partial class MainWindow : Window
    {
        private ISaveFileInfoService _saveFileInfoService;

        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            ViewModel = Ioc.Default.GetRequiredService<MainWindowViewModel>();
            _saveFileInfoService = Ioc.Default.GetRequiredService<ISaveFileInfoService>();
            AppWindow.SetIcon("Assets/applicationIcon.ico");
            AppWindow.Closing += AppWindow_Closing;
            Ioc.Default.GetRequiredService<ISaveDataService>().CharacterSaved += MainWindow_CharacterSaved;
            Ioc.Default.GetRequiredService<ISaveDataService>().CharacterLoaded += MainWindow_CharacterLoaded; ;
        }

        private void MainWindow_CharacterLoaded(object? sender, Core.Character.CharacterData e)
        {
            CharacterLoadedTeachingTip.IsOpen = true;
        }

        private void MainWindow_CharacterSaved(object? sender, Core.Character.CharacterData e)
        {
            CharacterSavedTeachingTip.IsOpen = true;
        }

        private async void AppWindow_Closing(Microsoft.UI.Windowing.AppWindow sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
        {
            args.Cancel = true;
            await Task.Run(() => _saveFileInfoService.SaveSaveFileInfos());
            this.Close();
        }

        public NavigationView AppNavigationViewControl { get => AppNavigationView; }

        public Frame ContentFrameControl { get => ContentFrame; }

        public MainWindowViewModel ViewModel { get; }

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }
    }
}