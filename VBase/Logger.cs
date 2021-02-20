using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VBase
{
    public enum Loglevel {DUMP = 0 ,DEBUG = 1, INFO = 2, WARN = 3, ERROR = 4 }
    public static class Logger
    {
        private static string latestlogpath;
        private static string targetfolderpath;
        private static string[] threads;
        private static Loglevel MinLvl;
        private static StreamWriter LogFile;
        public static void init(string[] thread, string targetfolder, Loglevel MinLevel = Loglevel.INFO)
        {
            latestlogpath = targetfolder + "\\latest.log";
            targetfolderpath = targetfolder;
            threads = thread;
            if (!File.Exists(latestlogpath))
            {
                File.Create(latestlogpath).Close();
            }
            LogFile = File.AppendText(latestlogpath);
        }

        public static void Addlogline(Loglevel level, string message,int thread=0)
        {
            if ((int)level >= (int)MinLvl)
                return;
            LogFile.WriteLine($"[{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}]:[{threads[thread]}/{level}] {message}");
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}]:[{threads[thread]}/{level}] {message}");

        }
        public static void Close()
        {
            LogFile.Close();
            LogFile.Dispose();
            string fname = targetfolderpath+"\\log_" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".log";
            File.Move(latestlogpath, fname);
        }
    }
}
