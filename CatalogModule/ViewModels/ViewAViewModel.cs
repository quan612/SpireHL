using Prism.Events;
using Prism.Mvvm;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace WhitelistModule.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private string _title = "Hello from ViewAViewModel";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string LoggedInName
        {
            get { return _loggedInName; }
            set { SetProperty(ref _loggedInName, value); }
        }

        private string _loggedInName;

        public DelegateCommand ClickCommand { get; private set; }

        public ViewAViewModel()
        {

            ClickCommand = 
                new DelegateCommand(Click)
                    .ObservesCanExecute(()=>CanExecute)
                ;

            // CustomIdentity myid = Thread.CurrentPrincipal.Identity as CustomIdentity;

        }

        private bool _canExecute =false;
        private IEventAggregator _eventAggregator;

        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                SetProperty(ref _canExecute, value);
                // ClickCommand.RaiseCanExecuteChanged();
            }
        }

        private void Click()
        {
            Title = "you clicked me";
        }
    }
}
