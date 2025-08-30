using Flux.Models;
using Flux.Models.StreamContainers;
using Flux.ViewModels.Values;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels
{
    public class StreamViewModel
    {
        public string FilePath { get; set; }

        public ObservableCollection<ClassListViewModel> Items { get; set; } = new();

        private static ClassViewModel ConvertObject(ContainerInstance obj)
        {
            ClassViewModel model = new ClassViewModel(obj);

            for (int i = 0; i < obj.Fields.Length; i++)
            {
                var field = obj.Fields[i];

                if (field is PrimitiveField pf)
                {
                    if (pf.Value != null)
                    {
                        if (pf.FieldType == FieldType.String)
                        {
                            model.Properties.Add(new StringValueViewModel(pf));
                        }
                    }
                }
            }

            return model;
        }

        private static ClassListViewModel ConvertObjectList(ContainerList objectList)
        {
            ClassListViewModel list = new ClassListViewModel(objectList);

            foreach (var item in objectList.Containers)
            {
                list.Instances.Add(ConvertObject(item));
            }

            return list;
        }

        public static StreamViewModel Parse(string fileLocation)
        {
            var container = StreamContainer.Parse(fileLocation);

            var stream = new StreamViewModel();

            foreach (var list in container.Stream.Objects)
            {
                stream.Items.Add(ConvertObjectList(list));

                //foreach (var obj in list.Containers)
                //{
                //    foreach (var field in obj.Fields)
                //    {
                //        if (field is PrimitiveField pf)
                //        {
                //            Console.WriteLine(pf.Value);
                //        }
                //    }
                //}
            }

            return stream;
        }
    }
}
