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
}