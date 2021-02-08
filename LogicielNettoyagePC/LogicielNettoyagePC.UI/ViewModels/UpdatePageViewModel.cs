using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class UpdatePageViewModel : IPage
    {
        public PageKind PageKind => PageKind.Update;

        public string Caption => ResourceFR.UpdateBtnTxt;

        public bool IsEnabled { get; set ; }

        public void Refreshe()
        {
           
        }
    }
}
