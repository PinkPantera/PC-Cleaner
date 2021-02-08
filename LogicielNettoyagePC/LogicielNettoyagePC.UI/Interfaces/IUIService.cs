using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LogicielNettoyagePC.UI.Interfaces
{
    public interface IUIService
    {
        Window GetAppWindow();
        void Initialize();
        void ShowError(Exception e);
    }
}
