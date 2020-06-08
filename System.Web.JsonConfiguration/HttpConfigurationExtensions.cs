
using System.Web.Http;

namespace System.Web.JsonConfiguration
{
    public static partial class HttpConfigurationExtensions
    {
        public static void AddJsonFile(this HttpConfiguration conf, string jsonFileName, string path)
        {
            //初始化将配置内容添加到管理中并开启对配置文件的watcher
            JsonConfigurationManager.SetWatcher(jsonFileName, path);
        }
    }
}
