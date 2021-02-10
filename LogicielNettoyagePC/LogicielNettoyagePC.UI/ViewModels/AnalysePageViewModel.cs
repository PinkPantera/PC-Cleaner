using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class AnalysePageViewModel : ViewModelBase, IPage
    {
        private bool selectAll;
        private IDirectoriesProvider directoriesProvider;
        private bool isAnalysed;
        private long spaceToClean;
        private string operationInProgressText;
        private bool operationInProgress;

        public AnalysePageViewModel(IDirectoriesProvider directoriesProvider)
        {
            this.directoriesProvider = directoriesProvider;

            CleanCommand = new RelayCommand<EventArgs>(ExecuteCleanCommand);
            AnalyseCommand = new RelayCommand<EventArgs>(ExecuteAnalyseCommand);
            OperationInProgressText = "Test";
            OperationInProgress = false;
        }

        public PageKind PageKind => PageKind.Analyse;
        public string Caption => ResourceFR.Analyzer;

        public string OperationInProgressText
        {
            get { return operationInProgressText; }
            set
            {
                SetProperty(ref operationInProgressText, value);
            }
        }

        public bool IsAnalysed
        {
            get { return isAnalysed; }
            set
            {
                SetProperty(ref isAnalysed, value);
                if (value == false)
                    SelectAll = false;
            }
        }
        public ICommand CleanCommand { get; }
        public ICommand AnalyseCommand { get; }

        public long SpaceToClean
        {
            get { return spaceToClean; }
            set { SetProperty(ref spaceToClean, value); }
        }
        public bool OperationInProgress
        {
            get { return operationInProgress; }
            set { SetProperty(ref operationInProgress, value); }
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

        private async void ExecuteCleanCommand(EventArgs obj)
        {
            OperationInProgressText = ResourceFR.CleaningInProgressTxt;
            OperationInProgress = true;

            await ExecuteCleanAsync();

            IsAnalysed = false;
            SpaceToClean = 0;
            OperationInProgress = false;
            OperationInProgressText = string.Empty;
        }

        private async void ExecuteAnalyseCommand(EventArgs obj)
        {
            OperationInProgressText = ResourceFR.AnalysisInProgressTxt;
            OperationInProgress = true;

            await ExecuteAnalyseAsync();

            OperationInProgressText = string.Empty;
            IsAnalysed = true;
            OperationInProgress = false;
            SpaceToClean = Directories.Sum(t => t.DirectorySize);
        }

        private Task ExecuteAnalyseAsync()
        {
            return Task.Run(() =>
            {
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
            }
        );
        }

        private Task ExecuteCleanAsync()
        {
            return Task.Run(() =>
            {
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

            } );
        }
    }
}
