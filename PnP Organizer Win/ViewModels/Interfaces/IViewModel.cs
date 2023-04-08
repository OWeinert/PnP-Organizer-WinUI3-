using System;

namespace PnPOrganizer.ViewModels.Interfaces
{
    public interface IViewModel
    {
        public event EventHandler? Initialized;

        public bool IsInitialized { get; }

        public void Initialize();
    }
}
