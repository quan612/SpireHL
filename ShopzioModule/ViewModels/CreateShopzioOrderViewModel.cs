using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using ShopzioModule.Models;
using ShopzioModule.Services;
using SpireHL.Core;
using SpireHL.Core.Events;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using SpireHL.Core.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace ShopzioModule.ViewModels
{
    public class CreateShopzioOrderViewModel : ViewModelBase
    {
        private SpireShopzioRepository _repository;
        private List<SpireShopzioItem> _itemsFromDb;

        private string _selectedExcel;

        public string SelectedExcel
        {
            get { return _selectedExcel; }
            set { SetProperty(ref _selectedExcel, value); }
        }

        private ObservableCollection<SpireShopzioItem> _inventoryListDisplayItems;
        public ObservableCollection<SpireShopzioItem> InventoryListDisplayItems
        {
            get
            {
                return _inventoryListDisplayItems;
            }
            set
            {
                SetProperty(ref _inventoryListDisplayItems, value);
            }
        }

        private ICollectionView _listViewSpireItems;
        public ICollectionView InventoryListViewItems
        {
            get => _listViewSpireItems;
            set
            {
                SetProperty(ref _listViewSpireItems, value);
            }
        }

        private string _customerName = "Test";

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                SetProperty(ref _customerName, value);
                if (_userOptions != null)
                {
                    _userOptions.CustomerName = _customerName;
                }

            }
        }

        private bool _isPriceOverride = true;

        public bool IsPriceOverride
        {
            get { return _isPriceOverride; }
            set
            {
                SetProperty(ref _isPriceOverride, value);
                if (_userOptions != null)
                {
                    _userOptions.IsPriceOverride = _isPriceOverride;
                }
            }
        }

        private bool _isQuantityOverride = true;

        public bool IsQuantityOverride
        {
            get { return _isQuantityOverride; }
            set
            {
                SetProperty(ref _isQuantityOverride, value);
                if (_userOptions != null)
                {
                    _userOptions.IsQuantityOverride = _isQuantityOverride;
                }
            }
        }

        private ShopzioCreateOrderUserOptions _userOptions { get; set; }

        private string _orderNumber = string.Empty;

        public string OrderNumber
        {
            get { return _orderNumber; }
            set
            {
                SetProperty(ref _orderNumber, value);
                RaisePropertyChanged(nameof(ShowOrderNumber));
            }
        }

        public bool ShowOrderNumber => !string.IsNullOrEmpty(OrderNumber);


        public CreateShopzioOrderViewModel(SpireShopzioRepository repository, IDialogService dialogService, IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
            _repository = repository;
            //CurrentInventory = new InventoryList();

            //UserSelectOptions = new UserSelectOptions();
            //_temporaryList = new List<SpireItem>();
            _itemsFromDb = new List<SpireShopzioItem>();

            InventoryListDisplayItems = new ObservableCollection<SpireShopzioItem>();
            InventoryListViewItems = new ListCollectionView(InventoryListDisplayItems);

            IsPriceOverride = true;
            IsQuantityOverride = true;

            _userOptions = new ShopzioCreateOrderUserOptions()
            {
                CustomerName = this.CustomerName,
                IsPriceOverride = this.IsPriceOverride,
                IsQuantityOverride = this.IsQuantityOverride
            };

        }

        #region Query

        private DelegateCommand _openFileCommand;
        private DelegateCommand<object> _queryCommand;

        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand =
            new DelegateCommand(OpenExcelFile));

        private void OpenExcelFile()
        {
            SelectedExcel = DialogService.ShowOpenFileDialog(ModuleConstants.ProjectDirectory);
        }

        public ICommand QueryCommand => _queryCommand ?? (_queryCommand =
            new DelegateCommand<object>(QuerySpireItems));

        private async void QuerySpireItems(object queryType)
        {
            _itemsFromDb.Clear();
            InventoryListDisplayItems.Clear();

            if (string.IsNullOrEmpty(SelectedExcel))
            {
                DialogService.ShowException(new Exception("Please select an excel file to query items from excel"));
                return;
            }

            EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(true, "Getting Spire Items..."));
            try
            {
                _itemsFromDb = await Task.Run(() => _repository.GetSpireShopzioItemsFromExcel(SelectedExcel, _userOptions).OrderBy(x => x.PartNo).ToList());
                _itemsFromDb.ForEach(item => InventoryListDisplayItems.Add(item));
                InventoryListViewItems.Refresh();
            }
            catch (Exception ex)
            {
                DialogService.ShowException(ex);
            }
            finally
            {
                EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(false));
            }
        }
        #endregion

        private DelegateCommand _runImportCommand;
        public ICommand RunImportCommand => _runImportCommand ?? (_runImportCommand =
            new DelegateCommand(RunImport));

        private void RunImport()
        {
            var service = new ShopzioService(_itemsFromDb, _userOptions);
            OrderNumber = service.RunImport();
        }

    }
}