using PnPOrganizer.ViewModels.Interfaces;

namespace PnPOrganizer.Views.Interfaces
{
    public interface IViewFor<TViewModel> : IView where TViewModel : IViewModel
    {
        public TViewModel ViewModel { get; }
    }
}
