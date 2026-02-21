namespace Melia.Shared.ObjectProperties
{
	public interface IProperty : Properties.IVariable
	{
	}
	/// <summary>
	/// A property that can't be set.
	/// </summary>
	/// <remarks>
	/// Used to quickly determine wheter a certain property can be set or not.
	/// </remarks>
	public interface IUnsettableProperty
	{
	}
}
