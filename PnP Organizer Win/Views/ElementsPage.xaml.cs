using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.Helpers;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;

namespace PnPOrganizer.Views
{
    // HACK use base class and only change the url for Rules / Elements
    public sealed partial class ElementsPage : Page, IViewFor<ElementsPageViewModel>
    {
        public ElementsPageViewModel ViewModel { get; }

        public ElementsPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<ElementsPageViewModel>()!;
            ButtonShadow.Receivers.Add(MyWebView2);
        }

        private void OpenWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var externalWindow = WindowHelper.CreateWindow();
            var rootPage = new ExternalElementsPage();
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
