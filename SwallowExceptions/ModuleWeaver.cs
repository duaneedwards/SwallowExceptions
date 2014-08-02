using System;
using System.Linq;
using Mono.Cecil;
using SwallowExceptions.Fody;

public class ModuleWeaver
{
    public Action<string> LogInfo { get; set; }

    public ModuleDefinition ModuleDefinition { get; set; }
    public IAssemblyResolver AssemblyResolver { get; set; }
    TypeSystem typeSystem;

    public TypeReference ExceptionType;
    public ArrayType ObjectArray;

    public ModuleWeaver()
    {
        LogInfo = m => { };
    }

    public void Execute()
    {
//        typeSystem = ModuleDefinition.TypeSystem;
//
//        var ifDef = new TypeDefinition("MarkerInterfaces", "IDTOMarkerInterface", TypeAttributes.Interface | TypeAttributes.Public | TypeAttributes.Abstract);
//
//        ModuleDefinition.Types.Add(ifDef);
//
//        var dtos = ModuleDefinition.GetTypes().Where(x => x.Name.EndsWith("DTO"));
//
//        foreach (var dto in dtos)
//        {
//            dto.Interfaces.Add(ifDef);
//        }
        ObjectArray = new ArrayType(ModuleDefinition.TypeSystem.Object);
        
        var msCoreLibDefinition = AssemblyResolver.Resolve("mscorlib");
        ExceptionType = ModuleDefinition.Import(msCoreLibDefinition.MainModule.Types.First(x => x.Name == "Exception"));
        foreach (var type in ModuleDefinition
            .GetTypes()
            .Where(x => (x.BaseType != null) && !x.IsEnum && !x.IsInterface))
        {
            ProcessType(type);
        }
    }

    void ProcessType(TypeDefinition type)
    {
        foreach (var method in type.Methods)
        {
            //skip for abstract and delegates
            if (!method.HasBody)
            {
                continue;
            }

            var onExceptionProcessor = new OnExceptionProcessor
            {
                Method = method,
                ModuleWeaver = this
            };
            onExceptionProcessor.Process();
        }
    }
}