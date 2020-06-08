#System.Web.JsonConfiguration
.net framework for net45 添加json配置文件，节点读取和文件变化监听 

## 特性
1. 支持读取json文件配置，动态监听变化修改，配置获取
2. 支持json内常规类型，如int,string,float,以及class
3. 扩展webapi中HttpConfigurationExtensions,添加AddJsonFile

## 使用
1. 使用前，先在webapi中Register注册AddJsonFile
2. 或者在Global.ashx中Application_Start启动JsonConfigurationManager.SetWatcher方法
3. 然后，读取json文件中节点使用JsonConfigurationManager提供Get,GetInt,GetFloat,GetSection方法

##示例
### json文件
```javascript 
{
  "one": "1",
  "two": "2.1",
  "three": "x1",
  "four": {
    "minSize": "1",
    "maxSize": "10"
  }
}
```
### WebApi中WebApiConfig配置Register注册
```s
public static void Register(HttpConfiguration config)
{
   // Web API 配置和服务
   //添加json配置文件
   config.AddJsonFile("settings.json", AppDomain.CurrentDomain.BaseDirectory);

   // Web API 路由
   config.MapHttpAttributeRoutes();

   config.Routes.MapHttpRoute(
     name: "DefaultApi",
     routeTemplate: "api/{controller}/{id}",
     defaults: new { id = RouteParameter.Optional }
   );
}
```

### json文件节点输出
```s 
public IHttpActionResult GetUser()
{
    var one = JsonConfigurationManager.GetInt("one");
    var two = JsonConfigurationManager.GetFloat("two");
    var three = JsonConfigurationManager.Get("three");
    var four = JsonConfigurationManager.GetSection<IDictionary<string, object>>("four");
    return Ok(new { one = one, two = two, three = three, four = four });
}
```