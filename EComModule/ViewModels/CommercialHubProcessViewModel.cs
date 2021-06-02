using EComModule.Repository;
using EComModule.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using SpireHL.Core;
using SpireHL.Core.Events;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using SpireHL.Core.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace EComModule.ViewModels
{
    public class CommercialHubProcessViewModel : ViewModelBase
    {
        private readonly List<SpireConfigs> _moduleConfigs;
        private EComRepository _repository;
        private EComService _service;
        private string _lowesCanadaImportPath;
        private string _lowesCanadaExportPath;

        private string _selectedSource;
        public string SelectedSource
        {
            get { return _selectedSource; }
            set { SetProperty(ref _selectedSource, value); }
        }
        public CommercialHubProcessViewModel(EComService service, EComRepository repository, IDialogService dialogService, IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
            _repository = repository;
            _service = service;
            _moduleConfigs = ModuleConfigs.GetConfigs("Inventory", "Catalog");

            _lowesCanadaImportPath = _moduleConfigs.Find(cf => cf.ParameterName == "LowesCanadaImportPath").ParameterValue ?? "C:\\";
            _lowesCanadaExportPath = _moduleConfigs.Find(cf => cf.ParameterName == "LowesCanadaExportPath").ParameterValue ?? "C:\\";

        }

        #region Open File
        private DelegateCommand _openLowesFileCommand;
        public ICommand OpenLowesFileCommand => _openLowesFileCommand ?? (_openLowesFileCommand =
           new DelegateCommand(OpenLowesFile));

        private void OpenLowesFile()
        {
             SelectedSource = DialogService.ShowOpenFileDialog(_lowesCanadaImportPath, "", ".neworders");
        }
        #endregion

        #region Make File
        private DelegateCommand<object> _makeFileCommand;
        public ICommand MakeFileCommand => _makeFileCommand ?? (_makeFileCommand =
           new DelegateCommand<object>(MakeFile));

        private async void MakeFile(object queryType)
        {
            if (string.IsNullOrEmpty(SelectedSource))
            {
                DialogService.ShowException(new Exception("Please select the downloaded order file to proceed"));
                return;
            }

            bool isSaveSucceed = false;

            EventAggregator.GetEvent<IsBusyEvent>().Publish(new BusyEventPayLoad(true, "Making Order File..."));
            try
            { 
                var lowesList = await Task.Run(() => _service.ReadLowesCsvOrderFile(SelectedSource));
                var lowestListWithWeight = await Task.Run(() => _repository.GetItemsWeight(lowesList));
                var lowesShippingOrder = _service.BuildLowesMVKFTemplate(lowestListWithWeight);

                _service.MakeLowesShippingMVKF(lowesShippingOrder, SelectedSource);
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
                        { "PathToFile", _lowesCanadaExportPath}
                    };
                    DialogService.ShowSummary(dialogParameters);
                }
            }
        }
        #endregion 
    }
}