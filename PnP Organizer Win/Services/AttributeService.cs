using Microsoft.UI;
using PnPOrganizer.Core.Attributes;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Windows.ApplicationModel.Resources;

namespace PnPOrganizer.Services
{
    public class AttributeService : IAttributeService
    {
        private IReadOnlyDictionary<AttributeType, Core.Attributes.Attribute> _attributes;
        public IReadOnlyDictionary<AttributeType, Core.Attributes.Attribute> Attributes => _attributes;

        public AttributeService()
        {
            var resourceLoader = ResourceLoader.GetForViewIndependentUse();
            _attributes = new Dictionary<AttributeType, Core.Attributes.Attribute>()
            {
                { AttributeType.Strength, new Core.Attributes.Attribute(resourceLoader.GetString("Attributes_Strength"), Colors.Coral)},
                { AttributeType.Constitution, new Core.Attributes.Attribute(resourceLoader.GetString("Attributes_Constitution"), Colors.LightGreen)},
                { AttributeType.Dexterity, new Core.Attributes.Attribute(resourceLoader.GetString("Attributes_Dexterity"), Colors.LightSkyBlue)},
                { AttributeType.Intelligence, new Core.Attributes.Attribute(resourceLoader.GetString("Attributes_Intelligence"), Colors.Moccasin)},
                { AttributeType.Wisdom, new Core.Attributes.Attribute(resourceLoader.GetString("Attributes_Wisdom"), Colors.LightGray)},
                { AttributeType.Charisma, new Core.Attributes.Attribute(resourceLoader.GetString("Attributes_Charisma"), Colors.LightPink)},
            }.ToImmutableDictionary();
        }

        public void LoadFromCharacter(CharacterData data)
        {
            Attributes[AttributeType.Strength].Value = data.Attributes.Strength;
            Attributes[AttributeType.Constitution].Value = data.Attributes.Constitution;
            Attributes[AttributeType.Dexterity].Value = data.Attributes.Dexterity;
            Attributes[AttributeType.Intelligence].Value = data.Attributes.Intelligence;
            Attributes[AttributeType.Wisdom].Value = data.Attributes.Wisdom;
            Attributes[AttributeType.Charisma].Value = data.Attributes.Charisma;
        }

        public void SaveToCharacter(ref CharacterData data)
        {
            data.Attributes = new CharacterAttributes()
            {
                Strength = Attributes[AttributeType.Strength].Value,
                Constitution = Attributes[AttributeType.Constitution].Value,
                Dexterity = Attributes[AttributeType.Dexterity].Value,
                Intelligence = Attributes[AttributeType.Intelligence].Value,
                Wisdom = Attributes[AttributeType.Wisdom].Value,
                Charisma = Attributes[AttributeType.Charisma].Value
            };
        }
    }
}