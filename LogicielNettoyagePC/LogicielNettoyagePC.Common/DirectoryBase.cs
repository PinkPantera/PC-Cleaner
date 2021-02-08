using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LogicielNettoyagePC.Common
{
    public abstract class  DirectoryBase: IXmlSerializable
    {
        public DirectoryBase ()
        {

        }

        public abstract string DirectoryPath { get; set; }

        public abstract string DirectoryName { get; set; }

        #region IXmlSerializable
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            DirectoryName = reader.ReadElementContentAsString(nameof(DirectoryName), "");
            DirectoryPath = reader.ReadElementContentAsString(nameof(DirectoryPath), "");
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(DirectoryName), DirectoryName);
            writer.WriteElementString(nameof(DirectoryPath), DirectoryPath);
        }
        #endregion#region IXmlSerializable
    }
}
