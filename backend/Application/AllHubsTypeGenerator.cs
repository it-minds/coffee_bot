// using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Application.Common.Interfaces.Hubs;

namespace Application
{
  public class AllHubsTypeGenerator
  {
    public Type Generate() {
      var dict = AllMyHubs();

      TypeBuilder parent = GetTypeBuilder("AllHubs");
      ConstructorBuilder parentConstructor = parent.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

      foreach (var item in dict)
      {
        TypeBuilder tb = GetTypeBuilder(item.Key + "Hub");
        ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

        // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
        foreach (var field in item.Value)
            CreateProperty(tb, field.Key, field.Value.First());

        Type objectType = tb.CreateType();
        CreateProperty(parent, item.Key, objectType);
      }

      return parent.CreateType();
    }

    private TypeBuilder GetTypeBuilder(string typeSignature = "MyDynamicType")
    {
      var an = new AssemblyName(typeSignature);

      var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

      ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

      TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
        TypeAttributes.Public |
        TypeAttributes.Class |
        TypeAttributes.AutoClass |
        TypeAttributes.AnsiClass |
        TypeAttributes.BeforeFieldInit |
        TypeAttributes.AutoLayout,
        null);

      return tb;
    }

    private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
    {
      FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

      PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
      MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
      ILGenerator getIl = getPropMthdBldr.GetILGenerator();

      getIl.Emit(OpCodes.Ldarg_0);
      getIl.Emit(OpCodes.Ldfld, fieldBuilder);
      getIl.Emit(OpCodes.Ret);

      MethodBuilder setPropMthdBldr =
          tb.DefineMethod("set_" + propertyName,
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.HideBySig,
            null, new[] { propertyType });

      ILGenerator setIl = setPropMthdBldr.GetILGenerator();
      Label modifyProperty = setIl.DefineLabel();
      Label exitSet = setIl.DefineLabel();

      setIl.MarkLabel(modifyProperty);
      setIl.Emit(OpCodes.Ldarg_0);
      setIl.Emit(OpCodes.Ldarg_1);
      setIl.Emit(OpCodes.Stfld, fieldBuilder);

      setIl.Emit(OpCodes.Nop);
      setIl.MarkLabel(exitSet);
      setIl.Emit(OpCodes.Ret);

      propertyBuilder.SetGetMethod(getPropMthdBldr);
      propertyBuilder.SetSetMethod(setPropMthdBldr);
    }

    private Dictionary<string, Dictionary<string, IEnumerable<Type>>> AllMyHubs()
    {
      var dict = new Dictionary<string, Dictionary<string, IEnumerable<Type>>>();
      dict.Add("Prize", MyHubs(typeof(IPrizeHubService)));
      return dict;
    }

    private Dictionary<string, IEnumerable<Type>> MyHubs(Type type)
    {
      var dict = new Dictionary<string, IEnumerable<Type>>();
      foreach (MethodInfo method in type.GetMethods())
      {
        dict.Add(method.Name, method.GetParameters().Select(x => x.ParameterType));
      }
      return dict;
    }
  }
}
