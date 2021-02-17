using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiuba.App7539.WebApi.Models
{
    public class RequirementModel
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{nameof(RequirementModel)} {Name}";
        }
    }
}