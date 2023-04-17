using PnPOrganizer.Core.Character;

namespace PnPOrganizer.Services.Interfaces
{
    public interface ISaveData
    {
        public void LoadFromCharacter(CharacterData data);

        public void SaveToCharacter(ref CharacterData data);

        public void ResetData();
    }
}
