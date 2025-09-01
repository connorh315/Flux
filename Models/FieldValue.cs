namespace Flux.Models
{
    public abstract class FieldValue(string name)
    {
        public string Name { get; set; } = name;
    }
}