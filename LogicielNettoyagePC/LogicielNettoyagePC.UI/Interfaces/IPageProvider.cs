using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.UI.Interfaces
{
    public interface IPageProvider
    {
        IPage GetPage(PageKind pageKind);
        List<IPage> GetAllOpenedPages();
    }
}
