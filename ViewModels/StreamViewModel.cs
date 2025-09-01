using Flux.Models;
using Flux.Models.StreamContainers;
using Flux.Models.StreamContainers.StreamInfo.Definitions;
using Flux.ViewModels.Values;
using System;
using System.Collections.ObjectModel;

namespace Flux.ViewModels
{
    public class StreamViewModel
    {
        public string FilePath { get; set; }

        public ObservableCollection<ClassListViewModel> Items { get; set; } = [];

        private static ClassViewModel ConvertObject(ContainerInstance obj)
        {
            ClassViewModel model = new(obj);

            for (int i = 0; i < obj.Fields.Length; i++)
            {
                var field = obj.Fields[i];

                if (field is PrimitiveField pf)
                {
                    if (pf.Value != null)
                    {
                        if (pf.FieldType == FieldType.Char)
                        {
                            model.Properties.Add(new CharValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Short)
                        {
                            model.Properties.Add(new ShortValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Int)
                        {
                            model.Properties.Add(new IntValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Int64)
                        {
                            model.Properties.Add(new LongValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Float)
                        {
                            model.Properties.Add(new FloatValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.String)
                        {
                            model.Properties.Add(new StringValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Colour3)
                        {
                            model.Properties.Add(new Colour3ValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Colour4)
                        {
                            model.Properties.Add(new Colour4ValueViewModel(pf));
                        }
                        // TODO: NuHSpecial.
                        else if (pf.FieldType == FieldType.Vec3)
                        {
                            model.Properties.Add(new Vec3ValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Vec4)
                        {
                            model.Properties.Add(new Vec4ValueViewModel(pf));
                        }
                        else if (pf.FieldType == FieldType.Mtx)
                        {
                            model.Properties.Add(new MtxValueViewModel(pf));
                        }
                        // TODO: Ptr.
                        // TODO: ClassObject?
                        // TODO: ClassObjectRef?
                        // TODO: Half.
                        // TODO: Data.
                        // TODO: HalfVec3.
                        else if (pf.FieldType == FieldType.UChar)
                        {
                            model.Properties.Add(new UCharValueViewModel(pf));
                        }
                        else
                        {
                            Program.Logger.Debug($"{pf.FieldType} editor not implemented!");
                        }
                    }
                }
                else if (field is ContainerInstance inst)
                {
                    model.Properties.Add(ConvertObject(inst));
                }
            }

            for (int i = 0; i < obj.Components.Length; i++)
            {
                var component = obj.Components[i];
                model.Components.Add(ConvertObjectList(component));
            }

            return model;
        }

        private static ClassListViewModel ConvertObjectList(ContainerList objectList)
        {
            ClassListViewModel list = new(objectList);

            foreach (var item in objectList.Containers)
            {
                list.Instances.Add(ConvertObject(item));
            }

            return list;
        }

        public static StreamViewModel Deserialize(string filePath)
        {
            StreamInfoContainer container = StreamInfoContainer.Deserialize(filePath);
            StreamViewModel     stream    = new();

            foreach (ContainerList list in container.StreamInfoBase.Objects)
            {
                stream.Items.Add(ConvertObjectList(list));
            }

            StreamInfoContainer.Dispose();

            return stream;
        }
    }
}