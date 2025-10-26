using Flux.Models;
using Flux.Models.StreamContainers;
using Flux.Models.StreamParts;
using Flux.ViewModels.Values;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels
{
    public class StreamViewModel
    {
        public string FilePath { get; set; }

        public StreamContainer StreamContainer { get; set; }

        public ObservableCollection<ClassListViewModel> Items { get; set; } = new();

        private static ClassViewModel ConvertObjectToViewModel(MemberObject obj)
        {
            ClassViewModel vm = new ClassViewModel(obj);

            if (obj.Params != null)
            {
                vm.Params = ConvertObjectToViewModel(obj.Params);
            }

            for (int i = 0; i < obj.Fields.Length; i++)
            {
                var field = obj.Fields[i];

                if (field is PrimitiveField pf)
                {
                    
                    if (pf.FieldType == FieldType.String)
                    {
                        vm.Properties.Add(new StringValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Char)
                    {
                        vm.Properties.Add(new CharValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.UChar)
                    {
                        vm.Properties.Add(new UCharValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Short)
                    {
                        vm.Properties.Add(new ShortValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Half)
                    {
                        vm.Properties.Add(new HalfValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Float)
                    {
                        vm.Properties.Add(new FloatValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Int)
                    {
                        vm.Properties.Add(new IntValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Int64)
                    {
                        vm.Properties.Add(new LongValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Colour3)
                    {
                        vm.Properties.Add(new Colour3ValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Vec3)
                    {
                        vm.Properties.Add(new Vec3ValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.HalfVec3)
                    {
                        vm.Properties.Add(new HalfVec3ValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Vec4)
                    {
                        vm.Properties.Add(new Vec4ValueViewModel(pf));
                    }
                    else if (pf.FieldType == FieldType.Mtx)
                    {
                        vm.Properties.Add(new MtxValueViewModel(pf));
                    }
                    else
                    {
                        Console.WriteLine($"{pf.FieldType} editor not implemented!");
                    }
                }
                else if (field is MemberObject inst)
                {
                    vm.Properties.Add(ConvertObjectToViewModel(inst));
                }
                else if (field is BlankClassObject blank)
                {
                    vm.Properties.Add(new BlankClassObjectViewModel(blank));
                }
            }

            for (int i = 0; i < obj.Components.Length; i++)
            {
                var component = obj.Components[i];
                vm.Components.Add(ConvertObjectListToViewModel(component));
            }

            return vm;
        }

        

        public ClassViewModel AddMemberToList(ClassListViewModel list)
        {
            if (list == null) return null;
            var thisClass = list.Model.Definition;
            MemberObject instance = StreamContainer.Stream.Instantiate(thisClass);
            
            var newInstance = ConvertObjectToViewModel(instance);
            newInstance.Parent = list;
            list.Instances.Add(newInstance);
            return newInstance;
        }

        private static ClassListViewModel ConvertObjectListToViewModel(ObjectList objectList)
        {
            ClassListViewModel list = new ClassListViewModel(objectList);

            foreach (var item in objectList.Containers)
            {
                var instance = ConvertObjectToViewModel(item);
                instance.Parent = list;
                list.Instances.Add(instance);
            }

            return list;
        }

        public static StreamViewModel Parse(string fileLocation)
        {
            var container = StreamContainer.Parse(fileLocation);

            var stream = new StreamViewModel();

            foreach (var list in container.Stream.Objects)
            {
                stream.Items.Add(ConvertObjectListToViewModel(list));
            }

            stream.StreamContainer = container;

            stream.FilePath = fileLocation;

            return stream;
        }

        private static MemberObject ConvertViewModelToObject(ClassViewModel classViewModel)
        {
            MemberObject obj = (MemberObject)classViewModel.Model;

            if (classViewModel.Params != null)
            {
                obj.Params = ConvertViewModelToObject(classViewModel.Params);
            }

            for (int i = 0; i < classViewModel.Properties.Count; i++)
            {
                var prop = classViewModel.Properties[i];
                if (prop is ClassViewModel cvm)
                {
                    obj.Fields[i] = ConvertViewModelToObject(cvm);
                }
                else if (prop is BlankClassObjectViewModel vm)
                {
                    obj.Fields[i] = vm.Model;
                }
                else
                {
                    obj.Fields[i] = prop.ConvertToPrimitiveModel();
                }
            }

            for (int i = 0; i < classViewModel.Components.Count; i++)
            {
                obj.Components[i] = ConvertViewModelToObjectList(classViewModel.Components[i]);
            }
            return obj;
        }

        private static ObjectList ConvertViewModelToObjectList(ClassListViewModel listViewModel)
        {
            ObjectList list = new ObjectList(listViewModel.Instances.Count);
            list.Definition = listViewModel.Model.Definition;
            list.Containers = new MemberObject[listViewModel.Instances.Count];
            for (int i = 0; i < listViewModel.Instances.Count; i++)
            {
                list.Containers[i] = ConvertViewModelToObject(listViewModel.Instances[i]);
            }
            return list;
        }   

        public static void Save(string fileLocation, StreamViewModel viewModel)
        {
            viewModel.StreamContainer.Stream.Objects = new ObjectList[viewModel.Items.Count];
            for (int i = 0; i < viewModel.Items.Count; i++)
            {
                viewModel.StreamContainer.Stream.Objects[i] = ConvertViewModelToObjectList(viewModel.Items[i]);
            }

            using (RawFile file = RawFile.Create(fileLocation))
            {
                viewModel.StreamContainer.Write(file);
            }
        }
    }
}
