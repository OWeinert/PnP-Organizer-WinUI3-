using PnPOrganizer.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnPOrganizer.Services.Interfaces
{
    public interface IAttributeCheckService
    {
        public IDictionary<AttributeType, IList<AttributeCheck>> AttributeChecks { get; }

        public List<AttributeCheck> GetAsList();

        public void ApplyStatModifiers();

        public void Refresh(AttributeType attributeType);

        public void Refresh(AttributeCheckType attributeCheckType);

        public void RefreshAll();
    }
}
