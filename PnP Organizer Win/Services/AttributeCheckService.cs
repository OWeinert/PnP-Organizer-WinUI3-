using PnPOrganizer.Core.Attributes;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Core.Character.Attributes;
using PnPOrganizer.Core.Character.StatModifiers;
using PnPOrganizer.Services.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace PnPOrganizer.Services
{
    public class AttributeCheckService : IAttributeCheckService
    {
        private readonly IDictionary<AttributeType, IList<AttributeCheck>> _attributeChecks;
        public IDictionary<AttributeType, IList<AttributeCheck>> AttributeChecks => _attributeChecks;

        private ICollection<Profession> _professions;
        public ICollection<Profession> Professions => _professions;

        private readonly ISkillsService _skillsService;
        private readonly IAttributeService _attributeService;

        public AttributeCheckService(IAttributeService attributeService, ISkillsService skillsService)
        {
            _attributeService = attributeService;
            _skillsService = skillsService;
            var resourceLoader = ResourceLoader.GetForViewIndependentUse();

            var strAttr = attributeService.Attributes[AttributeType.Strength];
            var conAttr = attributeService.Attributes[AttributeType.Constitution];
            var dexAttr = attributeService.Attributes[AttributeType.Dexterity];
            var intAttr = attributeService.Attributes[AttributeType.Intelligence];
            var wisAttr = attributeService.Attributes[AttributeType.Wisdom];
            var chaAttr = attributeService.Attributes[AttributeType.Charisma];

            var strChecks = new List<AttributeCheck>()
            {
                new AttributeCheck(AttributeCheckType.Athletics, resourceLoader.GetString("AttributeCheck_Athletics"), strAttr)
            };
            var conChecks = new List<AttributeCheck>()
            {
                new AttributeCheck(AttributeCheckType.Physique, resourceLoader.GetString("AttributeCheck_Physique"), conAttr)
            };
            var dexChecks = new List<AttributeCheck>()
            {
                new AttributeCheck(AttributeCheckType.Acrobatics, resourceLoader.GetString("AttributeCheck_Acrobatics"), dexAttr),
                new AttributeCheck(AttributeCheckType.SleightOfHand, resourceLoader.GetString("AttributeCheck_SleightOfHand"), dexAttr),
                new AttributeCheck(AttributeCheckType.Sneak, resourceLoader.GetString("AttributeCheck_SneakHide"), dexAttr)
            };
            var intChecks = new List<AttributeCheck>()
            {
                new AttributeCheck(AttributeCheckType.History, resourceLoader.GetString("AttributeCheck_History"), intAttr),
                new AttributeCheck(AttributeCheckType.Nature, resourceLoader.GetString("AttributeCheck_Nature"), intAttr),
                new AttributeCheck(AttributeCheckType.Inspect, resourceLoader.GetString("AttributeCheck_Inspect"), intAttr),
            };
            var wisChecks = new List<AttributeCheck>()
            {
                new AttributeCheck(AttributeCheckType.Insight, resourceLoader.GetString("AttributeCheck_Insight"), wisAttr),
                new AttributeCheck(AttributeCheckType.FirstAid, resourceLoader.GetString("AttributeCheck_FirstAid"), wisAttr),
                new AttributeCheck(AttributeCheckType.HandleAnimals, resourceLoader.GetString("AttributeCheck_HandleAnimals"), wisAttr),
                new AttributeCheck(AttributeCheckType.Perceive, resourceLoader.GetString("AttributeCheck_Perceive"), wisAttr),
            };
            var chaChecks = new List<AttributeCheck>()
            {
                new AttributeCheck(AttributeCheckType.Intimidate, resourceLoader.GetString("AttributeCheck_Intimidate"), chaAttr),
                new AttributeCheck(AttributeCheckType.Performance, resourceLoader.GetString("AttributeCheck_Performance"), chaAttr),
                new AttributeCheck(AttributeCheckType.Bluff, resourceLoader.GetString("AttributeCheck_Bluff"), chaAttr),
                new AttributeCheck(AttributeCheckType.Persuade, resourceLoader.GetString("AttributeCheck_Persuade"), chaAttr),
            };

            _attributeChecks = new Dictionary<AttributeType, IList<AttributeCheck>>()
            {
                { AttributeType.Strength, strChecks },
                { AttributeType.Constitution, conChecks },
                { AttributeType.Dexterity, dexChecks },
                { AttributeType.Intelligence, intChecks },
                { AttributeType.Wisdom, wisChecks },
                { AttributeType.Charisma, chaChecks }
            };

            _professions = new Collection<Profession>();
        }

        public List<AttributeCheck> GetAsList()
        {
            var resultList = AttributeChecks.SelectMany(attrCheck => attrCheck.Value).ToList();
            return resultList;
        }

        public Dictionary<AttributeCheckType, AttributeCheck> GetSubDictionary()
        {
            var resultDict = GetAsList().ToDictionary(attrCheck => attrCheck.CheckType);
            return resultDict;
        }

        public void ApplyStatModifiers()
        {
            GetAsList().ForEach(attributeCheck =>
            {
                attributeCheck.StatModifierBoni.Clear();
                attributeCheck.StatModifierDiceBoni.Clear();
            });
            var activeStatModifiers = _skillsService.GetActiveStatModifiers<AttributeCheckStatModifier>();
            foreach (var statModifier in activeStatModifiers)
            {
                var temp = GetAsList();
                var attributeChecks = GetAsList().Where(attrCheck => attrCheck.CheckType == statModifier.AttributeCheckType);
                if(attributeChecks.Any())
                {
                    var attrCheck = attributeChecks.First();
                    if ((statModifier.Toggleable && statModifier.IsToggled) || !statModifier.Toggleable)
                    {
                        attrCheck.StatModifierBoni.Add(statModifier.Bonus);
                        attrCheck.StatModifierDiceBoni.Add(statModifier.Dice);
                    }
                }
            }
        }

        public void Refresh(AttributeType attributeType)
        {
            foreach(var attribute in AttributeChecks[attributeType])
            {
                attribute.Refresh();
            }
        }

        public void Refresh(AttributeCheckType attributeCheckType) => GetAsList().First(attrCheck => attrCheck.CheckType == attributeCheckType).Refresh();

        public void RefreshAll()
        {
            foreach(var attributeCheck in GetAsList())
            {
                attributeCheck.Refresh();
            }
        }

        public void LoadFromCharacter(CharacterData data)
        {
            Professions.Clear();
            foreach(var professionSaveData in data.Professions)
            {
                var attributeCheck = GetSubDictionary()[professionSaveData.AttributeCheckType];
                var profession = new Profession(attributeCheck);
                profession.Bonus = professionSaveData.Bonus;
                Professions.Add(profession);
            }
        }

        public void SaveToCharacter(ref CharacterData data)
        {
            data.Professions.Clear();
            foreach(var profession in Professions)
            {
                data.Professions.Add(new ProfessionSaveData()
                {
                    AttributeCheckType = profession.AttributeCheck.CheckType,
                    Bonus = profession.Bonus
                });
            }
        }

        public void ResetData()
        {
            Professions.Clear();
            foreach (var attributeCheck in GetAsList())
            {
                attributeCheck.AttributeBonus = 0;
                attributeCheck.PearlBonus = 0;
                attributeCheck.ProfessionBoni.Clear();
                attributeCheck.StatModifierBoni.Clear();
                attributeCheck.StatModifierDiceBoni.Clear();
            }
        }
    }
}
