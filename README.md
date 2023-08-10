# Urbanite
This library extends System.Text.Json to make it easier to serialize polymorphic types.
Especially when working with type hierarchies that are not easily described using attributes.

This library works with type hierarhies described using attributes as well as using dependency injection extensions.
To use it, register Urbanite using `DIExtension`
```
services.AddUrbanite();
```
and then use `AddUrbaniteSerializableType` to register types that should be serialized polymorphically and couldn't be described using attributes.
```
interface IAnimal { }
class Dog : IAnimal { }
class Cat : IAnimal { }

services.AddUrbaniteSerializableType<IAnimal, Dog>("dog");
services.AddUrbaniteSerializableType<IAnimal, Cat>("cat");
```
To serialize polymorphically, use `JsonSerializerOptions` with `PolymorphicTypeResolver`:
```
class JsonOptionsConfiguration : IConfigureOptions<JsonOptions>
{
	private readonly PolymorphicTypeResolver _resolver;

	public JsonOptionsConfiguration(PolymorphicTypeResolver resolver)
	{
		_resolver = resolver;
	}

	public void Configure(JsonOptions options)
	{
		options.JsonSerializerOptions.TypeInfoResolver = _resolver;
	}
}

```
## Swagger
To make swagger work with polymorphic types, use `Urbanite.Extensions.Swagger`:
```
services.AddUrbaniteSwaggerDocumentationPolymorphism();
```
It will configure swagger to use `PolymorphicTypeResolver` to describe polymorphic types (selecting derived types and type discriminators). 
And set polymorphic type handling to use All of for inheritance.
