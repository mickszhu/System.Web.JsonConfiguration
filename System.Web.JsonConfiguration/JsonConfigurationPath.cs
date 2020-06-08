

namespace System.Web.JsonConfiguration
{
    /// <summary>
    ///  json配置文件路径
    /// </summary>
    internal class JsonConfigurationPath
    {
        public string Path { get; }

        public string Name { get; }

        public JsonConfigurationPath(string path, string name)
        {
            Path = path;
            Name = name;
        }
    }
}
