using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiuba.App7539.WebApi.Models
{
    public class ItemModel
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public override string ToString()
        {
            return $"{nameof(ItemModel)} - {Order:00} {Name}";
        }
    }
}