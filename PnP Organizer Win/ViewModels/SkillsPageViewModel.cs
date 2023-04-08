using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

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

        [ObservableProperty]
        private int _usedSkillPoints = 0;

        public event EventHandler? Initialized;

        private ISkillsService _skillsService;

        public SkillsPageViewModel(ISkillsService skillsService) 
        {
            _skillsService = skillsService;
            Initialize();
            foreach (Skill skill in SkillsView.SourceCollection)
            {
                skill.PropertyChanged += Skill_PropertyChanged;
            }
        }

        public void Initialize()
        {
            _isInitialized = true;
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
                }));
            }
        }
    }
}
