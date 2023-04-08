using System.Reflection;

namespace PnPOrganizer.Core.Character.StatModifiers
{
    public struct CharacterStatModifier : IStatModifier
    {
        public PropertyInfo StatPropertyInfo { get; private set; }
        public int Bonus { get; private set; }

        public CharacterStatModifier(string statPropertyName, int bonus) 
        {
            // TODO
            //StatPropertyInfo = typeof(OverviewViewModel).GetProperty(statPropertyName)!;
            Bonus = bonus;
        }
    }
}
