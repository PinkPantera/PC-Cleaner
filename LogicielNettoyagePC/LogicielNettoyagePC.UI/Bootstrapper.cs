using LogicielNettoyagePC.InversionOfControl;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using LogicielNettoyagePC.UI.Providers;
using LogicielNettoyagePC.UI.ViewModels;

namespace LogicielNettoyagePC.UI
{
    public static class Bootstrapper
    {
        public static void Initialize (DependencyContainer container)
        {
            container.Register<IUIService, UIService>(Lifetime.Singleton);

            container.Register<IDirectoriesProvider, DirectoriesProvider>(Lifetime.Singleton);

            //register pages 
            container.Register<IPage, MainPageViewModel>(PageKind.Main.ToString(), Lifetime.Singleton);
            container.Register<IPage, AnalysePageViewModel>(PageKind.Analyse.ToString(), Lifetime.Singleton);
            container.Register<IPage, SettingPageVeiwModel>(PageKind.Settings.ToString(), Lifetime.Singleton);
            container.Register<IPage, HistoryPageViewModel>(PageKind.History.ToString(), Lifetime.Singleton);
            container.Register<IPage, UpdatePageViewModel>(PageKind.Update.ToString(), Lifetime.Singleton);
            container.Register<IPage, NavigatorPageViewModel>(PageKind.Navigator.ToString(), Lifetime.Singleton);

            container.Register<IPageProvider, PageProvider>(Lifetime.Singleton);
        }
    }
}
