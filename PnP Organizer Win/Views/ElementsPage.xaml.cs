using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PnPOrganizer.Core;
using PnPOrganizer.ViewModels.Interfaces;
using PnPOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
