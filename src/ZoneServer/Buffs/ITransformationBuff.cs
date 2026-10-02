namespace Melia.Zone.Buffs
{
	/// <summary>
	/// Marks a buff that puts its target into a body other than its own.
	/// </summary>
	/// <remarks>
	/// The client offers to end a transformation early no matter what caused
	/// it, so anything that disguises its target implements this and is ended
	/// the same way. Whatever took the form has to be given back on the way
	/// out, which each handler's own end does.
	/// </remarks>
	public interface ITransformationBuff
	{
	}
}