namespace PnPOrganizer.Core.Character.SkillSystem.EventArgs
{
    public class SkillChangedEventArgs
    {
        public Skill Skill { get; }
        public object OldValue { get; }
        public object NewValue { get; }
        public string PropertyName { get; }

        public SkillChangedEventArgs(Skill skill, object oldValue, object newValue, string propertyName)
        {
            Skill = skill;
            OldValue = oldValue;
            NewValue = newValue;
            PropertyName = propertyName;
        }
    }
}
