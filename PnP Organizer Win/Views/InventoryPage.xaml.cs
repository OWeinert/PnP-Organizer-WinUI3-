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
using PnPOrganizer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.UI;
using PnPOrganizer.Views.Interfaces;

namespace PnPOrganizer.Views
{
    [INotifyPropertyChanged]
    public sealed partial class InventoryPage : Page, IViewFor<InventoryPageViewModel>
    {
        public InventoryPageViewModel ViewModel { get; }

        [ObservableProperty]
        private InventoryItemModel? _storedItem;


        private const byte DETAILCARD_ALPHA = 224;

        public InventoryPage()
        {
            ViewModel = Ioc.Default.GetRequiredService<InventoryPageViewModel>();
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            SharedShadow.Receivers.Add(ItemsScrollViewer);
            SharedShadow.Receivers.Add(ItemsGridView);
        }

        #region Item Detail View
        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", ItemDetails);
            SmokeGrid.Children.Remove(ItemDetails);

            animation.Completed += Animation_Completed;

            ItemsGridView.ScrollIntoView(StoredItem, ScrollIntoViewAlignment.Default);
            ItemsGridView.UpdateLayout();

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                animation.Configuration = new DirectConnectedAnimationConfiguration();
            }
            await ItemsGridView.TryStartConnectedAnimationAsync(animation, StoredItem, "Item");
            ViewModel.ItemsView.Refresh();
        }

        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            SmokeGrid.Visibility = Visibility.Collapsed;
            SmokeGrid.Children.Add(ItemDetails);
        }

        private void DetailContentDescr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (StoredItem is not null)
                StoredItem.Description = DetailContentDescr.Text;
        }
        #endregion

        #region ItemsGridView
        private void ItemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ElementSoundPlayer.Play(ElementSoundKind.Focus);
            if (ItemsGridView.ContainerFromItem(e.ClickedItem) is GridViewItem container)
            {
                if(container.Content is not null)
                {
                    StoredItem = (container.Content as InventoryItemModel)!;
                }
                var animation = ItemsGridView.PrepareConnectedAnimation("ForwardConnectedAnimation", StoredItem, "Item");
                
                if(StoredItem is not null)
                {
                    ItemDetails.DataContext = StoredItem;

                    var translucentBrush = new SolidColorBrush(Color.FromArgb(DETAILCARD_ALPHA, StoredItem.Brush!.Color.R, StoredItem.Brush.Color.G, StoredItem.Brush.Color.B));

                    DetailHeaderGrid.Background = translucentBrush;
                    DetailHeaderName.Text = StoredItem.Name;
                    DetailContentGrid.Background = translucentBrush;
                    DetailContentDescr.Text = StoredItem.Description;
                }

                animation.TryStart(ItemDetails);
                SmokeGrid.Visibility = Visibility.Visible;

            }
        }

        // A bit hacky and doesn't look that great, but it's better than nothing
        private void ItemsGridView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args) => ViewModel.ItemsView.Refresh();
        #endregion

        #region Edit Item Menu
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
        #endregion

        #region Item Search
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
        #endregion

        #region Filtering
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
        #endregion

        #region Item Command Flyout
        private void CommandFlyoutColorButton_Click(object sender, RoutedEventArgs e) 
        {
            SetStoredItem((Button)sender);
            ItemColorPicker.Color = StoredItem!.Brush is not null ? StoredItem.Brush.Color : Color.FromArgb(255, 255, 255, 255);
        }

        private void CommandFlyoutClearButton_Click(object sender, RoutedEventArgs e) => SetStoredItem((Button)sender);

        private void CommandFlyoutDeleteButton_Click(object sender, RoutedEventArgs e) => SetStoredItem((Button)sender);

        private void SetStoredItem(Button sender) => _storedItem = (InventoryItemModel)sender.DataContext;

        private void ConfirmColor_Click(object sender, RoutedEventArgs e)
        {
            if(StoredItem is not null)
                StoredItem.Brush = new SolidColorBrush(ItemColorPicker.Color);

            ViewModel.ItemsView.Refresh();
            PickColorButton.Flyout.Hide();
        }

        private void CancelColor_Click(object sender, RoutedEventArgs e) => PickColorButton.Flyout.Hide();
        #endregion
    }
}
