using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;

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
            MapScrollViewer.ZoomToFactor(MapScrollViewer.MinZoomFactor);
        }
    }
}
