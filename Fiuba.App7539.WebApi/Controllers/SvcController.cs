using Fiuba.App7539.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fiuba.App7539.WebApi.Controllers
{
    public class SvcController : ApiController
    {        
        [HttpPost]
        public object Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Ok(GetProcesses().Take(10).Select(p => new
                {
                    p.Id,
                    p.Name
                }).ToArray());
            }
            else
            {
                return Ok(GetProcesses().Where(p => Simplify(p.Name).Contains(Simplify(text))).Select(p => new
                {
                    p.Id,
                    p.Name
                }).ToArray());
            }
        }

        private static ProcessModel[] GetProcesses()
        {
            return (ProcessModel[])System.Web.HttpContext.Current.Application[Consts.Processes];
        }

        private string Simplify(string str)
        {
            return str.ToTrimOrEmpty().ToLower().WithoutAccents();
        }

        [HttpPost]
        public object Process(string id)
        {
            var p = GetProcesses().Single(a => a.Id.ToLower().Equals(id.ToLower()));
            return Ok(new
            {
                p.Id,
                p.Name,
                Items = p.Items.OrderBy(i => i.Order).Select(i => new
                {
                    i.Order,
                    i.Name,
                    i.Description,
                    Image = Url.Content($"~/api/Svc/Image?iid={i.Id}")
                }).ToArray(),
                Requirements = p.Requirements.Select(i => new
                {
                    i.Name,
                }).ToArray(),
            });
        }

        [HttpPost]
        public string Image(string iid)
        {
            var images = (IDictionary<string, byte[]>)System.Web.HttpContext.Current.Application[Consts.Images];
            var image = images[iid];
            return Convert.ToBase64String(image);
        }
    }
}
