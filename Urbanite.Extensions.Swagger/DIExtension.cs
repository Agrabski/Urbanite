using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Urbanite.Extensions.Swagger;

public static class DIExtension
{
	public static IServiceCollection AddUrbaniteSwaggerDocumentationPolymorphism(this IServiceCollection collection) => collection
		.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptionsConfiguration>();

}
