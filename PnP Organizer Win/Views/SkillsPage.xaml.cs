using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;

namespace PnPOrganizer.Views
{
    [INotifyPropertyChanged]
    public sealed partial class SkillsPage : Page, IViewFor<SkillsPageViewModel>
    {
        public SkillsPageViewModel ViewModel { get; }

        [ObservableProperty]
        private Skill? _selectedSkill;

        [ObservableProperty]
        private bool _selectedSkillSkillable = false;

        public SkillsPage()
        {
            ViewModel = Ioc.Default.GetRequiredService<SkillsPageViewModel>();
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void ItemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedSkill = (Skill)e.ClickedItem;

            SkillsSplitView.IsPaneOpen = true;
        }

        private void LockedHyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            UnlockTeachingTip.IsOpen = true;
        }
    }
}
