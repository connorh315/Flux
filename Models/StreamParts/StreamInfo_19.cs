using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class StreamInfo_19 : StreamInfo_1B
    {
        public override void ReadClasses(RawFile file)
        {
            uint classCount = file.ReadUInt();

            Classes = new ClassDefinition[classCount];
            for (int i = 0; i < classCount; i++)
                Classes[i] = new ClassDefinition();

            for (int i = 0; i < classCount; i++)
            {
                long startOfClass = file.Position;

                uint classDefBlockSize = file.ReadUInt();

                string classStr = file.ReadIntPascalString(false);
                if (classStr != "Class") throw new DataMisalignedException($"Expected 'Class' got '{classStr}'");

                string className = file.ReadIntPascalString(false);

                Console.WriteLine($"Class {className} ({i}):");

                uint versionBlockSize = file.ReadUInt();
                string versionStr = file.ReadIntPascalString(false);
                if (versionStr != "Version") throw new DataMisalignedException($"Expected 'Version' got '{versionStr}'");

                float version = file.ReadFloat();

                uint classTypesBlockSize = file.ReadUInt();
                string classTypesStr = file.ReadIntPascalString(false);
                if (classTypesStr != "Types") throw new DataMisalignedException($"Expected 'Types' got '{classTypesStr}'");

                uint classTypeCount = file.ReadUInt();
                ClassTypeDefinition[] classTypeDefinitions = new ClassTypeDefinition[classTypeCount];

                for (int j = 0; j < classTypeCount; j++)
                {
                    uint typeIndex = file.ReadUInt();

                    string classTypeName = file.ReadIntPascalString(false);

                    uint flag1 = file.ReadUInt();
                    uint flag2 = file.ReadUInt();
                    uint shouldUseAlternative = file.ReadUInt();

                    int countVal = file.ReadInt(); // usually -1, but appears to be 0 if it's an list ("Bone Names" in .AS files), and then two bytes need to be read before reading the type that is the number of items (I'd assume that if this is > 0, then it's a fixed size array)

                    var def = new ClassTypeDefinition
                    {
                        Name = classTypeName,
                        CountVal = countVal,
                        Index = typeIndex,
                        UseClassRef = ((shouldUseAlternative & 0xc0000000) != 0),
                        ReadAsBlock = ((shouldUseAlternative & 0x00900000) == 0x00900000),
                    };

                    if (def.UseClassRef)
                    {
                        Console.WriteLine($"\tProperty: {classTypeName} - Class: {Classes[typeIndex].Name}");
                    }
                    else
                    {
                        Console.WriteLine($"\tProperty: {classTypeName} - Type: {Types[typeIndex]}");
                    }

                    Console.WriteLine($"\t\tFlags: {flag1:X8} {flag2:X8} {shouldUseAlternative:X8} {countVal:X8}");

                    classTypeDefinitions[j] = def;
                }

                Classes[i].Name = className;
                Classes[i].Version = version;
                Classes[i].Types = classTypeDefinitions;

                while (file.Position - startOfClass < classDefBlockSize)
                {
                    uint extraPartBlockSize = file.ReadUInt();
                    string extraPartTitle = file.ReadIntPascalString(false);
                    if (extraPartTitle == "Components")
                    {
                        uint componentsCount = file.ReadUInt(); // number of components in this class

                        ComponentDefinition[] componentDefinitions = new ComponentDefinition[componentsCount];

                        for (int j = 0; j < componentsCount; j++)
                        {
                            uint componentDefinitionSize = file.ReadUInt(); // Size of the component definition block

                            string componentLabel = file.ReadIntPascalString(false); // "Component"

                            string componentName = file.ReadIntPascalString(false); // e.g. "Animation sets", "Texture"

                            uint componentClassIndex = file.ReadUInt();

                            componentDefinitions[j] = new ComponentDefinition
                            {
                                Name = componentName,
                                ClassIndex = componentClassIndex
                            };

                            Console.WriteLine($"\tComponent: {componentName} ({componentClassIndex})");
                        }

                        Classes[i].Components = componentDefinitions;
                    }
                    else if (extraPartTitle == "Params")
                    {
                        uint paramsOffset = file.ReadUInt();
                        Classes[i].Params = Classes[paramsOffset];
                        Console.WriteLine($"\tParams: {Classes[paramsOffset].Name}");
                    }
                }

                if (file.Position - startOfClass != classDefBlockSize)
                {
                    throw new DataMisalignedException("Failed at reading class definition. Refusing to go further.");
                }
            }
        }

        
    }
}
