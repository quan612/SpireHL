using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using SpireHL.Core;
using SpireHL.Core.Events;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using SpireHL.Core.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SpireHL.ViewModels
{
    class OptionsViewModel : ViewModelBase
    {
        private Dictionary<string, string> _listOfChanges = new Dictionary<string, string>();

        private string _catalogExportPath;
        [Required(ErrorMessage = "CatalogExportPath is required!")]
        public string CatalogExportPath
        {
            get { return _catalogExportPath; }
            set
            {
                SetProperty(ref _catalogExportPath, value);
                RaisePropertyChanged(nameof(CanExecuteSaveCommand));
            }
        }

        private string _lowesCanadaExportPath;
        [Required(ErrorMessage = "Lowes Canada Export Path is required!")]
        public string LowesCanadaExportPath
        {
            get { return _lowesCanadaExportPath; }
            set
            {
                SetProperty(ref _lowesCanadaExportPath, value);
                RaisePropertyChanged(nameof(CanExecuteSaveCommand));
            }
        }

        private string _lowesCanadaImportPath;
        [Required(ErrorMessage = "Lowes Canada Import Path is required!")]
        public string LowesCanadaImportPath
        {
            get { return _lowesCanadaImportPath; }
            set
            {
                SetProperty(ref _lowesCanadaImportPath, value);
                RaisePropertyChanged(nameof(CanExecuteSaveCommand));
            }
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand).ObservesCanExecute(() => CanExecuteSaveCommand));

        private DelegateCommand _setCatalogPathCommand;
        public DelegateCommand SetCatalogPathCommand => 
            _setCatalogPathCommand ?? (_setCatalogPathCommand = new DelegateCommand(ExecuteSetCatalogPathCommand));

        private DelegateCommand<string> _setLowesCanadaPathCommand;
        public DelegateCommand<string> SetLowesCanadaPathCommand => 
            _setLowesCanadaPathCommand ?? (_setLowesCanadaPathCommand = new DelegateCommand<string>(ExecuteSetLowesCanadaPath));

        private void ExecuteSetCatalogPathCommand()
        {
            CatalogExportPath = DialogService.ShowSetPathDialog(ModuleConstants.ProjectDirectory ?? "");
        }

        private void ExecuteSetLowesCanadaPath(string parameter)
        {
            if (parameter == nameof(LowesCanadaExportPath))
            {
                LowesCanadaExportPath = DialogService.ShowSetPathDialog(ModuleConstants.ProjectDirectory ?? "");
            } 
            else
            {
                LowesCanadaImportPath = DialogService.ShowSetPathDialog(ModuleConstants.ProjectDirectory ?? "");
            }
        }

        public bool CanExecuteSaveCommand => !HasErrors;

        private async void ExecuteSaveCommand()
        {
            EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(true, "Saving Setting..."));
            try
            {
                _listOfChanges[nameof(CatalogExportPath)] = CatalogExportPath;
                _listOfChanges[nameof(LowesCanadaExportPath)] = LowesCanadaExportPath;
                _listOfChanges[nameof(LowesCanadaImportPath)] = LowesCanadaImportPath;

                await Task.Run(() => SaveSettings(_listOfChanges));
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

        public OptionsViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
           
        }
       
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var moduleConfigs = ModuleConfigs.GetConfigs("Inventory", "Catalog");

                CatalogExportPath = moduleConfigs.Find(cf => cf.ParameterName == nameof(CatalogExportPath)).ParameterValue ?? "";
                LowesCanadaExportPath = moduleConfigs.Find(cf => cf.ParameterName == nameof(LowesCanadaExportPath)).ParameterValue ?? "";
                LowesCanadaImportPath = moduleConfigs.Find(cf => cf.ParameterName == nameof(LowesCanadaImportPath)).ParameterValue ?? "";
            }
            catch (Exception ex)
            {
                DialogService.ShowException(ex);
            }
        }

        private void SaveSettings(Dictionary<string, string> listOfChanges)
        {
            try
            {
                foreach (var kvp in listOfChanges)
                {
                    ModuleConfigs.UpdateConfig("Inventory", "Catalog", kvp.Key, kvp.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
