using Fiuba.App7539.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fiuba.App7539.Helpers
{
    public static class JsonHelper
    {
        public static T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
