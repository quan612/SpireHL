using CatalogModule.Contracts;
using CatalogModule.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using SpireHL.Core;
using SpireHL.Core.Contracts;
using SpireHL.Core.Enums;
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

namespace CatalogModule.ViewModels
{
    public class BaseCatalogViewModel : ViewModelBase
    {
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var test = navigationContext.Parameters["isViewLoaded"];
        }

        protected InventoryRepository _repository;
        protected List<SpireItem> ItemsFromDb { get; set; }
        protected List<SpireItem> TemporaryList { get; set; }

        public IUserDefine UserSelectOptions { get; set; }
        public IWordCatalogService WordCatalogService { get; set; }
        public IExcelCatalogService ExcelCatalogService { get; set; }
        public InventoryList CurrentInventory { get; set; }

        private string _selectedExcel;

        public string SelectedExcel
        {
            get { return _selectedExcel; }
            set { SetProperty(ref _selectedExcel, value); }
        }


        private IInventory _selectedInventory;

        public IInventory SelectedInventory
        {
            get { return _selectedInventory; }
            set
            {
                SetProperty(ref _selectedInventory, value);
            }
        }

        private CategoryType _selectedCategory;

        public CategoryType SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                SetProperty(ref _selectedCategory, value);
            }
        }

        private ObservableCollection<SpireItem> _inventoryListDisplayItems;
        public ObservableCollection<SpireItem> InventoryListDisplayItems
        {
            get { return _inventoryListDisplayItems; }
            set { SetProperty(ref _inventoryListDisplayItems, value); }
        }

        private DelegateCommand _selectFieldsCommand;
        public ICommand SelectFieldsCommand => _selectFieldsCommand ?? (_selectFieldsCommand =
            new DelegateCommand(SelectFields));

        private void SelectFields()
        {
            DialogService.ShowUserSelectOption(cb =>
            {
                if (cb.Result == ButtonResult.OK)
                {
                    UserSelectOptions = cb.Parameters.GetValue<UserSelectOptions>("UserSelect");
                }
            });
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

        public BaseCatalogViewModel(InventoryRepository catalogRepository, IDialogService dialogService, IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
            _repository = catalogRepository;
            CurrentInventory = new InventoryList();

            UserSelectOptions = new UserSelectOptions();
            TemporaryList = new List<SpireItem>();
            ItemsFromDb = new List<SpireItem>();

            InventoryListDisplayItems = new ObservableCollection<SpireItem>();
            InventoryListViewItems = new ListCollectionView(InventoryListDisplayItems);
        }


        #region Query
        private DelegateCommand _openFileCommand;
        private DelegateCommand<object> _queryCommand;

        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand =
            new DelegateCommand(OpenExcelFile));

        public ICommand QueryCommand => _queryCommand ?? (_queryCommand =
            new DelegateCommand<object>(QuerySpireItems));

        private void OpenExcelFile()
        {
            SelectedExcel = DialogService.ShowOpenFileDialog(ModuleConstants.ProjectDirectory);
        }

        public virtual async void QuerySpireItems(object queryType)
        {
            ItemsFromDb.Clear();
            InventoryListDisplayItems.Clear();

            if ((QueryType)queryType == QueryType.FromExcel && string.IsNullOrEmpty(SelectedExcel))
            {
                DialogService.ShowException(new Exception("Please select an excel file to query items from excel"));
                return;
            }

            if ((QueryType)queryType == QueryType.FromDatabase && string.IsNullOrEmpty(SelectedInventory.Name))
            {
                DialogService.ShowException(new Exception("Please select an inventory type to continue"));
                return;
            }

            EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(true, "Getting Spire Items..."));
            try
            {
                if ((QueryType)queryType == QueryType.FromExcel)
                {
                    ItemsFromDb = await Task.Run(() => _repository.GetSpireItemsFromExcel(SelectedExcel).OrderBy(x => x.PartNo).ToList());
                }
                if ((QueryType)queryType == QueryType.FromDatabase)
                {
                    ItemsFromDb = await Task.Run(() => _repository.GetSpireItemsFromDatabase(SelectedInventory).OrderBy(x => x.PartNo).ToList());
                }

                ItemsFromDb.ForEach(item => TemporaryList.Add(item));
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
            ItemsFromDb.Clear();
            TemporaryList.Clear();
            InventoryListDisplayItems.Clear();
            InventoryListViewItems.Refresh();
        }
        #endregion

        #region Save Changes
        public async void SaveChanges(Func<ObservableCollection<SpireItem>, string> MakeCatalog)
        {
            bool isSaveSucceed = false;
            string pathToFileSaved = string.Empty;

            EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(true, "Exporting Spire Items To Catalog Template"));
            try
            {
                //   pathToFileSaved = await Task.Run(() => WordCatalogService?.MakeCatalog(InventoryListDisplayItems));
                pathToFileSaved = await Task.Run(() => MakeCatalog(InventoryListDisplayItems));
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

        /// <summary>
        /// Apply filtering based on FilterText which bind to UI Textbox
        /// Filter only applied when Apply button is clicked
        /// Otherwise the view is showed based on current filter acrossed different pages
        /// </summary>
        #region Filter
        private DelegateCommand _filterListCommand;
        public ICommand FilterCommand => _filterListCommand ?? (_filterListCommand =
            new DelegateCommand(FilterList));

        private void FilterList()
        {
            if (!string.Equals(CurrentFilter, FilterText))
            {
                CurrentFilter = FilterText;
            }
            RefreshCurrentView();
        }

        private string _currentFilter = string.Empty;

        public string CurrentFilter
        {
            get { return _currentFilter; }
            set
            {
                SetProperty(ref _currentFilter, value);
            }
        }

        public string FilterText { get; set; }
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

        #region Pagination
        private PaginationViewModel _pagination;

        public PaginationViewModel Pagination
        {
            get => _pagination;
            set { SetProperty(ref _pagination, value); }
        }

        /// <summary>
        /// When Pagination.CurrentPage property changes, the view needs to be updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pagination_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Pagination.CurrentPage))
            {
                //RefreshCurrentView();
            }
        }
        #endregion

        #region Refresh view
        protected void RefreshCurrentView()
        {
            var filteredList = GetFilteredWhiteListItems();
            var sortedList = GetSortedWhiteListItems(filteredList, _sortField, _sortDirection);

            InventoryListDisplayItems.Clear();
            sortedList.ForEach(item => InventoryListDisplayItems.Add(item));
            InventoryListViewItems.Refresh();
        }

        private IEnumerable<SpireItem> GetFilteredWhiteListItems()
        {
            return TemporaryList.Where(wi =>
                   {
                       switch (SelectedCategory)
                       {
                           case (CategoryType.Default):
                               return true;
                           case (CategoryType.AnimalStatue):
                               return wi.ProductCode != null && wi.ProductCode.StartsWith("FIG-AML");
                           case (CategoryType.Fountain):
                               return wi.ProductCode != null && wi.ProductCode.StartsWith("FTN");
                           case (CategoryType.LedPH):
                               return wi.ProductCode != null && (wi.ProductCode.StartsWith("LED") || wi.ProductCode.StartsWith("PH"));
                           case (CategoryType.Decor):
                               return wi.ProductCode != null &&
                                      !wi.ProductCode.StartsWith("LED") &&
                                      !wi.ProductCode.StartsWith("PH") &&
                                      !wi.ProductCode.StartsWith("FTN") &&
                                      !wi.ProductCode.StartsWith("FIG");
                           default:
                               return true;
                       }
                   }).Where(wi =>
                   {
                       if (string.IsNullOrEmpty(CurrentFilter))
                       {
                           return true;
                       }
                       return wi.ProductCode != null && wi.ProductCode.IndexOf(this.CurrentFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       wi.PartNo != null && wi.PartNo.IndexOf(this.CurrentFilter, StringComparison.OrdinalIgnoreCase) >= 0;
                   });
        }

        private List<SpireItem> GetSortedWhiteListItems(IEnumerable<SpireItem> listItems, string sortField, ListSortDirection sortDirection)
        {
            System.Reflection.PropertyInfo prop = typeof(SpireItem).GetProperty(sortField);

            return sortDirection == ListSortDirection.Descending ?
                                    listItems
                                        .OrderByDescending(i => prop.GetValue(i, null)).ToList() :
                                    listItems
                                        .OrderBy(i => prop.GetValue(i, null)).ToList();
        }
        #endregion
    }
}