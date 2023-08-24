using System.Text.Json.Serialization.Metadata;

namespace Urbanite;
public class PolymorphicTypeInfo : IPolymorphicTypeInfo
{
	private JsonDerivedType? _typeInfo;

	public Type Base { get; }
	public Type Derived { get; }
	public string? _discriminator;

	public PolymorphicTypeInfo(Type @base, Type derived, string? discriminator)
	{
		Base = @base;
		Derived = derived;
		_discriminator = discriminator;
	}

	public JsonDerivedType TypeInfo => _typeInfo ??= GenerateTypeInfo();

    private JsonDerivedType GenerateTypeInfo() => new(Derived, _discriminator ?? Derived.FullName ?? Derived.Name);

	public static PolymorphicTypeInfo
		FromImplementation<TBaseType, TDerived>(string? discriminator = null)
		where TDerived : TBaseType
	{
		var type = typeof(TDerived);
		return new(typeof(TBaseType), type, discriminator);
	}

	public JsonDerivedType? TryGetDerivedType(Type baseType)
	{
		return baseType == Base ? TypeInfo : null;
	}
}
