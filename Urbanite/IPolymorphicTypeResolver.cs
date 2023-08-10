using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Urbanite;
public interface IPolymorphicTypeResolver
{
	JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options);
}