using System.IO;

namespace System.Web.JsonConfiguration
{
    /// <summary>
    /// 文件监听
    /// </summary>
    class DefaultJsonConfigurationMonitor : IJsonConfigurationMonitor
    {
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="json"></param>
        public void Start(JsonConfigurationPath json)
        {
            var watcher = new FileSystemWatcher
            {
                IncludeSubdirectories = false,
                Path = json.Path,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.Size,
                Filter = json.Name
            };
            watcher.Changed += OnFileChanged;
            watcher.EnableRaisingEvents = true;
        }
        /// <summary>
        /// 文件改变
        /// </summary>
        /// <param name="render"></param>
        /// <param name="e"></param>
        private void OnFileChanged(object render, FileSystemEventArgs e)
        {
            if (!FileHelper.IsFileLocked(e.FullPath))
            {
                if (render is FileSystemWatcher watcher)
                {
                    watcher.EnableRaisingEvents = false;
                    if (e.ChangeType == WatcherChangeTypes.Changed)
                    {
                        JsonConfigurationProvider.Set(e.Name, File.ReadAllText(e.FullPath));
                    }

                    watcher.EnableRaisingEvents = true;
                }
            }
        }

    }

}
