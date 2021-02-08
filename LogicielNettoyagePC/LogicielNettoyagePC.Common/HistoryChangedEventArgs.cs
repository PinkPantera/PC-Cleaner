using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.Common
{
    public class HistoryChangedEventArgs : EventArgs
    {
        public readonly DateTime LastVerification;

        public HistoryChangedEventArgs(DateTime lastVerification)
        {
            LastVerification = lastVerification;
        }
    }
}
