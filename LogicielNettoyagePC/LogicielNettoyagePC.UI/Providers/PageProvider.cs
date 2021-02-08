using LogicielNettoyagePC.InversionOfControl;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
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

        //public IEnumerable<IPage> GetPages()
        //{
        //    return new IPage[]
        //    {
        //        IoC.Container.Resolve<IPage>("StudentSpace"),
        //        IoC.Container.Resolve<IPage>("TeacherSpace"),
        //        IoC.Container.Resolve<IPage>("AdministratorSpace"),
        //    };
        //}
    }
}
