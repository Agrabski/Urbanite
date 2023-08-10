using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Urbanite.Extensions.Swagger;
public class SwaggerOptionsConfiguration : IConfigureOptions<SwaggerGenOptions>
{
	private readonly IPolymorphicTypeResolver _infoResolver;

	public SwaggerOptionsConfiguration(IPolymorphicTypeResolver infoResolver)
	{
		_infoResolver = infoResolver;
	}

	public void Configure(SwaggerGenOptions s)
	{
		var typeDiscriminators = new Dictionary<Type, string>();
		s.SupportNonNullableReferenceTypes();
		s.UseAllOfForInheritance();
		s.SelectSubTypesUsing(baseType =>
		{
			var info = _infoResolver.GetTypeInfo(baseType, new());
			if (info.PolymorphismOptions is not null)
			{
				foreach (var derivedType in info.PolymorphismOptions.DerivedTypes)
					typeDiscriminators[derivedType.DerivedType] = derivedType.TypeDiscriminator?.ToString() ?? throw new InvalidOperationException($"Type dyscriminator for {derivedType} was null");
				return info.PolymorphismOptions.DerivedTypes.Select(d => d.DerivedType);
			}
			return Enumerable.Empty<Type>();
		});
		s.SelectDiscriminatorNameUsing(baseType =>
		{
			var info = _infoResolver.GetTypeInfo(baseType, new());
			if (info.PolymorphismOptions is not null)
				return info.PolymorphismOptions.TypeDiscriminatorPropertyName;
			return string.Empty;

		});
		s.SelectDiscriminatorValueUsing(type =>
		{
			if (typeDiscriminators.ContainsKey(type))
				return typeDiscriminators[type];
			throw new InvalidOperationException($"Unrecognised type: {type}");
		});
	}
}