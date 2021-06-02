using CatalogModule.ViewModels;
using CatalogModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using SpireHL.Core.Repository;

namespace CatalogModule
{
    public class CatalogMainModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public CatalogMainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<WordCatalog, WordCatalogViewModel>();
            ViewModelLocationProvider.Register<ExcelCatalog, ExcelCatalogViewModel>();
            ViewModelLocationProvider.Register<SaleOrderCatalog, SaleOrderCatalogViewModel>();
            ViewModelLocationProvider.Register<SaleAnalysisCatalog, SaleAnalysisCatalogViewModel>();

            containerRegistry.RegisterForNavigation<WordCatalog>();
            containerRegistry.RegisterForNavigation<ExcelCatalog>();
            containerRegistry.RegisterForNavigation<SaleOrderCatalog>();
            containerRegistry.RegisterForNavigation<SaleAnalysisCatalog>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE5NjM2QDMxMzgyZTM0MmUzMG96SkJiY1VhK2p4UlQ5YlJ5cVhrUFJvUGx2RjVFY01UTG5CYXM3czFLa0E9");
            ModuleConfigs.CheckAndCreateTable();
        }
    }
}
