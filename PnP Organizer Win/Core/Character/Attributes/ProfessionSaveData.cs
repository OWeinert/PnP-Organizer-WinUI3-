using PnPOrganizer.Core.Attributes;

namespace PnPOrganizer.Core.Character.Attributes
{
    /// <summary>
    /// Serializable simplest version of a Profession
    /// </summary>
    public struct ProfessionSaveData
    {
        public AttributeCheckType AttributeCheckType { get; set; }
        public int Bonus { get; set; }
    }
}
