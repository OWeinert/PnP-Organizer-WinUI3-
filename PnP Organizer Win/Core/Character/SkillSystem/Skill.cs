﻿using Microsoft.Extensions.Logging;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.Core.Character.SkillSystem.EventArgs;
using PnPOrganizer.Core.Character.StatModifiers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace PnPOrganizer.Core.Character
{
    public class Skill
    {
        private SkillIdentifier _identifier;
        /// <summary>
        /// Internal identifier for the Skill consisting of SkillCategory and an internal unlocalized Name
        /// </summary>
        public SkillIdentifier Identifier { 
            get
            {
                return _identifier;
            }
            set {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, Identifier, value, nameof(Identifier)));
                _identifier = value;
            }
        }

        private string _displayName;
        /// <summary>
        /// Localized name visible to the User
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, DisplayName, value, nameof(DisplayName)));
                _displayName = value;
            }
        }

        private string _description;
        /// <summary>
        /// Localized description visible to the User
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, Description, value, nameof(Description)));
                _description = value;
            }
        }

        private int _skillPoints;
        /// <summary>
        /// Current skill points
        /// </summary>
        public int SkillPoints
        {
            get
            {
                return _skillPoints;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, SkillPoints, value, nameof(SkillPoints)));
                _skillPoints = value;
            }
        }

        private int _maxSkillPoints;
        /// <summary>
        /// Maximum skill points needed
        /// </summary>
        public int MaxSkillPoints
        {
            get
            {
                return _maxSkillPoints;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, MaxSkillPoints, value, nameof(MaxSkillPoints)));
                _maxSkillPoints = value;
            }
        }

        private int _energyCost;
        /// <summary>
        /// Used by the BattleAssistant to determine Energy usage
        /// </summary>
        public int EnergyCost
        {
            get
            {
                return _energyCost;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, EnergyCost, value, nameof(EnergyCost)));
                _energyCost = value;
            }
        }

        private int _staminaCost;
        /// <summary>
        /// Used by the BattleAssistant to determine Stamina usage
        /// </summary>
        public int StaminaCost
        {
            get
            {
                return _staminaCost;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, StaminaCost, value, nameof(StaminaCost)));
                _staminaCost = value;
            }
        }

        private int _skillTreeCheckpoint;
        /// <summary>
        /// Corresponding checkpoint (0-3) for this Skill
        /// </summary>
        public int SkillTreeCheckpoint
        {
            get
            {
                return _skillTreeCheckpoint;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, SkillTreeCheckpoint, value, nameof(SkillTreeCheckpoint)));
                _skillTreeCheckpoint = value;
            }
        }

        /// <summary>
        /// Repeatable Skills apply their Bonus for each time the MaxSkillPoints are reached
        /// </summary>
        public bool IsRepeatable { get; private set; }
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

        private int _usesLeft;
        /// <summary>
        /// Used by the BattleAssistant to determine how many times the Skill can be used in the current Battle
        /// </summary>
        public int UsesLeft
        {
            get
            {
                return _usesLeft;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, UsesLeft, value, nameof(UsesLeft)));
                _usesLeft = value;
            }
        }

        private int _usesPerBattle;
        /// <summary>
        /// Used by the BattleAssistant to determine how many times the Skill can be used per Battle
        /// -1 = infinite uses
        /// </summary>
        public int UsesPerBattle
        {
            get
            {
                return _usesPerBattle;
            }
            set
            {
                SkillChanged?.Invoke(this, new SkillChangedEventArgs(this, UsesPerBattle, value, nameof(UsesPerBattle)));
                _usesPerBattle = value;
            }
        }

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

        public event SkillChangedEventHandler? SkillChanged;
        public delegate void SkillChangedEventHandler(object sender, SkillChangedEventArgs e);

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
        }

        public static string CreateTooltip(Skill skill)
        {
            var sb = new StringBuilder(skill.Description);
            /*
            if (skill.StaminaCost > 0 || skill.EnergyCost > 0)
            {
                sb.Append($"\n{Resources.Skills_Cost}: ");

                if (skill.StaminaCost > 0)
                    sb.Append($"{skill.StaminaCost} {Resources.Overview_Stamina}");

                if (skill.StaminaCost > 0 && skill.EnergyCost > 0)
                    sb.Append(", ");

                if (skill.EnergyCost > 0)
                    sb.Append($"{skill.StaminaCost} {Resources.Overview_Energy}");
            }
            */
            return sb.ToString();
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

        /// <summary>
        /// Checks if the skill is active, which means that the max skill points are reached
        /// </summary>
        /// <returns></returns>
        public bool IsActive() => SkillPoints == MaxSkillPoints;

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
