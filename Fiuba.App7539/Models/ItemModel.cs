namespace Fiuba.App7539.Models
{
    public class ItemModel
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public override string ToString()
        {
            return $"{nameof(ItemModel)} - {Order:00} {Name}";
        }
    }
}
