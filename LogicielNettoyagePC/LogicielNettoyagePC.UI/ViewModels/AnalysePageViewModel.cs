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
        public AnalysePageViewModel(IDirectoriesProvider directoriesProvider)
        {
            this.directoriesProvider = directoriesProvider;

            CleanCommand = new RelayCommand<EventArgs>(ExecuteCleanCommand);
            AnalyseCommand = new RelayCommand<EventArgs>(ExecuteAnalyseCommand);
            CancelCommand = new RelayCommand<EventArgs>(ExecuteCancelCommand);
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
            }
        }
        public ICommand CleanCommand { get; }
        public ICommand AnalyseCommand { get; }
        public ICommand CancelCommand { get; }

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

        #region Commands
        private async void ExecuteCleanCommand(EventArgs obj)
        {
            var directoriesToClean = Directories.Where(item => item.NeedToClean);
            if (directoriesToClean.Count() == 0)
                return;

            OperationInProgressText = ResourceFR.CleaningInProgressTxt;
            OperationInProgress = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            var tasks = new List<Task>();
            try
            {
                foreach (var dir in directoriesToClean)
                {
                    tasks.Add(CleanDirectory(dir, token));
                };

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
            }

            IsAnalysed = false;
            SpaceToClean = 0;

            OperationInProgress = false;
            OperationInProgressText = string.Empty;
        }

        private async void ExecuteAnalyseCommand(EventArgs obj)
        {
            ResetAnalise();
            OperationInProgressText = ResourceFR.AnalysisInProgressTxt;
            OperationInProgress = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var tasks = new List<Task>();

            try
            {
                foreach (var dir in Directories.Where(item => item.IsValid))
                {
                    tasks.Add(AnalyseDirectory(dir, token));
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
            }

            OperationInProgressText = string.Empty;
            IsAnalysed = true;
            OperationInProgress = false;
            SpaceToClean = Directories.Sum(t => t.DirectorySize);
        }

        private void ExecuteCancelCommand(EventArgs obj)
        {
            cancellationTokenSource?.Cancel();
        }
        #endregion Commands

        private Task AnalyseDirectory(DirectoryToDisplay dir, CancellationToken token)
        {
            return Task.Run(() =>
            {
                dir.DirectorySize = directoriesProvider.GetDirectorySize(dir.DirectoryPath);
                dir.ShowDirectorySize = (dir.DirectorySize != 0);
                dir.NeedToClean = (dir.DirectorySize != 0);

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
            });
        }

        private Task CleanDirectory(DirectoryToDisplay dir, CancellationToken token)
        {
            return Task.Run(() =>
            {
                directoriesProvider.CleanDirectory(dir.DirectoryPath);
                dir.DirectorySize = directoriesProvider.GetDirectorySize(dir.DirectoryPath);
                dir.ShowDirectorySize = false;

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
            });
        }

        private void ResetAnalise()
        {
            SpaceToClean = 0;
            foreach (var dir in Directories.Where(item => item.IsValid))
            {
                dir.ShowDirectorySize = false;
            }
        }
    }
}
