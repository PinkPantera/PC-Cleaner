using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.UI.Interfaces
{
    public interface ISettingsManager
    {
        List<DirectoryManager> ListProcessedDirectories { get; }
        List<Verification> ListHistories { get; } 

        bool UpdateFileSettings(List<DirectoryManager> directories);
        List<Verification> UpdateFileHistory(Verification verification);
        //List<Verification> LoadVerificationsFromXml();
        event EventHandler<HistoryChangedEventArgs> HistoryChanged;
    } 
}
