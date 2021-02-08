using LogicielNettoyagePC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;

namespace LogicielNettoyagePC.Common
{
    public class DirectoryToDisplay : DirectoryBase, INotifyPropertyChanged
    {
        private bool showDirectorySize;
        private bool needToClean;
        private long directorySize;
        private string directoryName;
        private string directoryPath;

        public DirectoryToDisplay()
        { }

        public DirectoryToDisplay(XmlReader reader)
        {
            ReadXml(reader);
        }

        public DirectoryToDisplay(string directoryPath, string name)
        {
            DirectoryPath = directoryPath;
            DirectoryName = name;
            showDirectorySize = false;
        }

        public override string DirectoryName
        {
            get { return directoryName; }
            set
            {
                if (directoryName == value)
                    return;
                directoryName = value;
                OnPropertyChanged();
            }
        }

        public override string DirectoryPath
        {
            get { return directoryPath; }
            set
            {
                if (directoryPath == value)
                    return;
                directoryPath = value;
                OnPropertyChanged();
            }
        }


        public long DirectorySize
        {
            get { return directorySize; }
            set
            {
                if (directorySize == value)
                    return;
                directorySize = value;
                OnPropertyChanged();
            }
        }

        public bool ShowDirectorySize
        {
            get { return showDirectorySize; }
            set
            {
                if (showDirectorySize == value)
                    return;
                showDirectorySize = value;
                OnPropertyChanged();
            }
        }

        public bool NeedToClean
        {
            get { return needToClean; }
            set
            {
                if (needToClean == value)
                    return;
                needToClean = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid
        {
            get { return System.IO.Directory.Exists(DirectoryPath); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //public XmlSchema GetSchema()
        //{
        //    return null;
        //}

        //public void ReadXml(XmlReader reader)
        //{
        //    reader.ReadStartElement();
        //    DirectoryName = reader.ReadElementContentAsString(nameof(DirectoryName), "");
        //    DirectoryPath = reader.ReadElementContentAsString(nameof(DirectoryPath), "");
        //    reader.ReadEndElement();
        //}

        //public void WriteXml(XmlWriter writer)
        //{
        //    writer.WriteElementString(nameof(DirectoryName), DirectoryName);
        //    writer.WriteElementString(nameof(DirectoryPath), DirectoryPath);
        //}

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
