namespace Melia.Shared.Game.Const
{
	public enum MoveType
	{
		None,
		Normal,

		// Note sure if there's a difference between these. Pets use "Fly"
		// and monsters use "Flying". We'll keep both for now.
		Flying,
		Fly,

		Holding,
		Link,
	}
}
