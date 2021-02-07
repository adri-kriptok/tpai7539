using System;
using System.Collections.Generic;
using System.Text;

namespace Fiuba.App7539.Models
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
