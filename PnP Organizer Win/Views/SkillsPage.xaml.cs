using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Resources;
using System.Text;
using System.Linq;

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

        [ObservableProperty]
        private string _currentSkillUnlockTitle = string.Empty;

        [ObservableProperty]
        private string _currentSkillUnlockContent = string.Empty;

        public SkillsPage()
        {
            ViewModel = Ioc.Default.GetRequiredService<SkillsPageViewModel>();
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        partial void OnSelectedSkillChanged(Skill? value)
        {
            var resourceLoader = ResourceLoader.GetForViewIndependentUse();
            CurrentSkillUnlockTitle = $"{resourceLoader.GetString("Skills_HowToUnlock_Title")}";
            var skillsService = Ioc.Default.GetService<ISkillsService>()!;
            var sB = new StringBuilder();
            if (SelectedSkill!.ForcedDependendSkill != null)
            {
                sB.AppendLine(resourceLoader.GetString("Skills_HowToUnlock_ContentForcedDependencyPrefix"));
                sB.AppendLine($"- {skillsService.Registry[(SkillIdentifier)SelectedSkill!.ForcedDependendSkill!].DisplayName}");
                if (SelectedSkill!.DependendSkills != null && SelectedSkill!.DependendSkills!.Any())
                {
                    sB.AppendLine();
                    sB.AppendLine(resourceLoader.GetString("Word_And"));
                    sB.AppendLine();
                }
            }
            if (SelectedSkill!.DependendSkills != null && SelectedSkill!.DependendSkills!.Any())
            {
                sB.AppendLine(resourceLoader.GetString("Skills_HowToUnlock_ContentDependencyPrefix"));
                foreach (var skillId in SelectedSkill!.DependendSkills!)
                {
                    sB.AppendLine($"- {skillsService.Registry[skillId].DisplayName}");
                }
            }
            CurrentSkillUnlockContent = sB.ToString();
        }

        private void ItemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedSkill = (Skill)e.ClickedItem;
            SkillsSplitView.IsPaneOpen = true;
        }

        private void LockedHyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            UnlockTeachingTip_0.Target = LockedHyperlinkButton;
            UnlockTeachingTip_0.IsOpen = true;
        }

        private void Skill_LockedHyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is HyperlinkButton button)
            {
                SelectedSkill = (Skill)button.DataContext;
                UnlockTeachingTip_0.Target = button;
                UnlockTeachingTip_0.IsOpen = true;
            }
        }

        private void SkillsSplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args) => UnlockTeachingTip_0.IsOpen = false;

        private void SkillsSplitView_PaneOpening(SplitView sender, object args) => UnlockTeachingTip_0.IsOpen = false;

        private void ItemsScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) => UnlockTeachingTip_0.IsOpen = false;

        private void IncrDecrButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) => ElementSoundPlayer.Play(ElementSoundKind.Focus);
    }
}
