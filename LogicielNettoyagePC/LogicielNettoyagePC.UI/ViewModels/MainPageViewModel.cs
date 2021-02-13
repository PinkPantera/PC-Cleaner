using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IPage
    {
        private IDirectoriesProvider directoriesProvider;

        public MainPageViewModel(IDirectoriesProvider directoriesProvider)
        {
            this.directoriesProvider = directoriesProvider;
            CanBeClosed = true;
            foreach (var dir in directoriesProvider.DirectoriesToAnalyse)
            {
                Directories.Add(new DirectoryToDisplay(dir.DirectoryPath, dir.DirectoryName));
            };
        }

        public ObservableCollection<DirectoryToDisplay> Directories { get; } = new ObservableCollection<DirectoryToDisplay>();
        public PageKind PageKind => PageKind.Main;
        public string Caption => ResourceFR.HomePageTitle;
        public bool IsEnabled
        {
            get; set;
        }

        public bool CanBeClosed { get; private set; }

        public void Refreshe()
        {
            Directories.Clear();
            foreach (var dir in directoriesProvider.DirectoriesToAnalyse)
            {
                Directories.Add(new DirectoryToDisplay(dir.DirectoryPath, dir.DirectoryName));
            };
        }
    }
}
