
using CatalogModule.Enums;
using CatalogModule.Models;
using CatalogModule.Repository;
using Prism.Events;
using Prism.Services.Dialogs;
using SpireHL.Core.Events;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using SpireHL.Core.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace CatalogModule.ViewModels
{
    public class SaleAnalysisCatalogViewModel : BaseCatalogViewModel
    {

        private ExcelCatalogType _selectedCatalogType;

        public ExcelCatalogType SelectedCatalogType
        {
            get { return _selectedCatalogType; }
            set
            {
                SetProperty(ref _selectedCatalogType, value);
            }
        }

        private DelegateCommand _saveExcelCommand;
        public ICommand SaveExcelCommand => _saveExcelCommand ?? (_saveExcelCommand =
            new DelegateCommand(SaveExcel));

        public SaleAnalysisCatalogViewModel(InventoryRepository catalogRepository, IDialogService dialogService, IEventAggregator eventAggregator)
            : base(catalogRepository, dialogService, eventAggregator)
        {

        }

        private void SaveExcel()
        {
            ExcelCatalogService = CatalogServiceFactory.GetExcelSaleAnalysisCatalogService();
            //base.SaveChanges(ExcelCatalogService.MakeCatalogFromObjectCollectionUsingImportData);
            base.SaveChanges(ExcelCatalogService.MakeCatalogFromObjectCollectionUsingTemplateMaker);

        }

        public async override void QuerySpireItems(object queryType)
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
                    //_itemsFromDb = await Task.Run(() => _repository.GetSpireItemsFromExcel(SelectedExcel).OrderBy(x => x.PartNo).ToList());
                }
                if ((QueryType)queryType == QueryType.FromDatabase)
                {
                    ItemsFromDb = await Task.Run(() => _repository.GetSpireItemsSaleAnalysisFromDatabase(SelectedInventory).OrderBy(x => x.PartNo).ToList());
                    //ItemsFromDb = await Task.Run(() => _repository.Test(SelectedInventory).OrderBy(x => x.PartNo).ToList());

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
    }
}