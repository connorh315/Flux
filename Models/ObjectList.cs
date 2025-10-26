using Flux.Models;
using Flux.Models.StreamParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models
{
    public class ObjectList
    {
        public MemberObject[] Containers;

        public ClassDefinition Definition { get; set; }

        public ObjectList(int containerCount)
        {
            Containers = new MemberObject[containerCount];
        }

        private int containerOffset = 0;
        public void AddContainer(MemberObject container)
        {
            Containers[containerOffset++] = container;
        }
    }
}
