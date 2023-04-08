using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;

namespace PnPOrganizer.Views
{
    public sealed partial class CharacterPage : Page, IViewFor<CharacterPageViewModel>
    {
        public CharacterPageViewModel ViewModel { get; }

        public CharacterPage()
        {
            ViewModel = Ioc.Default.GetRequiredService<CharacterPageViewModel>();
            InitializeComponent();
        }
    }
}
