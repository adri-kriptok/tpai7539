using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiuba.App7539.WebApi.Models
{
    public class ProcessModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ItemModel[] Items { get; set; }
        public RequirementModel[] Requirements { get; set; }

        public override string ToString()
        {
            return $"{nameof(ProcessModel)} {Name}";
        }
    }
}