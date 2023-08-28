using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
namespace Urbanite;

internal class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver, IPolymorphicTypeResolver
{
	private readonly IPolymorphicTypeInfo[] _typeInfos;
	private readonly ConcurrentDictionary<Type, JsonDerivedType[]> _typeInfoCache = new();

	public PolymorphicTypeResolver(IEnumerable<IPolymorphicTypeInfo> typeInfos)
	{
		_typeInfos = typeInfos.ToArray();
	}

	public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		var jsonTypeInfo = base.GetTypeInfo(type, options);
		var polymorphicTypeInfos = _typeInfoCache.GetOrAdd(type, t => _typeInfos
			.Select(t => t.TryGetDerivedType(type))
			.Where(t => t != null)
			.Select(t => t!.Value)
			.ToArray()
		);
		if (polymorphicTypeInfos.Any())
			return GetTypeInfoWithDerivedTypes(jsonTypeInfo, polymorphicTypeInfos);
		return jsonTypeInfo;
	}
	private JsonTypeInfo GetTypeInfoWithDerivedTypes(JsonTypeInfo baseInfo, IEnumerable<JsonDerivedType> derivedTypes)
	{
		baseInfo.PolymorphismOptions ??= new();
		baseInfo.PolymorphismOptions.IgnoreUnrecognizedTypeDiscriminators = true;
		baseInfo.PolymorphismOptions.UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization;
		foreach (var t in derivedTypes)
			baseInfo.PolymorphismOptions.DerivedTypes.Add(t);
		return baseInfo;
	}
}
