using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using SpireHL.Base;
using SpireHL.Core.Events;
using SpireHL.Core.Models;
using System.Windows;

namespace SpireHL.ViewModels
{
    public class HullWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand CloseViewClick { get; private set; }
        public DelegateCommand AppOptionsClick { get; private set; }
        public DelegateCommand ExitAppClick { get; private set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { SetProperty(ref _isBusy, value); }
        }

        private string _busyMessage;
        public string BusyMessage
        {
            get => _busyMessage;
            set { SetProperty(ref _busyMessage, value); }
        }

        public HullWindowViewModel(IRegionManager regionManager, IModuleCatalog moduleCatalog, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<IsBusyEvent>().Subscribe(IndicatingAppBusy);

            SetDelegateCommands();
        }

        private void IndicatingAppBusy(BusyEventPayLoad busyPayLoad)
        {
            IsBusy = busyPayLoad.IsBusy;
            BusyMessage = busyPayLoad?.Message;
        }

        private void SetDelegateCommands()
        {
            NavigateCommand = new DelegateCommand<string>(Navigate);
            CloseViewClick = new DelegateCommand(CloseViewClickCommand);
            AppOptionsClick = new DelegateCommand(AppOptionsClickCommand);
            ExitAppClick = new DelegateCommand(ExitAppClickCommand);
        }

        private void CloseViewClickCommand()
        {
            var activeViews = _regionManager.Regions[RegionNames.MainDeckRegion].ActiveViews;
            foreach (var activeView in activeViews)
            {
                _regionManager.Regions[RegionNames.MainDeckRegion].Remove(activeView);
            }
        }

        private void ExitAppClickCommand()
        {
            Application.Current.Shutdown();
        }

        private void AppOptionsClickCommand()
        {
            _regionManager.RequestNavigate(RegionNames.MainDeckRegion, "Options", Callback);
        }

        private void Navigate(string viewName)
        {
            if (viewName != null)
                _regionManager.RequestNavigate(RegionNames.MainDeckRegion, viewName, Callback);
        }

        private void Callback(NavigationResult result)
        {
            if (result.Error != null)
            {
                //TODO handle error here.
            }
        }
    }
}
