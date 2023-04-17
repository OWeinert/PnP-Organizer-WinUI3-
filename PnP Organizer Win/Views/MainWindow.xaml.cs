using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using System.Threading.Tasks;

namespace PnPOrganizer.Views
{
    public sealed partial class MainWindow : Window
    {
        private ISaveFileInfoService _saveFileInfoService;

        private int _notificationDuration = 2000;

        public XamlRoot XamlRoot { get; }
        public ElementTheme ActualTheme { get; }

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
            Ioc.Default.GetRequiredService<ISaveDataService>().CharacterLoaded += MainWindow_CharacterLoaded;
            XamlRoot = ContentGrid.XamlRoot;
            ActualTheme = ContentGrid.ActualTheme;
        }

        private void MainWindow_CharacterLoaded(object? sender, Core.Character.CharacterData e)
        {
            CharacterLoadedTT.Show(_notificationDuration);
        }

        private void MainWindow_CharacterSaved(object? sender, Core.Character.CharacterData e)
        {
            CharacterSavedTT.Show(_notificationDuration);
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