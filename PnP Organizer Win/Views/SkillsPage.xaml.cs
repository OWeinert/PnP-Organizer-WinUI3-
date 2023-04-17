using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using PnPOrganizer.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Resources;

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

        [ObservableProperty]
        private SkillabilityFilter _currentSkillabilityFilter;

        [ObservableProperty]
        private string _searchBoxText = string.Empty;

        public SkillsPage()
        {
            ViewModel = Ioc.Default.GetRequiredService<SkillsPageViewModel>();
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            CurrentSkillabilityFilter = ViewModel.SkillabilityFilters[0];
        }

        partial void OnSearchBoxTextChanged(string value) => UpdateFilter();

        partial void OnCurrentSkillabilityFilterChanged(SkillabilityFilter value) => UpdateFilter();

        private void SkillTreeMenuFlyoutItem_Click(object sender, RoutedEventArgs e) => UpdateFilter();

        private void UpdateFilter()
        {
            ViewModel.SkillsView.Filter = entry =>
            {
                if (entry is Skill skill)
                {
                    var correctSkillTree = (skill.Identifier.SkillCategory == SkillCategory.Character && ViewModel.SkillTreeCheckedCharacter)
                                            || (skill.Identifier.SkillCategory == SkillCategory.Melee && ViewModel.SkillTreeCheckedMelee)
                                            || (skill.Identifier.SkillCategory == SkillCategory.Ranged && ViewModel.SkillTreeCheckedRanged);

                    var correctSkillability = CurrentSkillabilityFilter.Skillability switch
                    {
                        Skillability.All => true,
                        Skillability.Locked => skill.IsSkillableInverted,
                        Skillability.Unlocked => skill.IsSkillable,
                        Skillability.Skilled => skill.IsActive,
                        _ => true,
                    };

                    var nameContainedInSearchBox = string.IsNullOrWhiteSpace(SearchBoxText);
                    if (SearchBoxText.Any())
                        nameContainedInSearchBox = skill.DisplayName.Contains(SearchBoxText, StringComparison.OrdinalIgnoreCase);

                    return correctSkillability && correctSkillTree && nameContainedInSearchBox;
                }
                return false;
            };
            ViewModel.SkillsView.RefreshFilter();
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

        private void SearchSkillBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableSkills = new List<Skill>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var entry in ViewModel.SkillsView.SourceCollection)
                {
                    if(entry is Skill skill)
                    {
                        var found = splitText.All((key) =>
                        {
                            return skill.DisplayName.Contains(key, StringComparison.OrdinalIgnoreCase);
                        });
                        if (found)
                        {
                            suitableSkills.Add(skill);
                        }
                    }
                }
                if (suitableSkills.Count == 0)
                {
                    sender.ItemsSource = new List<string>()
                    {
                        $"No Skill found containing \"{sender.Text}\""
                    };
                }
                else
                    sender.ItemsSource = suitableSkills.ConvertAll(skill => skill.DisplayName);
            }
        }
    }
}
