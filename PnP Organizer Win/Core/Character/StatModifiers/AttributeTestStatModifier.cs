using PnPOrganizer.Core.BattleAssistant;

namespace PnPOrganizer.Core.Character.StatModifiers
{
    public struct AttributeTestStatModifier : IStatModifier
    {
        public string AttributeTestName { get; private set; }
        public Dice Dice { get; private set; }
        public int Bonus { get; private set; }
        public bool Toggleable { get; private set; }

        public AttributeTestStatModifier(string attributeTestName, int bonus, bool toggleable = false) 
            : this(attributeTestName, Dice.D1, bonus, toggleable) { }

        public AttributeTestStatModifier(string attributeTestName, Dice dice, int bonus = 0, bool toggleable = false)
        {
            AttributeTestName = attributeTestName;
            Dice = dice;
            Bonus = bonus;
            Toggleable = toggleable;
        }
    }
}
