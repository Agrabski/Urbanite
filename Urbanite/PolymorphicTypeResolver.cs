using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
namespace Urbanite;

internal class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver, IPolymorphicTypeResolver
{
    private readonly IPolymorphicTypeInfo[] _typeInfos;
    private readonly ConcurrentDictionary<Type, JsonTypeInfo> _typeInfoCache = new();

    public PolymorphicTypeResolver(IEnumerable<IPolymorphicTypeInfo> typeInfos)
    {
        _typeInfos = typeInfos.ToArray();
    }

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        return _typeInfoCache.GetOrAdd(type, t => GetTypeInfoImpl(t, options));
    }

    private JsonTypeInfo GetTypeInfoImpl(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);
        if (
            type.IsInterface &&
            _typeInfos
                .Select(t => t.TryGetDerivedType(type))
                .Where(t => t != null)
                .Select(t => t!.Value)
                .ToArray() is { Length: > 0 } types)
            return GetTypeInfoWithDerivedTypes(jsonTypeInfo, types);
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
