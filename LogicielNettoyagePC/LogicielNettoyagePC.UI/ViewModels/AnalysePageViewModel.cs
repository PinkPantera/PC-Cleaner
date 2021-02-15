using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class AnalysePageViewModel : ViewModelBase, IPage
    {
        private IDirectoriesProvider directoriesProvider;
        private bool isAnalysed;
        private long spaceToClean;
        private string operationInProgressText;
        private bool operationInProgress;
        private CancellationTokenSource cancellationTokenSource = null;
        private string information;
        private bool canShowInformation;

        public AnalysePageViewModel(IDirectoriesProvider directoriesProvider)
        {
            this.directoriesProvider = directoriesProvider;
            this.directoriesProvider.OnDirectoriesToAnalyseChanged += DirectoriesProvider_OnDirectoriesToAnalyseChanged;

            CleanCommand = new RelayCommand<EventArgs>(ExecuteCleanCommand);
            AnalyseCommand = new RelayCommand<EventArgs>(ExecuteAnalyseCommand);
            CancelCommand = new RelayCommand<EventArgs>(ExecuteCancelCommand);
            RefreshListCommand = new RelayCommand<EventArgs>(ExecuteRefreshListCommand);
            OperationInProgressText = "Test";
            OperationInProgress = false;
            CanBeClosed = true;
            CanShowInformation = false;
            ReloadDirectoriesList();
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
            }
        }
        public ICommand CleanCommand { get; }
        public ICommand AnalyseCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RefreshListCommand { get; }

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
        public string Information
        {
            get { return information; }
            set { SetProperty(ref information, value); }
        }

        public bool CanShowInformation
        {
            get { return canShowInformation; }
            set { SetProperty(ref canShowInformation, value); }
        }

        public ObservableCollection<DirectoryToDisplay> Directories { get; } = new ObservableCollection<DirectoryToDisplay>();

        public bool IsEnabled
        {
            get; set;
        }

        public bool CanBeClosed { get; private set; }

        public void Refreshe()
        {
        }

        #region Commands
        private async void ExecuteCleanCommand(EventArgs obj)
        {
            var directoriesToClean = Directories.Where(item => item.NeedToClean).ToList();
            if (directoriesToClean.Count() == 0)
                return;

            OperationInProgressText = ResourceFR.CleaningInProgressTxt;
            OperationInProgress = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var p = GetProgress();

            CanBeClosed = false;

            var tasks = new List<Task>();
            try
            {
                foreach (var dir in directoriesToClean)
                {
                    var task = directoriesProvider.CleanDirectoryAsync(dir.DirectoryPath, token, p);
                    tasks.Add(task);
                };

                await Task.WhenAll(tasks).ContinueWith((res) =>
                {
                    OperationInProgressText = ResourceFR.UpdateHistoryFileInProgressTxt;
                    directoriesProvider.SaveHistoryAsync(new Verification(DateTime.Now, directoriesToClean.ToList())).ContinueWith((_) =>
                    {
                        SpaceToClean = 0;
                        IsAnalysed = false;
                        OperationInProgress = false;
                        OperationInProgressText = string.Empty;
                        CanBeClosed = true;
                    });
                });
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {
                //TODO add log
            }
            finally
            {
                cancellationTokenSource.Dispose();

                // directoriesProvider.SaveHistoryAsync(new Verification(DateTime.Now, directoriesToClean.ToList()));

            }
        }

        private async void ExecuteAnalyseCommand(EventArgs obj)
        {
            ResetAnalise();
            OperationInProgressText = ResourceFR.AnalysisInProgressTxt;
            OperationInProgress = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var tasks = new List<Task>();

            CanBeClosed = false;
            var p = GetProgress();

            try
            {
                foreach (var dir in Directories.Where(item => item.IsValid))
                {
                    var task = directoriesProvider.GetDirectorySizeAsync(dir.DirectoryPath, token, p);
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {
                //TODO add log
            }
            finally
            {
                cancellationTokenSource.Dispose();
                OperationInProgressText = string.Empty;
                IsAnalysed = true;
                OperationInProgress = false;
                SpaceToClean = Directories.Sum(t => t.DirectorySize);
                CanBeClosed = true;
            }
        }

        private void ExecuteCancelCommand(EventArgs obj)
        {
            cancellationTokenSource?.Cancel();
            OperationInProgressText = ResourceFR.CancelationInProgressTxt; ;
        }

        private void ExecuteRefreshListCommand(EventArgs obj)
        {
            if (CanBeClosed)
            {
                ReloadDirectoriesList();
                SpaceToClean = 0;
                IsAnalysed = false;
            }
        }
        #endregion Commands

        private Progress<(string dirPath, long dirSize)> GetProgress()
        {
            var p = new Progress<(string dirPath, long dirSize)>();

            p.ProgressChanged += (_, result) =>
            {
                Directories.Where(item => item.DirectoryPath == result.dirPath)
                .ForEach(s =>
                {
                    s.DirectorySize = result.dirSize;
                    s.ShowDirectorySize = (result.dirSize != 0);
                    s.NeedToClean = (result.dirSize != 0);
                });
            };
            return p;
        }

        private void ResetAnalise()
        {
            SpaceToClean = 0;
            foreach (var dir in Directories.Where(item => item.IsValid))
            {
                dir.ShowDirectorySize = false;
            }
        }
        private void ReloadDirectoriesList()
        {
            Directories.Clear();
            foreach (var dir in directoriesProvider.DirectoriesToAnalyse)
            {
                Directories.Add(new DirectoryToDisplay(dir.DirectoryPath, dir.DirectoryName));
            };

            CanShowInformation = false;
        }


        private void DirectoriesProvider_OnDirectoriesToAnalyseChanged(object sender, DirectoriesToAnalyseChangedEventArgs e)
        {
            Information = ResourceFR.UpdateDirectoriesListTxt;
            CanShowInformation = true;
        }
    }
}
