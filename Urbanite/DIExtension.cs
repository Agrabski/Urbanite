using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization.Metadata;

namespace Urbanite;
public static class DIExtension
{
	public static IServiceCollection AddUrbanite(this IServiceCollection collection) => collection
		.AddSingleton<IPolymorphicTypeResolver, PolymorphicTypeResolver>()
		.AddSingleton<IJsonTypeInfoResolver, PolymorphicTypeResolver>();
	public static IServiceCollection AddUrbaniteSerializableType<TBase, TImpl>(this IServiceCollection collection, string? discriminator = null) where TImpl : TBase => collection
		.AddSingleton<IPolymorphicTypeInfo>(_ => PolymorphicTypeInfo.FromImplementation<TBase, TImpl>(discriminator));

	public static IServiceCollection AddUrbaniteSerializableType(this IServiceCollection collection, Type baseType, Type derived, string? discriminator = null) => collection
		.AddSingleton<IPolymorphicTypeInfo>(_ => new PolymorphicTypeInfo(baseType, derived, discriminator));
}
