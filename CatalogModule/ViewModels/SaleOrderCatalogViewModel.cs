using CatalogModule.Contracts;
using CatalogModule.Enums;
using CatalogModule.Repository;
using Prism.Events;
using Prism.Services.Dialogs;
using SpireHL.Core.Enums;
using SpireHL.Core.Events;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace CatalogModule.ViewModels
{
    public class SaleOrderCatalogViewModel : ViewModelBase
    {
        private SaleOrderRepository _repository;
        private List<SpireSaleOrderItem> _itemsFromDb;
        private List<SpireSaleOrderItem> _temporaryList;

        public IWordCatalogService WordCatalogService { get; set; }
        public IExcelCatalogService ExcelCatalogService { get; set; }

        private SaleOrderType _selectedSaleOrderType;

        public SaleOrderType SelectedSaleOrderType
        {
            get { return _selectedSaleOrderType; }
            set
            {
                SetProperty(ref _selectedSaleOrderType, value);
            }
        }

        private string _saleOrderNumber;

        public string SaleOrderNumber
        {
            get { return _saleOrderNumber; }
            set { SetProperty(ref _saleOrderNumber, value); }
        }


        private ObservableCollection<SpireSaleOrderItem> _saleOrderDisplayItems;
        public ObservableCollection<SpireSaleOrderItem> SaleOrderDisplayItems
        {
            get
            {
                return _saleOrderDisplayItems;
            }
            set
            {
                SetProperty(ref _saleOrderDisplayItems, value);
            }
        }

        private ICollectionView _saleOrderListViewItems;
        public ICollectionView SaleOrderListViewItems
        {
            get => _saleOrderListViewItems;
            set
            {
                SetProperty(ref _saleOrderListViewItems, value);
            }
        }

        public SaleOrderCatalogViewModel(SaleOrderRepository saleOrderRepository, IDialogService dialogService, IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
            _repository = saleOrderRepository;
            _temporaryList = new List<SpireSaleOrderItem>();
            _itemsFromDb = new List<SpireSaleOrderItem>();

            SaleOrderDisplayItems = new ObservableCollection<SpireSaleOrderItem>();
            SaleOrderListViewItems = new ListCollectionView(SaleOrderDisplayItems);
        }

        #region Query
        private DelegateCommand _queryCommand;
        public ICommand QueryCommand => _queryCommand ?? (_queryCommand =
            new DelegateCommand(QuerySaleOrder));

        private async void QuerySaleOrder()
        {
            _itemsFromDb.Clear();
            SaleOrderDisplayItems.Clear();

            if (string.IsNullOrEmpty(SaleOrderNumber))
            {
                DialogService.ShowException(new Exception("Please enter a number for Sale Order "));
                return;
            }

            EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(true, "Getting Spire Items..."));
            try
            {
                if (SelectedSaleOrderType == SaleOrderType.SaleOrder)
                {
                    _itemsFromDb = await Task.Run(() => _repository.GetSaleOrderItems(SaleOrderNumber).OrderBy(x => x.PartNo).ToList());
                }
                if (SelectedSaleOrderType == SaleOrderType.SaleHistory)
                {
                    _itemsFromDb = await Task.Run(() => _repository.GetSaleHistoryItems(SaleOrderNumber).OrderBy(x => x.PartNo).ToList());
                }

                _itemsFromDb.ForEach(item => _temporaryList.Add(item));
                RefreshCurrentView();
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

        #region Clear
        private DelegateCommand _clearCommand;
        public ICommand ClearCommand => _clearCommand ?? (_clearCommand =
            new DelegateCommand(ClearList));

        private void ClearList()
        {
            _itemsFromDb.Clear();
            _temporaryList.Clear();
            SaleOrderDisplayItems.Clear();
            SaleOrderListViewItems.Refresh();
        }
        #endregion

        #region Save Excel
        private DelegateCommand _saveExcelCommand;
        public ICommand SaveExcelCommand => _saveExcelCommand ?? (_saveExcelCommand = new DelegateCommand(SaveExcel));

        public async void SaveExcel()
        {
            ExcelCatalogService = CatalogServiceFactory.GetExcelSaleOrderCatalogService(ExcelSaleOrderCatalogType.SaleOrder);

            bool isSaveSucceed = false;
            string pathToFileSaved = string.Empty;

            EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(true, "Exporting Sale Order Spire Items To Catalog"));
            try
            {
                pathToFileSaved = await Task.Run(() => ExcelCatalogService?.MakeCatalog<SpireSaleOrderItem>(SaleOrderDisplayItems));
                isSaveSucceed = true;
            }
            catch (Exception ex)
            {
                DialogService.ShowException(ex);
            }
            finally
            {
                EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(false));

                if (isSaveSucceed)
                {
                    var dialogParameters = new DialogParameters()
                    {
                        { "PathToFile", pathToFileSaved}
                    };
                    DialogService.ShowSummary(dialogParameters);
                }
            }
        }
        #endregion

        #region Apply Sort
        private string _sortField = "Id";
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public void ApplyCustomSort(string sortField, ListSortDirection sortDirection)
        {
            _sortField = sortField;
            _sortDirection = sortDirection;
            RefreshCurrentView();
        }
        #endregion

        #region Refresh view

        private void RefreshCurrentView()
        {
            var sortedList = GetSortedListItems<SpireSaleOrderItem>(_temporaryList, _sortField, _sortDirection);
            SaleOrderDisplayItems.Clear();
            sortedList.ForEach(item => SaleOrderDisplayItems.Add(item));
            SaleOrderListViewItems.Refresh();
        }

        private List<T> GetSortedListItems<T>(IEnumerable<T> listItems, string sortField, ListSortDirection sortDirection)
        {
            System.Reflection.PropertyInfo prop = typeof(T).GetProperty(sortField);

            return sortDirection == ListSortDirection.Descending ?
                                    listItems
                                        .OrderByDescending(i => prop.GetValue(i, null)).ToList() :
                                    listItems
                                        .OrderBy(i => prop.GetValue(i, null)).ToList();
        }
        #endregion
    }
}