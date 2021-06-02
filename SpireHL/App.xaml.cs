using CatalogModule;
using CatalogModule.Dialogs;
using DryIoc;
using EComModule;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using ShopzioModule;
using SpireHL.Base.Dialogs;
using SpireHL.Dialogs;
using SpireHL.ViewModels;
using SpireHL.Views;
using System.Windows;

namespace SpireHL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<Options, OptionsViewModel>();

            containerRegistry.RegisterDialog<ExceptionDialog, ExceptionDialogViewModel>();
            containerRegistry.RegisterDialog<Confirmation, ConfirmationViewModel>();
            containerRegistry.RegisterDialog<SummaryDialog, SummaryDialogViewModel>();
            containerRegistry.RegisterDialog<UserOptionsCatalogDialog, UserOptionsCatalogDialogViewModel>();

            containerRegistry.RegisterForNavigation<Options>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var newCat = new ModuleCatalog();
            var catalogModule = new ModuleInfo(
                typeof(CatalogMainModule),
                "CatalogModule",
                InitializationMode.WhenAvailable);

            var shopzioModule = new ModuleInfo(
                typeof(ShopzioMainModule),
                "ShopzioModule",
                InitializationMode.WhenAvailable);

            var eComModule = new ModuleInfo(
                typeof(EComMainModule),
                "EComMainModule",
                InitializationMode.WhenAvailable);

            newCat.AddModule(catalogModule);
            newCat.AddModule(shopzioModule);
            newCat.AddModule(eComModule);
            return newCat;
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<HullWindow>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
