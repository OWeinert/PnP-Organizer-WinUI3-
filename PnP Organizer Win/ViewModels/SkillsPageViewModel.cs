using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PnPOrganizer.ViewModels
{
    public partial class SkillsPageViewModel : ObservableObject, IViewModel
    {
        private bool _isInitialized = false;
        public bool IsInitialized => _isInitialized;

        private IAdvancedCollectionView? _skillsView;
        public IAdvancedCollectionView SkillsView
        {
            get
            {
                _skillsView ??= new AdvancedCollectionView(_skillsService.Registry.Values.ToList());
                return _skillsView;
            }
            set => SetProperty(ref _skillsView, value);
        }

        private IList<SkillabilityFilter>? _skillabilityFilters;
        public IList<SkillabilityFilter> SkillabilityFilters
        {
            get
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
                _skillabilityFilters ??= new List<SkillabilityFilter>()
                {
                    new SkillabilityFilter(resourceLoader.GetString("SkillsPage_SkillabilityAll"), Skillability.All),
                    new SkillabilityFilter(resourceLoader.GetString("SkillsPage_SkillabilityLocked"), Skillability.Locked),
                    new SkillabilityFilter(resourceLoader.GetString("SkillsPage_SkillabilityUnlocked"), Skillability.Unlocked),
                    new SkillabilityFilter(resourceLoader.GetString("SkillsPage_SkillabilitySkilled"), Skillability.Skilled),
                };
                return _skillabilityFilters;
            }
        }
        
        [ObservableProperty]
        private int _usedSkillPoints = 0;

        [ObservableProperty]
        private bool _skillTreeCheckedAll = true;
        [ObservableProperty]
        private bool _skillTreeCheckedCharacter = true;
        [ObservableProperty]
        private bool _skillTreeCheckedMelee = true;
        [ObservableProperty]
        private bool _skillTreeCheckedRanged = true;

        private bool _suppressPropertyChanged = false;

        public event EventHandler? Initialized;

        private readonly ISkillsService _skillsService;

        public SkillsPageViewModel(ISkillsService skillsService) 
        {
            _skillsService = skillsService;
            _skillsService.ViewRefreshRequested += SkillsService_ViewRefreshRequested;
            Initialize();
        }

        private void SkillsService_ViewRefreshRequested(object? sender, EventArgs e)
        {
            SkillsView?.Refresh();
        }

        public void Initialize()
        {
            PropertyChanged += SkillsPageViewModel_PropertyChanged;
            foreach (Skill skill in SkillsView.SourceCollection)
            {
                skill.PropertyChanged += Skill_PropertyChanged;
            }
            _isInitialized = true;
        }

        private void SkillsPageViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_suppressPropertyChanged)
            {
                _suppressPropertyChanged = true;
                if (e.PropertyName is nameof(SkillTreeCheckedCharacter) or nameof(SkillTreeCheckedMelee) or nameof(SkillTreeCheckedRanged))
                {
                    SkillTreeCheckedAll = (SkillTreeCheckedCharacter && SkillTreeCheckedMelee && SkillTreeCheckedRanged);
                    
                }
                if (e.PropertyName is nameof(SkillTreeCheckedAll))
                {
                    SkillTreeCheckedCharacter = SkillTreeCheckedMelee = SkillTreeCheckedRanged = SkillTreeCheckedAll;
                }
                _suppressPropertyChanged = false;
            }
        }

        private async void Skill_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var senderSkill = (Skill?)sender;
            if (senderSkill != null && e.PropertyName is nameof(senderSkill.SkillPoints))
            {
                UsedSkillPoints = await Task.Run(() => ((IEnumerable<Skill>)SkillsView.SourceCollection).Sum(skill =>
                {
                    var skillPointSum = skill.SkillPoints;
                    if (skill.IsRepeatable)
                        skillPointSum += skill.MaxSkillPoints * skill.Repetition;
                    return skillPointSum;
                })).AsAsyncOperation();
            }
        }
    }

    public readonly struct SkillabilityFilter
    {
        public string DisplayName { get; }
        public Skillability Skillability { get; }

        public SkillabilityFilter(string displayName, Skillability skillability)
        {
            DisplayName = displayName;
            Skillability = skillability;
        }
    }

    public enum Skillability
    {
        All,
        Locked,
        Unlocked,
        Skilled
    }
}
