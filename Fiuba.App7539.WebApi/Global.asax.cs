using Fiuba.App7539.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Fiuba.App7539.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            var data = JsonConvert.DeserializeObject<ProcessModel[]>(
                File.ReadAllText(Server.MapPath("~/data.json")));
            Application[Consts.Processes] = data;
            Application[Consts.Images] = data.SelectMany(p => p.Items).ToDictionary(p => p.Id, p => p.Image);
        }
    }
}
