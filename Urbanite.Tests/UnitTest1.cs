using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Urbanite.Tests;

public class SerializationTests
{
	[Fact]
	public void Test1()
	{
		var serviceProvider = new ServiceCollection()
			.AddUrbanite()
			.BuildServiceProvider()
			;
		var options = new JsonSerializerOptions()
		{
			TypeInfoResolver = serviceProvider.GetRequiredService<IPolymorphicTypeResolver>()
		}
	}
}