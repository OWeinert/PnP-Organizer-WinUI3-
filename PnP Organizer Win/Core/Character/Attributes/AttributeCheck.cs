using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.Core.BattleAssistant;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PnPOrganizer.Core.Attributes
{
    public partial class AttributeCheck : ObservableObject
    {
        public AttributeCheckType CheckType { get; }

        [ObservableProperty]
        private string _displayName = string.Empty;

        [ObservableProperty]
        private Attribute? _attribute;

        [ObservableProperty]
        private int _attributeBonus;
        [ObservableProperty]
        private int _pearlBonus;
        [ObservableProperty]
        private ObservableCollection<int> _statModifierBoni = new();
        [ObservableProperty]
        private ObservableCollection<Dice> _statModifierDiceBoni = new();
        [ObservableProperty]
        private ObservableCollection<Profession> _professionBoni = new();
        [ObservableProperty]
        private int _bonusSum;

        [ObservableProperty]
        private int _minRollBonusSum;

        [ObservableProperty]
        private string _bonusText = string.Empty;

        public AttributeCheck(AttributeCheckType attributeCheckType, string displayName, Core.Attributes.Attribute attribute)
        {
            DisplayName = displayName;
            Attribute = attribute;
            AttributeBonus = Attribute.Bonus;
            CheckType = attributeCheckType;

            Attribute.PropertyChanged += Attribute_PropertyChanged;
            PropertyChanged += AttributeCheck_PropertyChanged;
            StatModifierBoni.CollectionChanged += StatModifierBoni_CollectionChanged;
            StatModifierDiceBoni.CollectionChanged += StatModifierDiceBoni_CollectionChanged;
            ProfessionBoni.CollectionChanged += ProfessionBoni_CollectionChanged;

            UpdateBonusSum();
            UpdateBonusText();
        }

        private void StatModifierBoni_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => UpdateBonusSum();

        private void StatModifierDiceBoni_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateMinRollBonusSum();
            UpdateBonusText();
        }

        private void ProfessionBoni_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach (Profession profession in e.NewItems!)
                {
                    profession.PropertyChanged += (sender, e) => UpdateBonusSum();
                }
            }
            UpdateBonusSum();
        }

        private void AttributeCheck_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(AttributeBonus) or nameof(StatModifierBoni) or nameof(PearlBonus))
                UpdateBonusSum();

            if (e.PropertyName is nameof(BonusSum))
            {
                UpdateMinRollBonusSum();
                UpdateBonusText();
            }
                
        }

        private void Attribute_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName is nameof(Core.Attributes.Attribute.Bonus))
                AttributeBonus = Attribute!.Bonus;
        }

        public void UpdateBonusSum() => BonusSum = AttributeBonus + PearlBonus + StatModifierBoni.Sum() + ProfessionBoni.Sum(p => p.Bonus);

        public void UpdateBonusText()
        {
            var sb = new StringBuilder();
            if (BonusSum != 0 || !StatModifierDiceBoni.Any())
                sb.Append($"{BonusSum} ");

            var sameDiceBoni = StatModifierDiceBoni.GroupBy(dice => dice.Name);
            foreach (var diceGroup in sameDiceBoni)
            {
                sb.Append($"+ {diceGroup.Count()}D{diceGroup.First().Name} ");
            }

            BonusText = sb.ToString();
        }

        public void UpdateMinRollBonusSum()
        {
            MinRollBonusSum = BonusSum + StatModifierDiceBoni.Count;
        }

        public void Refresh()
        {
            UpdateBonusSum();
            UpdateBonusText();
        }
    }

    public enum AttributeCheckType
    {
        Athletics,
        Acrobatics,
        Bluff,
        FirstAid,
        HandleAnimals,
        History,
        Insight,
        Inspect,
        Intimidate,
        Nature,
        Perceive,
        Performance,
        Persuade,
        Physique,
        SleightOfHand,
        Sneak
    }
}
