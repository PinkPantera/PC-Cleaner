using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace LogicielNettoyagePC.StartUp
{
    public class DefaultSettingManager : ISettingsManager
    {

        private readonly static List<DirectoryManager> defaultFoldersToAnalise = new List<DirectoryManager>
        {
            new DirectoryManager( @"C:\Windows\Temp" , "Fichier temporaires Windows" ),
            new DirectoryManager ( System.IO.Path.GetTempPath() , "Fichier temporaires des applications")
        };

        private readonly string fileSettingsName;
        private readonly string fileHistoryName;

        public DefaultSettingManager()
        {
            string exeDirectory = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetEntryAssembly().Location);

            fileSettingsName = Path.Combine(exeDirectory, SettingsApp.Default.FileSettings);
            fileHistoryName = Path.Combine(exeDirectory, SettingsApp.Default.FileHistory);

            if (!LoadListFoldersFromSettings())
            {
                UpdateFileSettings(defaultFoldersToAnalise);
            }

            LoadVerificationsFromXml();
        }

        public event EventHandler<HistoryChangedEventArgs> HistoryChanged;
        public List<DirectoryManager> ListProcessedDirectories { get; private set; } = new List<DirectoryManager>();
        public List<Verification> ListHistories { get; private set; } = new List<Verification>();

        public bool UpdateFileSettings(List<DirectoryManager> directories)
        {
            var result = false;
            var file = new FileXml<DirectoryManager>(fileSettingsName);
            try
            {
                file.Save(directories);
                result = true;
            }
            catch
            {
                //
                //TODO
                //write in log file
            }

            return result;
        }

        public List<Verification> UpdateFileHistory(Verification verification)
        {
            var file = new FileXml<Verification>(fileHistoryName);

            ListHistories.Add(verification);
            ListHistories = ListHistories.OrderByDescending(item => item.VerificationDate).Take(10).ToList();

            OnHistoryChanged(new HistoryChangedEventArgs(ListHistories.FirstOrDefault().VerificationDate));
            try
            {
                file.Save(ListHistories);
            }
            catch (Exception ex)
            {
                //
                //TODO
                //write in log file
            }

            return ListHistories;
        }

        protected virtual void OnHistoryChanged(HistoryChangedEventArgs e)
        {
            HistoryChanged?.Invoke(this, e);
        }

        private void LoadVerificationsFromXml()
        {
            var file = new FileXml<Verification>(fileHistoryName);

            try
            {
                ListHistories.AddRange(file.LoadList()
                    .Where(item=>item.Directories.Count>0)
                    .OrderByDescending(item => item.VerificationDate)
                    .Take(10)
                    .ToList());

                var lastVerif = DateTime.MinValue;
                if (ListHistories.FirstOrDefault() != null)
                {
                    lastVerif = ListHistories.FirstOrDefault().VerificationDate;
                }

                OnHistoryChanged(new HistoryChangedEventArgs(ListHistories.FirstOrDefault().VerificationDate));
            }
            catch (Exception ex)
            {
                //
                //TODO
                //write in log file
            }
        }

        private bool LoadListFoldersFromSettings()
        {
            var file = new FileXml<DirectoryManager>(fileSettingsName);

            try
            {
                var tmp = file.LoadList();
                ListProcessedDirectories = file.LoadList().ToList();
            }
            catch (Exception ex)
            {
                //
                //TODO
                //write in log file
            }

            return ListProcessedDirectories != null && ListProcessedDirectories.Count > 0;
        }

    }
}
