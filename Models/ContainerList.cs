namespace Flux.Models
{
    public class ContainerList(int containerCount) : FieldValue("")
    {
        public ContainerInstance[] Containers = new ContainerInstance[containerCount];

        private int _containerOffset = 0;

        public void AddContainer(ContainerInstance container)
        {
            Containers[_containerOffset++] = container;
        }
    }
}
