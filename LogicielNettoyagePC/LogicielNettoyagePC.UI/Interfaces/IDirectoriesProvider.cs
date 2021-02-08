using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogicielNettoyagePC.UI.Interfaces
{
    public interface IDirectoriesProvider
    {
        List<DirectoryManager>  DirectoriesToAnalyse { get; }
        List<Verification> Verifications { get; }
        IList<string> CleanDirectory(string path);
        long GetDirectorySize(string path);
        bool AddDirectory(string directoryPath, string directoryName);
        void DeleteDirectory(string directoryPath, string directoryName);
        void EditDirectory(DirectoryManager directory);
        void SaveHistory(Verification verification);
    }
}
