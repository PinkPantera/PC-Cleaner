using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.UI.Interfaces
{
    public interface IPage
    {
        PageKind PageKind { get; }
        string Caption { get; }
        bool IsEnabled { get; set; }
        void Refreshe();
        bool CanBeClosed { get; }
    }
}
