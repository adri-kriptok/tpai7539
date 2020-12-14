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
    }
}
