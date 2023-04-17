using PnPOrganizer.ViewModels.Interfaces;
using System;

namespace PnPOrganizer.ViewModels
{
    public class ElementsPageViewModel : IViewModel
    {
        public bool IsInitialized { get; }

        public event EventHandler? Initialized;

        public void Initialize()
        {
        }
    }
}
