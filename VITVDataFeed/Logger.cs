using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITVDataFeed
{
    public class Logger
    {
        public static void Write(string logMessage)
        {
            string strLogMessage = string.Empty;
            string strLogFile = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            StreamWriter swLog;

            strLogMessage = string.Format("{0}: {1}", DateTime.Now, logMessage);

            if (!File.Exists(strLogFile))
            {
                swLog = new StreamWriter(strLogFile);
            }
            else
            {
                swLog = File.AppendText(strLogFile);
            }

            swLog.WriteLine(strLogMessage);
            swLog.WriteLine();

            swLog.Close();
        }
    }
}
