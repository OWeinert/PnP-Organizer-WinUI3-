using PnPOrganizer.Core.BattleAssistant;
using PnPOrganizer.Core.Character.SkillSystem;

namespace PnPOrganizer.Core.Character.StatModifiers
{
    public struct CalculatorStatModifier : IStatModifier
    {
        public CalculatorValueType CalculatorValueType { get; private set; }
        public ApplianceMode ApplianceMode { get; private set; }
        public Dice Dice { get; private set; }
        public double Bonus { get; private set; }
        public CalculatorBonusType CalculatorBonusType { get; private set; }

        public CalculatorStatModifier(CalculatorValueType calculatorValueType, ApplianceMode applianceMode, double bonus,
            CalculatorBonusType calculatorBonusType = CalculatorBonusType.Additive)
            : this(calculatorValueType, applianceMode, Dice.D1, bonus, calculatorBonusType) { }

        public CalculatorStatModifier(CalculatorValueType calculatorValueType, ApplianceMode applianceMode, Dice dice, double bonus = 0, 
            CalculatorBonusType calculatorBonusType = CalculatorBonusType.Additive)
        {
            CalculatorValueType = calculatorValueType;
            ApplianceMode = applianceMode;
            Dice = dice;
            Bonus = bonus;
            CalculatorBonusType = calculatorBonusType;
        }
    }
}
