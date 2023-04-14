using PnPOrganizer.Core.Character.StatModifiers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PnPOrganizer.Core.Character.SkillSystem
{
    public interface ISkillsService
    {
        public event EventHandler? ViewRefreshRequested;

        public Dictionary<SkillIdentifier, Skill> Registry { get; }

        public Skill CreateAndRegisterSkill(string name, string displayName, SkillCategory skillCategory = SkillCategory.Character,
            int maxSkillPoints = 1, string description = "", IStatModifier[]? skillModifiers = null, SkillIdentifier[]? skillDependencies = null,
            ActivationType activationType = ActivationType.Passive, int energyCost = 0, int staminaCost = 0, int usesPerBattle = -1,
            int skillTreeCheckpoint = 0);

        public Skill RegisterSkill(Skill skill);

        public int GetSkillIndex(SkillIdentifier identifier);

        public Skill? GetSkill(int index);

        public Skill? GetSkillFromStatModifier(IStatModifier statModifier);

        public IReadOnlyDictionary<SkillIdentifier, Skill> GetSkillsFromStatModifier(IStatModifier statModifier);

        public void LoadSkillSaveData(CharacterData data);

        public void SaveSkillSaveData(ref CharacterData data);

        public void ResetSkills();
    }
}
