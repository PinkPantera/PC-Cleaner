using LogicielNettoyagePC.InversionOfControl;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Windows;

namespace LogicielNettoyagePC.UI
{
    internal class UIService : IUIService
    {
        private AppWindowViewModel appWindowViewModel;


        public Window GetAppWindow()
        {
            return new AppWindow
            {

                DataContext = appWindowViewModel
            };
        }

        public void Initialize()
        {
            appWindowViewModel = IoC.Container.Resolve<AppWindowViewModel>();
        }

        public void ShowError(Exception e)
        {
            
        }
    }
}
