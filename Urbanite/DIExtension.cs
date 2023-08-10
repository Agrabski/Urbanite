using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization.Metadata;

namespace Urbanite;
public static class DIExtension
{
	public static IServiceCollection AddUrbanite(this IServiceCollection collection) => collection
		.AddSingleton<IPolymorphicTypeResolver, PolymorphicTypeResolver>()
		.AddSingleton<IJsonTypeInfoResolver, PolymorphicTypeResolver>();
	public static IServiceCollection AddUrbaniteSerializableType<TBase, TImpl>(this IServiceCollection collection, string? discriminator = null) where TImpl : TBase => collection
		.AddSingleton<IPolymorphicTypeInfo>(_ => PolymorphicTypeInfo<TBase>.FromImplementation<TImpl>(discriminator));
}
