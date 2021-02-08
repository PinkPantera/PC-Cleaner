using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public void EditDirectory(DirectoryManager directory)
        {
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

        public long GetDirectorySize(string path)
        {
            return DirectoriesToAnalyse.Where(item => item.DirectoryPath == path)
                .FirstOrDefault()
                .GetDirectorySize();
        }

        public void SaveHistory(Verification verification)
        {
            Verifications = settingsManager.UpdateFileHistory(verification);
        }

        private bool ExportListDirectoriesToXml()
        {
            return settingsManager.UpdateFileSettings(DirectoriesToAnalyse);
        }
    }
}
