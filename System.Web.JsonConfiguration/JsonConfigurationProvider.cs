using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Web.JsonConfiguration
{
    static class JsonConfigurationProvider
    {
        private static readonly ConcurrentDictionary<string, string> JsonDictionary = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 添加设置文件内容
        /// </summary>
        /// <param name="settingsKey">设置文件名</param>
        /// <param name="value">设置文件内容</param>
        /// <returns></returns>
        public static bool Set(string settingsKey, string value)
        {
            if (!JsonDictionary.ContainsKey(settingsKey))
            {
                return JsonDictionary.TryAdd(settingsKey, value);
            }

            if (JsonDictionary.TryGetValue(settingsKey, out var v))
            {
                return JsonDictionary.TryUpdate(settingsKey, value, v);
            }
            return false;
        }
        /// <summary>
        ///  获取设置文件区域内容
        /// </summary>
        /// <param name="settingsKey">设置文件名</param>
        /// <param name="key">区域key</param>
        /// <returns>eg:{username:"test"}</returns>
        public static string GetSection(string settingsKey, string key)
        {
            var list = GetSections(settingsKey, key);
            return list == null ? string.Empty : list.FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingsKey">设置文件名</param>
        /// <param name="key">配置项</param>
        /// <returns>eg:{arr:[1,2,3]}</returns>
        public static IEnumerable<string> GetSections(string settingsKey, string key)
        {
            var jObject = GetJsonDictionary(settingsKey);
            if (jObject != null)
            {
                var jo = jObject[key];
                if (!(jo is JToken jtoken))
                {
                    IList<string> list = new List<string>(1);
                    list.Add(jo.ToString());
                    return list;
                }

                if (jtoken.Type == JTokenType.Array && jtoken.HasValues)
                {
                    if (jtoken.First is JToken first && first.Type != JTokenType.Object)
                    {
                        return jtoken.Values<string>();
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///  获取某区域配置的单项对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsKey"></param>
        /// <param name="section"></param>
        /// <returns>eg:{redis:[{hosts:[],ssl:false}]}中redis其中第一个对象</returns>
        public static T GetSection<T>(string settingsKey, string section) where T : class
        {
            return GetSections<T>(settingsKey, section).FirstOrDefault();
        }

        /// <summary>
        ///  获取膜区域配置对象下层对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsKey"></param>
        /// <param name="section"></param>
        /// <returns>eg:{redis:[{hosts:[],ssl:false}]}中redis其中第一个对象</returns>
        public static T GetSectionNext<T>(string settingsKey, string section, string nextKey) where T : class
        {
            var jObject = GetJsonDictionary(settingsKey);
            if (jObject != null)
            {
                var jo = jObject[section];
                if (jo is JToken jtoken)
                {
                    if (jtoken.Type == JTokenType.Object && jtoken.HasValues)
                    {
                        var dictionaryNext = JsonConvert.DeserializeObject<IDictionary<string, object>>(jtoken.ToString());
                        if (dictionaryNext.ContainsKey(nextKey))
                        {
                            var jnext = dictionaryNext[nextKey];
                            if (jnext is JToken jtokenNext)
                            {
                                if (jtokenNext.Type == JTokenType.Object && jtokenNext.HasValues)
                                {
                                    return JsonConvert.DeserializeObject<T>(jtokenNext.ToString());
                                }
                            }
                        }
                    }
                }
            }
            return default(T);
        }
        /// <summary>
        /// 获取某区域配置的对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsKey">设置文件名</param>
        /// <param name="section">设置文件区域名称</param>
        /// <returns>eg:{redis:[{hosts:[],ssl:false}]}</returns>
        public static IEnumerable<T> GetSections<T>(string settingsKey, string section) where T : class
        {
            var jObject = GetJsonDictionary(settingsKey);
            if (jObject != null)
            {
                var jo = jObject[section];
                if (jo is JToken jtoken)
                {
                    if (jtoken.Type == JTokenType.Array && jtoken.HasValues)
                    {
                        jtoken = jtoken.First;
                        if (jtoken != null && jtoken.Type == JTokenType.Object)
                        {
                            return JsonConvert.DeserializeObject<IEnumerable<T>>(jo.ToString());
                        }
                    }else if (jtoken.Type == JTokenType.Object && jtoken.HasValues)
                    {
                        var list = new List<T>(1);
                        var single = JsonConvert.DeserializeObject<T>(jo.ToString());
                        list.Add(single);
                        return list;
                    }
                }
            }
            return default(IEnumerable<T>);
        }

        /// <summary>
        /// 获取设置文件内容对应的字典集合
        /// </summary>
        /// <param name="settingsKey">设置文件名</param>
        /// <returns></returns>
        private static IDictionary<string, object> GetJsonDictionary(string settingsKey)
        {
            JsonDictionary.TryGetValue(settingsKey, out var o);
            if (o != null)
            {
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(o);
            }
            return null;
        }
    }

}
