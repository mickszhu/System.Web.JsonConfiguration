
using System.Web.Http;
using System.Web.JsonConfiguration;
using System.Collections.Generic;

namespace ApiDemo.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        [Route("")]
        public IHttpActionResult GetUser()
        {
            var one = JsonConfigurationManager.GetInt("one");
            var two = JsonConfigurationManager.GetFloat("two");
            var three = JsonConfigurationManager.Get("three");
            var four = JsonConfigurationManager.GetSection<IDictionary<string, object>>("four");
            var five = JsonConfigurationManager.GetSectionNext<IDictionary<string, object>>("four");
            return Ok(new { one = one, two = two, three = three, four = four });
        }
    }
}
