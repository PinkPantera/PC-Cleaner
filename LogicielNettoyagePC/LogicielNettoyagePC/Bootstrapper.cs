using LogicielNettoyagePC.InversionOfControl;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.StartUp
{
   public static class Bootstrapper
    {
        public static void Initialize(DependencyContainer container)
        {
            container.Register<ISettingsManager, DefaultSettingManager>(Lifetime.Singleton);
            UI.Bootstrapper.Initialize (container);
        }
    }
}
