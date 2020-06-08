
using System.IO;

namespace System.Web.JsonConfiguration
{
    internal class FileHelper
    {
        /// <summary>
        /// 文件是否锁住
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string filePath)
        {
            FileStream stream = null;
            try
            {
                stream = File.OpenRead(filePath);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }
            return false;
        }
    }
}
