using EComModule.ViewModels;
using EComModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using SpireHL.Core.Repository;

namespace EComModule
{
    public class EComMainModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public EComMainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<CommercialHubProcess, CommercialHubProcessViewModel>();

            containerRegistry.RegisterForNavigation<CommercialHubProcess>();
            //containerRegistry.RegisterForNavigation<ExcelCatalog>();
            //containerRegistry.RegisterForNavigation<CatalogSettings>();
            //containerRegistry.RegisterDialog<SummaryDialog, SummaryDialogViewModel>();
            //containerRegistry.RegisterDialog<UserSelectOptionsDialog, UserSelectOptionsViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            ModuleConfigs.CheckAndCreateTable();
        }
    }
}
