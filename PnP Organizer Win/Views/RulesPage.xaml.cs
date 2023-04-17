using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.Helpers;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;

namespace PnPOrganizer.Views
{
    public sealed partial class RulesPage : Page, IViewFor<RulesPageViewModel>
    {
        public RulesPageViewModel ViewModel { get; }

        public RulesPage()
        {
            InitializeComponent();
            ViewModel = Ioc.Default.GetService<RulesPageViewModel>()!;
            ButtonShadow.Receivers.Add(MyWebView2);
        }

        private void OpenWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var externalWindow = WindowHelper.CreateWindow();
            var rootPage = new ExternalRulesPage();
            externalWindow.Content = rootPage;
            externalWindow.Closed += (sender, e) =>
            {
                if (sender is Window window)
                {
                    if (window.Content is ExternalRulesPage rulesPage)
                        rulesPage.RulesWebView.Close();
                }
            };
            externalWindow.Activate();
        }
    }
}
