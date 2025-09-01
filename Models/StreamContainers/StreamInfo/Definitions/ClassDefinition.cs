namespace Flux.Models.StreamContainers.StreamInfo.Definitions
{
    public class ClassDefinition
    {
        public string                Name;
        public float                 Version;
        public ClassTypeDefinition[] Types;
        public ComponentDefinition[] Components;
        public ClassDefinition       Params;
    }
}