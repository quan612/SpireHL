using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using ShopzioModule.ViewModels;
using ShopzioModule.Views;
using SpireHL.Core.Repository;

namespace ShopzioModule
{
    public class ShopzioMainModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public ShopzioMainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<CreateShopzioOrder, CreateShopzioOrderViewModel>();

            containerRegistry.RegisterForNavigation<CreateShopzioOrder>();
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
