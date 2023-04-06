using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.ViewModels;

namespace PnPOrganizer.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel = Ioc.Default.GetRequiredService<MainPageViewModel>();
        }

        public MainPageViewModel ViewModel { get; }
    }
}