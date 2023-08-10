using System.Text.Json.Serialization.Metadata;

namespace Urbanite;

public interface IPolymorphicTypeInfo
{
	JsonDerivedType? TryGetDerivedType(Type baseType);
}
