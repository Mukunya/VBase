using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VBase
{
    public class FileBrowser
    {				
        private string _lastPath;
        private string _extension;
        private string _hint;

        public enum EFileOperations { Open, Save }

        public FileBrowser(string Extension, string Hint)
        {
            _lastPath = "";
            _extension = Extension;
            _hint = Hint;
        }

        public string GetFileName(EFileOperations FileOperation)
        {
            switch (FileOperation)
            {
                default:
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


                    dlg.DefaultExt = _extension;

                    dlg.Filter = _hint + "|*" + _extension;

                    dlg.CheckFileExists = true;

                    if (_lastPath.Length > 2)
                        dlg.InitialDirectory = _lastPath;


                    Nullable<bool> result = dlg.ShowDialog();

                    string fileName = null;

                    if (result == true)
                    {
                        fileName = dlg.FileName;
                        _lastPath = Path.GetDirectoryName(fileName);
                    }

                    return fileName;
                case EFileOperations.Save:
                    Microsoft.Win32.SaveFileDialog dlg1 = new Microsoft.Win32.SaveFileDialog();

                    dlg1.DefaultExt = _extension;

                    dlg1.Filter = _hint + "|*" + _extension;

                    dlg1.CheckFileExists = false;

                    if (_lastPath.Length > 2)
                        dlg1.InitialDirectory = _lastPath;

                    Nullable<bool> result1 = dlg1.ShowDialog();

                    string fname = null;

                    if (result1==null)
                    {
                        fname = dlg1.FileName;
                        _lastPath = Path.GetDirectoryName(fname);
                    }

                    return fname;
            }
            
        }
    }
}