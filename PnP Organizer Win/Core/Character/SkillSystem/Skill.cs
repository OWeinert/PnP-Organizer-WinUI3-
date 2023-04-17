using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PnPOrganizer.Core.Character.StatModifiers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace PnPOrganizer.Core.Character.SkillSystem
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Skill : ObservableObject
    {
        public static Skill Empty => new(new SkillIdentifier() { SkillCategory = SkillCategory.Character, Name = "" }, "", 0, "");

        /// <summary>
        /// Internal identifier for the Skill consisting of SkillCategory and an internal unlocalized Name
        /// </summary>
        [ObservableProperty]
        private SkillIdentifier _identifier;

        /// <summary>
        /// Localized name visible to the User
        /// </summary>
        [ObservableProperty]
        private string _displayName;

        /// <summary>
        /// Localized description visible to the User
        /// </summary>
        [ObservableProperty] 
        private string _description;

        /// <summary>
        /// Display text for SkillPoints. Formatted as "SkillPoints / MaxSkillPoints"
        /// </summary>
        [ObservableProperty]
        private string _skillPointsDisplayText = string.Empty;

        /// <summary>
        /// Current skill points
        /// </summary>
        [ObservableProperty]
        private int _skillPoints;

        /// <summary>
        /// Maximum skill points needed
        /// </summary>
        [ObservableProperty] 
        private int _maxSkillPoints;

        /// <summary>
        /// Used by the BattleAssistant to determine Energy usage
        /// </summary>
        [ObservableProperty] 
        private int _energyCost;

        /// <summary>
        /// Used by the BattleAssistant to determine Stamina usage
        /// </summary>
        [ObservableProperty] 
        private int _staminaCost;

        /// <summary>
        /// Corresponding checkpoint (0-3) for this Skill
        /// </summary>
        [ObservableProperty] 
        private int _skillTreeCheckpoint;

        /// <summary>
        /// 
        /// </summary>
        [ObservableProperty]
        private bool _isSkillable;

        /// <summary>
        /// 
        /// </summary>
        [ObservableProperty]
        private bool _isSkillableInverted;

        /// <summary>
        /// 
        /// </summary>
        [ObservableProperty]
        private bool _isActive;

        [ObservableProperty]
        private bool _isActiveInverted;

        /// <summary>
        /// Repeatable Skills apply their Bonus for each time the MaxSkillPoints are reached
        /// </summary>
        public bool IsRepeatable { get; private set; }

        [ObservableProperty]
        private int _repetition = 0;

        /// <summary>
        /// Determines if the Skill's Bonus is Active or Passive
        /// </summary>
        public ActivationType ActivationType { get; private set; }

        /// <summary>
        /// Used by the BattleAssistant to check if this Skill can be used with other Skills or only alone
        /// </summary>
        public bool UsableWithOtherSkills { get; private set; }
        /// <summary>
        /// Used by the BattleAssistant to check if this Skill's Modifiers can change depending on the current Battle's turn count
        /// </summary>
        public bool HasRoundDependendModifiers { get; private set; }

        /// <summary>
        /// Used by the BattleAssistant to determine how many times the Skill can be used in the current Battle
        /// </summary>
        [ObservableProperty] 
        private int _usesLeft;

        /// <summary>
        /// Used by the BattleAssistant to determine how many times the Skill can be used per Battle
        /// -1 = infinite uses
        /// </summary>
        [ObservableProperty] 
        private int _usesPerBattle;

        /// <summary>
        /// Collection of StatModifiers which this Skill will apply when skilled
        /// </summary>
        public IStatModifier[]? StatModifiers { get; private set; }
        /// <summary>
        /// Used by the BattleAssistant when the given StatModifiers of this Skill differ per Turn in a Battle
        /// </summary>
        public List<IStatModifier[]> RoundDependendStatModifiers { get; private set; }

        /// <summary>
        /// True if the skill has round dependend StatModifiers AND does not depend on UsesLeft
        /// </summary>
        public bool IsRoundDependend { get; private set; }
        /// <summary>
        /// Used by the BattleAssistant to inform the Skill about the current Battle's turn count
        /// </summary>
        public int CurrentRound { get; set; }

        /// <summary>
        /// Names of skills of which at least one has to be skilled in order to unlock this skill.
        /// </summary>
        public SkillIdentifier[] DependendSkills { get; private set; }

        /// <summary>
        /// A name of a skill which has to be skilled in order to unlock this skill.
        /// This skill AND one of the other dependend skills have to be skilled.
        /// </summary>
        public SkillIdentifier? ForcedDependendSkill { get; private set; }

        public Skill(SkillIdentifier identifier, string displayName, int maxSkillPoints, string description, IStatModifier[]? statModifiers = null,
            SkillIdentifier[]? dependendSkillNames = null, ActivationType activationType = ActivationType.Passive, int energyCost = 0, int staminaCost = 0, int usesPerBattle = -1,
            int skillTreeCheckpoint = 0)
        {
            _identifier = identifier;
            _displayName = displayName;
            _description = description;
            _maxSkillPoints = maxSkillPoints;
            StatModifiers = statModifiers;
            _skillPoints = 0;

            ActivationType = activationType;
            _energyCost = energyCost;
            _staminaCost = staminaCost;

            IsRoundDependend = false;
            HasRoundDependendModifiers = false;
            UsableWithOtherSkills = true;

            _usesLeft = UsesPerBattle = usesPerBattle;

            _skillTreeCheckpoint = skillTreeCheckpoint;

            DependendSkills = dependendSkillNames ?? Array.Empty<SkillIdentifier>();

            RoundDependendStatModifiers = new List<IStatModifier[]>();

            SkillPointsDisplayText = $"{SkillPoints} / {MaxSkillPoints}";

            IsSkillableInverted = !IsSkillable;
            IsActiveInverted = !IsActive;

            PropertyChanged += Skill_PropertyChanged;
        }

        [RelayCommand]
        private void IncreaseSkillPoints()
        {
            if(Repetition < 99)
            {
                if (IsRepeatable && SkillPoints >= (MaxSkillPoints - 1))
                {
                    Repetition++;
                    SkillPoints = 0;
                }
                else if (SkillPoints < MaxSkillPoints)
                    SkillPoints++;
            }
        }

        [RelayCommand]
        private void DecreaseSkillPoints()
        {
            if (SkillPoints > 0)
                SkillPoints--;
            else if(IsRepeatable && Repetition > 0)
            {
                Repetition--;
                SkillPoints = MaxSkillPoints - 1;
            }
        }

        private void Skill_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName is nameof(SkillPoints) or nameof(MaxSkillPoints))
            {
                SkillPointsDisplayText = $"{SkillPoints} / {MaxSkillPoints}";
                if(!IsRepeatable)
                    IsActive = SkillPoints >= MaxSkillPoints;
            }
            if (e.PropertyName is nameof(Repetition) && IsRepeatable)
                IsActive = Repetition > 0;

            if (e.PropertyName is nameof(IsSkillable))
                IsSkillableInverted = !IsSkillable;

            if(e.PropertyName is nameof(IsActive))
                IsActiveInverted = !IsActive;
        }

        public Skill SetRepeatable(bool repeatable = true)
        {
            IsRepeatable = repeatable;
            return this;
        }

        public Skill AddForcedDependency(SkillIdentifier forcedDependendSkillName)
        {
            ForcedDependendSkill = forcedDependendSkillName;
            return this;
        }

        public Skill AddRoundDependendModifiers(int round, IStatModifier[] roundStatModifiers)
        {
            RoundDependencyModifierCheck();
            RoundDependendStatModifiers.Insert(round, roundStatModifiers);
            HasRoundDependendModifiers = true;
            return this;
        }

        public Skill AddRoundDependendModifier(int round, IStatModifier roundStatModifier) => AddRoundDependendModifiers(round, new IStatModifier[] { roundStatModifier });

        public Skill SetDependendOnUsesLeft()
        {
            IsRoundDependend = false;
            return this;
        }

        public Skill SetOnlySoloUsable()
        {
            UsableWithOtherSkills = false;
            return this;
        }

        private void RoundDependencyModifierCheck()
        {
            if (!IsRoundDependend && RoundDependendStatModifiers.Count > UsesPerBattle)
            {
                Log.Warning("Skill \"{name}\" has more round dependend StatModifiers than uses per battle!\n" +
                    $"The excess StatModifiers will not be used!", DisplayName);
            }
            if (StatModifiers != null)
            {
                Log.Warning("Skill \"{name}\" has set StatModifiers! They will be discarded in favor of round dependend StatModifiers.", DisplayName);
                StatModifiers = null; // Setting StatModifiers to null here, so they can't be used if set before, to prevent unwanted behaviour
            }
        }
    }
}
