namespace Urbanite.Tests;

internal class GenericB<T> : IOpenGenericBase<T>
{
	public int B { get; set; }
	public T? Value { get; set; }
}