using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class HistoryPageViewModel : IPage
    {
        private IDirectoriesProvider directoriesProvider;

        public HistoryPageViewModel(IDirectoriesProvider directoriesProvider)
        {
            this.directoriesProvider = directoriesProvider;
            CanBeClosed = true;
        }

        public PageKind PageKind => PageKind.History;

        public string Caption => ResourceFR.HistoryPageCaptionTxt;

        public bool IsEnabled { get; set ; }

        public ObservableCollection<Verification> Verifications { get; } = new ObservableCollection<Verification>();

        public bool CanBeClosed { get; private set; }

        public void Refreshe()
        {
            Verifications.Clear();
            foreach (var item in directoriesProvider.Verifications)
            {
                Verifications.Add(item);
            }
        }
    }
}
