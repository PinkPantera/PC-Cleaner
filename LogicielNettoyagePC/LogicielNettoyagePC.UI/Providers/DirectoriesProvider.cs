using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogicielNettoyagePC.UI.Providers
{
    public class DirectoriesProvider : IDirectoriesProvider
    {
        private readonly ISettingsManager settingsManager;

        public DirectoriesProvider(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            DirectoriesToAnalyse = settingsManager.ListProcessedDirectories;
            Verifications = settingsManager.ListHistories;
        }

        public event EventHandler<DirectoriesToAnalyseChangedEventArgs> OnDirectoriesToAnalyseChanged;

        public List<DirectoryManager> DirectoriesToAnalyse { get; private set; }
        public List<Verification> Verifications { get; private set; }

        public bool AddDirectory(string directoryPath, string directoryName)
        {
            var result = false;

            if (Directory.Exists(directoryPath))
            {
                var directory = new DirectoryManager(directoryPath, directoryName);
                DirectoriesToAnalyse.Add(directory);
                if (ExportListDirectoriesToXml())
                {
                    result = true;
                }
            }

            return result;
        }

        public void DeleteDirectory(string directoryPath, string directoryName)
        {
            var directory = DirectoriesToAnalyse.Find(item => item.DirectoryName == directoryName && item.DirectoryPath == directoryPath);
            DirectoriesToAnalyse.Remove(directory);
            ExportListDirectoriesToXml();
        }

        public IList<string> CleanDirectory(string path)
        {
            var result = new List<string>();

            var directory = DirectoriesToAnalyse.Where(item => item.DirectoryPath == path).FirstOrDefault();
            directory.CleanSpace();
            result.Add(path);
            return result;
        }

        public Task CleanDirectoryAsync(string path, CancellationToken token, IProgress<(string dirPath, long dirSize)> p)
        {
            var tcs = new TaskCompletionSource<long>();

            Task.Run(() =>
            {
                try
                {
                    //TODO need to remove
                    var rng = new Random();
                    Thread.Sleep(1000 + rng.Next(2000, 8000));
                    // need to remove
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    var directory = DirectoriesToAnalyse.Where(item => item.DirectoryPath == path).FirstOrDefault();
                    directory.CleanSpace();

                    var dirSize = GetDirectorySize(path);

                    p?.Report((path, dirSize));
                    tcs.SetResult(dirSize);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

           return tcs.Task;
        }

        public long GetDirectorySize(string path)
        {
            return DirectoriesToAnalyse.Where(item => item.DirectoryPath == path)
                .FirstOrDefault()
                .GetDirectorySize();
        }

        public Task GetDirectorySizeAsync(string path, CancellationToken token, IProgress<(string dirPath, long dirSize)> p)
        {
            var tcs = new TaskCompletionSource<long>();
            Task.Run(() =>
            {
                try
                {
                    //TODO need to remove
                    var rng = new Random();
                    Thread.Sleep(1000 + rng.Next(2000, 8000));
                    // need to remove

                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    var result = DirectoriesToAnalyse.Where(item => item.DirectoryPath == path)
                                                     .FirstOrDefault()
                                                     .GetDirectorySize();
                    p?.Report((path, result));
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
                );

            return tcs.Task;
        }

        public Task SaveHistoryAsync(Verification verification)
        {
           return  Task.Run(()=>
            {
                Thread.Sleep(7000);
                Verifications = settingsManager.UpdateFileHistory(verification);
            });
        }

        protected virtual void DirectoriesToAnalyseChanged(DirectoriesToAnalyseChangedEventArgs e)
        {
            OnDirectoriesToAnalyseChanged?.Invoke(this, e);
        }

        private bool ExportListDirectoriesToXml()
        {
            DirectoriesToAnalyseChanged(new DirectoriesToAnalyseChangedEventArgs());
            return settingsManager.UpdateFileSettings(DirectoriesToAnalyse);
        }
    }
}
