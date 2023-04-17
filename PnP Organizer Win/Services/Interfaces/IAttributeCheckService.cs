using PnPOrganizer.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnPOrganizer.Services.Interfaces
{
    public interface IAttributeCheckService : ISaveData
    {
        public IDictionary<AttributeType, IList<AttributeCheck>> AttributeChecks { get; }
        public ICollection<Profession> Professions { get; }

        public List<AttributeCheck> GetAsList();

        public Dictionary<AttributeCheckType, AttributeCheck> GetSubDictionary();

        public void ApplyStatModifiers();

        public void Refresh(AttributeType attributeType);

        public void Refresh(AttributeCheckType attributeCheckType);

        public void RefreshAll();
    }
}
