using System.Text.Json.Serialization.Metadata;

namespace Urbanite;

public class OpenGenericPolymorphicTypeInfo : IPolymorphicTypeInfo
{
	public Type Base { get; }
	public Type Derived { get; }
	public string? _discriminator;

	public OpenGenericPolymorphicTypeInfo(Type @base, Type derived, string? discriminator)
	{
		if(!@base.IsGenericTypeDefinition)
			throw new ArgumentException("Base type must be an open generic type definition.", nameof(@base));
		if(!derived.IsGenericTypeDefinition)
			throw new ArgumentException("Derived type must be an open generic type definition.", nameof(derived));
		if (@base.GetGenericArguments().Length != derived.GetGenericArguments().Length)
			throw new ArgumentException("Base and derived types must have the same number of generic arguments.", nameof(derived));
		Base = @base;
		Derived = derived;
		_discriminator = discriminator;
	}

	public JsonDerivedType? TryGetDerivedType(Type baseType)
	{
		if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == Base)
		{
			var implementationType = Derived.MakeGenericType(baseType.GetGenericArguments());
			return new JsonDerivedType(implementationType, _discriminator ?? implementationType.FullName ?? implementationType.Name);
		}
		return null;
	}
}
