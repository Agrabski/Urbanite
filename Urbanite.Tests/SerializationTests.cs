using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Urbanite.Tests;

public class SerializationTests
{
	[Fact]
	public void ListOfPolymorphicTypesCanBeSerializedWhenRegisteredUsingGenericApi()
	{
		var serviceProvider = new ServiceCollection()
			.AddUrbanite()
			.AddUrbaniteSerializableType<IBase, A>("a")
			.AddUrbaniteSerializableType<IBase, B>("b")
			.BuildServiceProvider()
			;
		var options = new JsonSerializerOptions()
		{
			TypeInfoResolver = serviceProvider.GetRequiredService<IPolymorphicTypeResolver>()
		};
		var text = JsonSerializer.Serialize(new List<IBase>() { new A(), new B() }, options);
		Assert.Equal("""[{"$type":"a","PropertyA":0},{"$type":"b","PropertyB":null}]""", text);

	}


	[Fact]
	public void ListOfPolymorphicTypesCanBeSerializedWhenRegisteredUsingNonGenericApi()
	{
		var serviceProvider = new ServiceCollection()
			.AddUrbanite()
			.AddUrbaniteSerializableType(typeof(IBase), typeof(A), "a")
			.AddUrbaniteSerializableType(typeof(IBase), typeof(B), "b")
			.BuildServiceProvider()
			;
		var options = new JsonSerializerOptions()
		{
			TypeInfoResolver = serviceProvider.GetRequiredService<IPolymorphicTypeResolver>()
		};
		var text = JsonSerializer.Serialize(new List<IBase>() { new A(), new B() }, options);
		Assert.Equal("""[{"$type":"a","PropertyA":0},{"$type":"b","PropertyB":null}]""", text);

	}

	[Fact]
	public void ListOfPolymorphicOpenGenericTypesCanBeSerializedWhenRegistered()
	{
		var serviceProvider = new ServiceCollection()
			.AddUrbanite()
			.AddUrbaniteSerializableGenericType(typeof(IOpenGenericBase<>), typeof(GenericA<>), "a")
			.AddUrbaniteSerializableGenericType(typeof(IOpenGenericBase<>), typeof(GenericB<>), "b")
			.BuildServiceProvider()
			;
		var options = new JsonSerializerOptions()
		{
			TypeInfoResolver = serviceProvider.GetRequiredService<IPolymorphicTypeResolver>()
		};
		var text = JsonSerializer.Serialize(new List<IOpenGenericBase<int>>() { new GenericA<int>() { Value = 6 }, new GenericB<int>() { Value = 88 } }, options);
		Assert.Equal("""[{"$type":"a","A":0,"Value":6},{"$type":"b","B":0,"Value":88}]""", text);
		var deserialized = JsonSerializer.Deserialize<List<IOpenGenericBase<int>>>(text, options);
		Assert.Equal(2, deserialized?.Count);


	}
}