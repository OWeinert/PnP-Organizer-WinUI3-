using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
