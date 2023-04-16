using PnPOrganizer.Core.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnPOrganizer.Services.Interfaces
{
    public interface ISaveData
    {
        public void LoadFromCharacter(CharacterData data);

        public void SaveToCharacter(ref CharacterData data);
    }
}
