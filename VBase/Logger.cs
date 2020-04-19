using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VBase
{
    public enum Loglevel { INFO, WARN, ERROR }
    public static class Logger
    {
        private static string latestlogpath;
        private static string targetfolderpath;
        private static string[] threads;

        public static void init(string[] thread,string targetfolder)
        {
            latestlogpath = targetfolder + "\\latest.log";
            targetfolderpath = targetfolder;
            threads = thread;
        }

        public static void Addlogline(Loglevel level, string message,int thread=0)
        {
            using (StreamWriter writer = new StreamWriter(latestlogpath, true))
            {
                writer.WriteLine("[" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")+ "]" + ":" + "["+threads[thread]+"/" + level.ToString() + "]" + message);
            }

        }
        public static void Close()
        {
            string fname = targetfolderpath+"\\log_" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".log";
            File.Move(latestlogpath, fname);
        }
    }
}
