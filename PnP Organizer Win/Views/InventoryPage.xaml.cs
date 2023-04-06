using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PnPOrganizer.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation.Metadata;
using System.Diagnostics;
using PnPOrganizer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PnPOrganizer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [INotifyPropertyChanged]
    public sealed partial class InventoryPage : Page
    {
        public InventoryPageViewModel ViewModel { get; }

        [ObservableProperty]
        private InventoryItemModel? _storedItem;

        private const byte DETAILCARD_ALPHA = 224;

        public InventoryPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel = Ioc.Default.GetRequiredService<InventoryPageViewModel>();
            SharedShadow.Receivers.Add(ItemsScrollViewer);
            SharedShadow.Receivers.Add(ItemsGridView);
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", destinationElement);
            SmokeGrid.Children.Remove(destinationElement);

            animation.Completed += Animation_Completed;

            ItemsGridView.ScrollIntoView(StoredItem, ScrollIntoViewAlignment.Default);
            ItemsGridView.UpdateLayout();

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                animation.Configuration = new DirectConnectedAnimationConfiguration();
            }
            await ItemsGridView.TryStartConnectedAnimationAsync(animation, StoredItem, "connectedElement");
        }

        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            SmokeGrid.Visibility = Visibility.Collapsed;
            SmokeGrid.Children.Add(destinationElement);
        }

        private void ItemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ItemsGridView.ContainerFromItem(e.ClickedItem) is GridViewItem container)
            {
                if(container.Content is not null)
                {
                    StoredItem = (container.Content as InventoryItemModel)!;
                }
                var animation = ItemsGridView.PrepareConnectedAnimation("ForwardConnectedAnimation", StoredItem, "connectedElement");
                
                if(StoredItem is not null)
                {
                    destinationElement.DataContext = StoredItem;

                    var translucentBrush = new SolidColorBrush(Color.FromArgb(DETAILCARD_ALPHA, StoredItem.Brush!.Color.R, StoredItem.Brush.Color.G, StoredItem.Brush.Color.B));

                    DetailHeaderGrid.Background = translucentBrush;
                    DetailHeaderName.Text = StoredItem.Name;
                    DetailContentGrid.Background = translucentBrush;
                    DetailContentDescr.Text = StoredItem.Description;
                }

                animation.TryStart(destinationElement);
                SmokeGrid.Visibility = Visibility.Visible;
            }
        }

        private void ShowMenu(bool isTransient, UIElement target)
        {
            var myOption = new FlyoutShowOptions
            {
                ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard
            };
            EditItemFlyout.ShowAt(target, myOption);
        }

        private void EditItemButton_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
        {
            ShowMenu(false, sender);
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement target)
                ShowMenu(true, target);
        }

        private void OnElementClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CLICKED");
        }

        private void SearchItemBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<InventoryItemModel>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var item in ViewModel.ItemModels)
                {
                    var found = splitText.All((key) =>
                    {
                        return item.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(item);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    //TODO Give information that no item was found
                }
                sender.ItemsSource = suitableItems.ConvertAll(invItem => invItem.Name);
            }
            FilterItems();
        }

        private void FilterItems()
        {
            if (ViewModel != null)
            {
                if (string.IsNullOrWhiteSpace(SearchItemBox.Text))
                {
                    ResetFilter();
                }
                else
                {
                    ResetFilter();
                    ViewModel.ItemsView.Filter = x => ((InventoryItemModel)x).Name.ToLower().Contains(SearchItemBox.Text.ToLower());
                }
            }
        }

        private void ResetFilter() => ViewModel.ItemsView.Filter = _ => true;

        // A bit hacky and doesn't look that great, but it's better than nothing
        private void ItemsGridView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args) => ViewModel.ItemsView.Refresh();
    }
}
