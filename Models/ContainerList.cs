using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models
{
    public class ContainerList : FieldValue
    {
        public ContainerInstance[] Containers;

        public ContainerList(int containerCount) : base("")
        {
            Containers = new ContainerInstance[containerCount];
        }

        private int containerOffset = 0;
        public void AddContainer(ContainerInstance container)
        {
            Containers[containerOffset++] = container;
        }
    }
}
