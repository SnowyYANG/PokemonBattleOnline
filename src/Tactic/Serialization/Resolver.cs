using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;

namespace LightStudio.Tactic.Serialization
{
  internal class Resolver : DataContractResolver
  {
    private const string CollectionType = "CollectionOf";
    private const string ArrayType = "ArrayOf";

    private static bool IsCollection(Type type)
    {
      return type.IsGenericType &&
          type.GetGenericTypeDefinition() == typeof(Collection<>);
    }

    private static bool IsArray(Type type)
    {
      return type.IsArray && type.GetArrayRank() == 1;
    }

    private static Type GetGenericCollectionType(Type arg)
    {
      return typeof(Collection<>).MakeGenericType(arg);
    }

    public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
    {
      Type type = knownTypeResolver.ResolveName(typeName, typeNamespace,
          declaredType, null);
      if (type != null)
        return type;
      if (typeNamespace == Namespaces.DEFAULT)
      {
        if (typeName.StartsWith(CollectionType))
        {
          Type argType = ResolveName(typeName.Substring(12), knownTypeResolver);
          if (argType != null)
            return Resolver.GetGenericCollectionType(argType);
        }
        else if (typeName.StartsWith(ArrayType))
        {
          Type elementType = ResolveName(typeName.Substring(7), knownTypeResolver);
          return elementType.MakeArrayType();
        }
      }
      return null;
    }

    private Type ResolveName(string name, DataContractResolver knownTypeResolver)
    {
      string ns;
      if (name.StartsWith(Namespaces.MS))
      {
        ns = Namespaces.STANDARD;
        name = name.Substring(2);
      }
      else
      {
        ns = Namespaces.DEFAULT;
      }
      return ResolveName(name, ns, typeof(object), knownTypeResolver);
    }

    public override bool TryResolveType(Type type, Type declaredType,
        DataContractResolver knownTypeResolver,
        out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
    {
      if (knownTypeResolver.TryResolveType(type, declaredType,
          null, out typeName, out typeNamespace))
        return true;

      string typeNameString = null;
      if (Resolver.IsCollection(type))
      {
        TryGetCollectionTypeName(type, knownTypeResolver, out typeNameString);
      }
      else if (Resolver.IsArray(type))
      {
        TryGetArrayTypeName(type, knownTypeResolver, out typeNameString);
      }

      if (typeNameString != null)
      {
        typeNamespace = new XmlDictionaryString(XmlDictionary.Empty, Namespaces.DEFAULT, 0);
        typeName = new XmlDictionaryString(XmlDictionary.Empty, typeNameString, 0);
        return true;
      }
      return false;
    }

    #region GetTypeName

    private bool TryGetCollectionTypeName(Type type, DataContractResolver knownTypeResolver, out string typeName)
    {
      Type typeArg = type.GetGenericArguments()[0];
      return TryGetCollectionTypeName(Resolver.CollectionType, typeArg,
          knownTypeResolver, out typeName);
    }

    private bool TryGetArrayTypeName(Type type, DataContractResolver knownTypeResolver, out string typeName)
    {
      Type elementType = type.GetElementType();
      return TryGetCollectionTypeName(Resolver.ArrayType, elementType,
          knownTypeResolver, out typeName);
    }

    private bool TryGetCollectionTypeName(string collectionType, Type elementType,
        DataContractResolver knownTypeResolver, out string typeName)
    {
      XmlDictionaryString elementTypeName;
      XmlDictionaryString elementNamespace;
      if (TryResolveType(elementType, elementType, knownTypeResolver,
          out elementTypeName, out elementNamespace))
      {
        switch (elementNamespace.Value)
        {
          case Namespaces.STANDARD:
            typeName = string.Format("{0}{1}{2}", collectionType, Namespaces.MS,
                elementTypeName.Value);
            return true;
          case Namespaces.DEFAULT:
            typeName = string.Format("{0}{1}", collectionType, elementTypeName.Value);
            return true;
        }
      }
      typeName = null;
      return false;
    }

    #endregion
  }
}
