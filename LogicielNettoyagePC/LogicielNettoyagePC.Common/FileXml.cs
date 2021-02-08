using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace LogicielNettoyagePC.Common
{
    public class FileXml<T>
    {
        private string fileName;

        public FileXml(string fileName)
        {
            this.fileName = fileName;
        }

        public void Save(List<T> dirManager)
        {
            XmlSerializer xs = new XmlSerializer(dirManager.GetType());
            using (Stream s = File.Create(fileName))
            {
                xs.Serialize(s, dirManager);
            }
        }

        public IList<T> LoadList()
        {
            var result = new List<T>();
            XmlSerializer xs = new XmlSerializer(result.GetType());

            using (Stream s = File.OpenRead(fileName))
            {
                result = (List<T>)xs.Deserialize(s);
            }

            return result;
        }

    }
}
