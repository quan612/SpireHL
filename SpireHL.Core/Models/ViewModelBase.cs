using Prism.Events;
using Prism.Navigation;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace SpireHL.Core.Models
{
    public class ViewModelBase : BindableBaseWithValidation, INavigationAware, IDestructible
    {
        protected IDialogService DialogService;
        protected IEventAggregator EventAggregator;

        public ViewModelBase(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            DialogService = dialogService;
            EventAggregator = eventAggregator;
        }

        public ViewModelBase()
        {

        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
