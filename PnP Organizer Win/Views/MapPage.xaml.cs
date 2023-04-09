using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using PnPOrganizer.Views.Maps;

namespace PnPOrganizer.Views
{
    public sealed partial class MapPage : Page, IViewFor<MapPageViewModel>
    {
        public MapPageViewModel ViewModel { get; }

        public MapPage()
        {
            ViewModel = Ioc.Default.GetService<MapPageViewModel>()!;
            InitializeComponent();
            ViewModel.SetupMapService(ContentFrame);
        }
    }
}
