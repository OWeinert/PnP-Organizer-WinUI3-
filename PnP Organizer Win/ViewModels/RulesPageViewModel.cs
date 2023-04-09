using PnPOrganizer.ViewModels.Interfaces;
using System;

namespace PnPOrganizer.ViewModels
{
    public class RulesPageViewModel : IViewModel
    {
        public bool IsInitialized => throw new NotImplementedException();

        public event EventHandler? Initialized;

        public RulesPageViewModel() 
        {
            Initialize();
        }

        public void Initialize()
        {
        }
    }
}
