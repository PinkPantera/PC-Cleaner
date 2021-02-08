using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LogicielNettoyagePC.Common
{
    public class Verification : IXmlSerializable, INotifyPropertyChanged
    {
        public Verification()
        {

        }

        public Verification(DateTime verificationDate, List<DirectoryToDisplay> directories)
        {
            VerificationDate = verificationDate;

            Directories = directories; 
        }

        public DateTime VerificationDate { get; private set; }

        public List<DirectoryToDisplay> Directories { get; private set; } = new List<DirectoryToDisplay>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.ReadStartElement();
                var tmp = reader.ReadElementContentAsString(nameof(VerificationDate), "");
                VerificationDate = Convert.ToDateTime(tmp);
                reader.ReadStartElement();
                while (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "directory")
                        Directories.Add(new DirectoryToDisplay(reader));
                }

                reader.ReadEndElement();
                reader.ReadEndElement();
            }
            catch (Exception ex)
            {
                
            }
        }


    //    reader.ReadStartElement();
    //        var tmp = reader.ReadElementContentAsString(nameof(VerificationDate), "");
    //    VerificationDate = Convert.ToDateTime(tmp);
    //        var tt = reader.Name;
    //        if (reader.Name == nameof(Directories))
    //        {
    //            reader.ReadStartElement();
    //            if (reader.NodeType != XmlNodeType.EndElement)
    //            {
    //                while (reader.NodeType == XmlNodeType.Element)
    //                {
    //                    if (reader.Name == "directory")
    //                        Directories.Add(new DirectoryToDisplay(reader));
    //                }
    //reader.ReadEndElement();
    //            }
    //        }
    //       reader.ReadEndElement(); //end Verification
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(VerificationDate), VerificationDate.ToString());
            writer.WriteStartElement(nameof(Directories));
            foreach (var directory in Directories)
            {
                writer.WriteStartElement(nameof(directory));
                directory.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

    }
}
