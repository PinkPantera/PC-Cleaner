using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogicielNettoyagePC.UI.Common
{
    public class DirectoryManager : LogicielNettoyagePC.Common.DirectoryBase
    {
        public DirectoryManager()
        {

        }

        public DirectoryManager(string path, string directoryFrendlyName)
        {
            DirectoryName = directoryFrendlyName;
            DirectoryPath = path;
        }

        public override string DirectoryPath { get ; set ; }
        public override string DirectoryName { get; set ; }

        public void CleanSpace()
        {
            var directory = new DirectoryInfo(DirectoryPath);

            foreach (var file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    //
                    //TODO
                    //write in log file
                }
            }

            foreach (var dir in directory.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch (Exception ex)
                {
                    //
                    //TODO
                    //write in log file
                }
            }
        }

        public long GetDirectorySize()
        {
            var directory = new DirectoryInfo(DirectoryPath);
            return CalculateDirectorySize(directory);
        }
      
        private long CalculateDirectorySize(DirectoryInfo dir)
        {
            return dir.GetFiles()
                .Sum(f => f.Length) + dir.GetDirectories().Sum(d => CalculateDirectorySize(d));
        }

    }
}
