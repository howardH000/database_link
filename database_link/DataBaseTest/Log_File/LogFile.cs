using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Log_File
{
    public class LogFile
    {
        public LogFile(string logfilepath)
        {
            LogFilePath = logfilepath;

            search(LogFilePath);
        }
        public string LogFilePath;
        void search(string filepath)
        {

            if (filepath.LastIndexOf(@"/") >= 0)
            {
                string directory = filepath.Remove(filepath.LastIndexOf(@"/"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            else
            {
                throw new ArgumentNullException("請指定正確的檔案路徑");
            }
        }
           public void WriteLog(string logfilepath,string WriteString)
           {
               LogFilePath=logfilepath;

               search(LogFilePath);
               using (StreamWriter sw = new StreamWriter(LogFilePath, true))
               {
                   sw.WriteLine(WriteString);


               }
           }
           public void WriteLog( string WriteString)
           {            
               search(LogFilePath);
               using (StreamWriter sw = new StreamWriter(LogFilePath, true))
               {
                   sw.WriteLine(WriteString);


               }
           }
        
    }
}
