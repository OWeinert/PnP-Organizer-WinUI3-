using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.ViewModels;

namespace PnPOrganizer.Views
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            ViewModel = Ioc.Default.GetRequiredService<MainWindowViewModel>();
        }

        public AppTitleBar TitleBar { get => AppTitleBar; }

        public NavigationView AppNavigationViewControl { get => AppNavigationView; }

        public Frame ContentFrameControl { get => ContentFrame; }

        public MainWindowViewModel ViewModel { get; }
    }
}