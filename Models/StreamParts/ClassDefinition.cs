using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class ClassDefinition
    {
        public string Name;
        public float Version;
        public ClassTypeDefinition[] Types;
        public ComponentDefinition[] Components;
        public ClassDefinition Params;

        public ContainerInstance Instantiate()
        {
            ContainerInstance instance = new ContainerInstance(Name, Types.Length, Components.Length);

            return instance;
        }
    }
}
