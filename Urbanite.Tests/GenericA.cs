namespace Urbanite.Tests;

internal class GenericA<T> : IOpenGenericBase<T>
{
	public int A { get; set; }
	public T? Value { get; set; }
}
