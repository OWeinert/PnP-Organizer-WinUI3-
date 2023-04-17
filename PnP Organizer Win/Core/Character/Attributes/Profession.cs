using CommunityToolkit.Mvvm.ComponentModel;

namespace PnPOrganizer.Core.Attributes
{
    /// <summary>
    /// Professions give a Bonus to the attached AttributeCheck
    /// </summary>
    public partial class Profession : ObservableObject
    {
        [ObservableProperty]
        private AttributeCheck _attributeCheck;

        [ObservableProperty]
        private int _bonus;

        public Profession(AttributeCheck attributeCheck) 
        {
            AttributeCheck = attributeCheck;
            attributeCheck.ProfessionBoni.Add(this);
        }
        
        public void RemoveFromAttributeCheck()
        {
            AttributeCheck.ProfessionBoni.Remove(this);
        }
    }
}
