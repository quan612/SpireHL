using CatalogModule.Enums;
using CatalogModule.Repository;
using Prism.Events;
using Prism.Services.Dialogs;
using SpireHL.Core.Repository;
using System.Windows.Input;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace CatalogModule.ViewModels
{
    public class WordCatalogViewModel : BaseCatalogViewModel
    {
        private WordCatalogType _selectedCatalogType;

        public WordCatalogType SelectedCatalogType
        {
            get { return _selectedCatalogType; }
            set
            {
                SetProperty(ref _selectedCatalogType, value);
            }
        }

        private DelegateCommand _saveWordCommand;
        public ICommand SaveWordCommand => _saveWordCommand ?? (_saveWordCommand = new DelegateCommand(SaveWordCatalog));


        public WordCatalogViewModel(InventoryRepository catalogRepository, IDialogService dialogService, IEventAggregator eventAggregator)
            : base(catalogRepository, dialogService, eventAggregator)
        {
        }

        private void SaveWordCatalog()
        {
            WordCatalogService = CatalogServiceFactory.GetWordCatalogService(SelectedCatalogType, UserSelectOptions);
            base.SaveChanges(WordCatalogService.MakeCatalog);
        }
    }
}