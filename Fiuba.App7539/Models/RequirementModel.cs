namespace Fiuba.App7539.Models
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
