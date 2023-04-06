using System.Collections.Generic;

namespace PnPOrganizer.Core.BattleAssistant
{
    public struct Dice
    {
        public static readonly List<Dice> Dices = new();

        public static readonly Dice D1 = new("1", 1, 1);
        public static readonly Dice D2 = new("2", 2);
        public static readonly Dice D4 = new("4", 4);
        public static readonly Dice D6 = new("6", 6);
        public static readonly Dice D8 = new("8", 8);
        public static readonly Dice D10 = new("10", 10);
        public static readonly Dice D10_10 = new("10 %", 10, 10);
        public static readonly Dice D12 = new("12", 12);
        public static readonly Dice D20 = new("20", 20);

        public string Name { get; set; }
        public int MaxValue { get; set; }
        public int Multiplier { get; set; }

        public Dice(string name, int maxValue, int multiplier)
        {
            Name = name;
            MaxValue = maxValue;
            Multiplier = multiplier;
            Dices.Add(this);
        }

        public Dice(string name, int maxValue) : this(name, maxValue, 1) { }
    }
}
