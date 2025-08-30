using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class ClassTypeDefinition
    {
        public string Name;

        public uint Index;

        public bool UseClassRef;

        public bool ReadAsBlock;

        public int CountVal;
    }
}
