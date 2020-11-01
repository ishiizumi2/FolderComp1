using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiLeListLibrary
{
    public class FileList
    {
        /// <summary>
        /// フルパスから最後のフォルダ名を取得
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLastFolderName(string path)
        {
            string lastFolder = null;
            try
            {
                string[] folders = path.Split('\\');
                if (folders.Length > 0)
                    lastFolder = folders[folders.Length - 1];
                else
                    lastFolder = null;
            }
            catch (Exception)
            {
                throw;
            }
            return lastFolder;
        }
    }
}
