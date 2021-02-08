using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class AnalysePageViewModel : ViewModelBase, IPage
    {
        // private readonly Dictionary<string, DirectoryManager> directoriesToAnalyse = new Dictionary<string, DirectoryManager>();
        private bool selectAll;
        private IDirectoriesProvider directoriesProvider;
        private bool isAnalysed;
        private long spaceToClean;
        private string dateOfLastAnalises = ResourceFR.NeverTxt;
        private string operationInProgress;

        public AnalysePageViewModel(IDirectoriesProvider directoriesProvider)
        {
            this.directoriesProvider = directoriesProvider;

            CleanCommand = new RelayCommand<EventArgs>(ExecuteCleanCommand);
            AnalyseCommand = new RelayCommand<EventArgs>(ExecuteAnalyseCommand);
            OperationInProgress = string.Empty;
        }

        public PageKind PageKind => PageKind.Analyse;
        public string Caption => ResourceFR.Analyzer;

        public string OperationInProgress
        {
            get { return operationInProgress; }
            set 
            { 
                SetProperty(ref operationInProgress, value); 
            }
        }

        public bool IsAnalysed
        {
            get { return isAnalysed; }
            set
            {
                SetProperty(ref isAnalysed, value);
            }
        }
        public ICommand CleanCommand { get; }
        public ICommand AnalyseCommand { get; }

        public long SpaceToClean
        {
            get { return spaceToClean; }
            set { SetProperty(ref spaceToClean, value); }
        }
        public string DateOfLastAnalises
        {
            get { return dateOfLastAnalises; }
            set { SetProperty(ref dateOfLastAnalises, value); }
        }
        public ObservableCollection<DirectoryToDisplay> Directories { get; } = new ObservableCollection<DirectoryToDisplay>();
        public bool SelectAll
        {
            get { return selectAll; }
            set
            {
                if (SetProperty(ref selectAll, value))
                {
                    foreach (var item in Directories.Where(item => item.IsValid && item.DirectorySize > 0))
                    {
                        item.NeedToClean = value;
                    }
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Directories));
                }
            }
        }

        public bool IsEnabled
        {
            get; set;
        }

        public void Refreshe()
        {
            Directories.Clear();
            foreach (var dir in directoriesProvider.DirectoriesToAnalyse)
            {
                Directories.Add(new DirectoryToDisplay(dir.DirectoryPath, dir.DirectoryName));
            };
        }

        private void ExecuteCleanCommand(EventArgs obj)
        {
            OperationInProgress = ResourceFR.CleaningInProgressTxt;
            var directoriesToClean = Directories.Where(item => item.NeedToClean);

            if (directoriesToClean.Count() > 0)
            {
                foreach (var dir in directoriesToClean)
                {
                    directoriesProvider.CleanDirectory(dir.DirectoryPath);
                    dir.DirectorySize = directoriesProvider.GetDirectorySize(dir.DirectoryPath);
                    dir.ShowDirectorySize = (dir.DirectorySize != 0);
                }

                directoriesProvider.SaveHistory(new Verification(DateTime.Now, directoriesToClean.ToList()));
            }

            SelectAll = false;
            IsAnalysed = false;
            SpaceToClean = 0;
            OperationInProgress = string.Empty;
        }

        private void ExecuteAnalyseCommand(EventArgs obj)
        {

            long totalSize = 0;
            OperationInProgress = ResourceFR.AnalysisInProgressTxt;
            //Thread.Sleep(10000);
            foreach (var dir in Directories.Where(item => item.IsValid))
            {
                try
                {
                    dir.DirectorySize = directoriesProvider.GetDirectorySize(dir.DirectoryPath);
                    dir.ShowDirectorySize = (dir.DirectorySize != 0);
                }
                catch (Exception ex)
                {
                    //TODO
                    //add log
                }
            }

            totalSize = Directories.Sum(t => t.DirectorySize);
            SpaceToClean = totalSize;
            IsAnalysed = true;
            DateOfLastAnalises = DateTime.Now.ToString();
            OperationInProgress = string.Empty;
        }

    }
}
