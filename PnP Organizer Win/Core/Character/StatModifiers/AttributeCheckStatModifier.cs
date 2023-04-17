using PnPOrganizer.Core.Attributes;
using PnPOrganizer.Core.BattleAssistant;

namespace PnPOrganizer.Core.Character.StatModifiers
{
    public struct AttributeCheckStatModifier : IStatModifier
    {
        public AttributeCheckType AttributeCheckType { get; private set; }
        public Dice Dice { get; private set; }
        public int Bonus { get; private set; }
        public bool Toggleable { get; private set; }
        public bool IsToggled { get; set; }

        public AttributeCheckStatModifier(AttributeCheckType attributeCheckType, int bonus, bool toggleable = false) 
            : this(attributeCheckType, Dice.D1, bonus, toggleable) { }

        public AttributeCheckStatModifier(AttributeCheckType attributeCheckType, Dice dice, int bonus = 0, bool toggleable = false)
        {
            AttributeCheckType = attributeCheckType;
            Dice = dice;
            Bonus = bonus;
            Toggleable = toggleable;
            IsToggled = false;
        }
    }
}
