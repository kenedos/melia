namespace Melia.Shared.Game.Const
{
	/// <summary>
	/// Constants describing how entities move through the world.
	/// </summary>
	public static class Movement
	{
		/// <summary>
		/// World units covered per second, per point of MSPD. Empirically
		/// a 30 MSPD entity travels ~75 units/s, so 1 MSPD ≈ 2.5 units/s.
		/// </summary>
		public const float UnitsPerMspdSecond = 2.5f;
	}
}
