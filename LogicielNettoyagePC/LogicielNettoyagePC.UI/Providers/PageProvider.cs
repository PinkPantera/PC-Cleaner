﻿using LogicielNettoyagePC.InversionOfControl;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicielNettoyagePC.UI.Providers
{
    public class PageProvider : IPageProvider
    {
        private static readonly Dictionary<PageKind, IPage> pages = new Dictionary<PageKind, IPage>();
        public PageProvider()
        {
        }

        public IPage GetPage(PageKind pageKind)
        {
            pages.TryGetValue(pageKind, out IPage page);
            if (page == null)
            {
                page = IoC.Container.Resolve<IPage>(pageKind.ToString());
                if (page != null)
                    pages.Add(page.PageKind, page);
            }

            return page;
        }

        public List<IPage> GetAllOpenedPages()
        {
            return pages.Values.ToList<IPage>(); ;
        }
    }
}
