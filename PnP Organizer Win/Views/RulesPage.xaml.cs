using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Core;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using System;
using System.Diagnostics;
using Windows.UI.WindowManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PnPOrganizer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
            var externalWindow = new Window();
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
