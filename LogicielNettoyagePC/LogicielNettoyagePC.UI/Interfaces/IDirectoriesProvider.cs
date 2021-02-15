using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogicielNettoyagePC.UI.Interfaces
{
    public interface IDirectoriesProvider
    {
        event EventHandler<DirectoriesToAnalyseChangedEventArgs> OnDirectoriesToAnalyseChanged;
        List<DirectoryManager>  DirectoriesToAnalyse { get; }
        List<Verification> Verifications { get; }
        IList<string> CleanDirectory(string path);
        long GetDirectorySize(string path);
        Task CleanDirectoryAsync(string path, CancellationToken token, IProgress<(string dirPath, long dirSize)> p);
        Task GetDirectorySizeAsync(string path, CancellationToken token, IProgress<(string dirPath, long dirSize)> p);
        bool AddDirectory(string directoryPath, string directoryName);
        void DeleteDirectory(string directoryPath, string directoryName);
        Task SaveHistoryAsync(Verification verification);
    }
}
