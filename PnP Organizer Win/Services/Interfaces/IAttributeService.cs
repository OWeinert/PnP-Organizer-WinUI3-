using PnPOrganizer.Core.Attributes;
using System.Collections.Generic;

namespace PnPOrganizer.Services.Interfaces
{
    public interface IAttributeService : ISaveData
    {
        public IReadOnlyDictionary<AttributeType, Attribute> Attributes { get; }
    }
}
