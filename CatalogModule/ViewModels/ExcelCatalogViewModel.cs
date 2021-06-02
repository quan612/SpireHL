
using CatalogModule.Enums;
using CatalogModule.Repository;
using Prism.Events;
using Prism.Services.Dialogs;
using SpireHL.Core.Repository;
using System.Windows.Input;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace CatalogModule.ViewModels
{
    public class ExcelCatalogViewModel : BaseCatalogViewModel
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
            new DelegateCommand(SaveExcelFile));

        public ExcelCatalogViewModel(InventoryRepository catalogRepository, IDialogService dialogService, IEventAggregator eventAggregator)
            : base(catalogRepository, dialogService, eventAggregator)
        {

        }

        private void SaveExcelFile()
        {
            ExcelCatalogService = CatalogServiceFactory.GetExcelCatalogService(SelectedCatalogType, UserSelectOptions);
            base.SaveChanges(ExcelCatalogService.MakeCatalog);
        }
    }
}