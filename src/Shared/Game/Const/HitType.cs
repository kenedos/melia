namespace Melia.Shared.Game.Const
{
	/// <summary>
	/// A skill's hit type, which determines certain skill hit values.
	/// </summary>
	/// <remarks>
	/// Based on the property CT_HitType. It's currently unknown whether
	/// this enum is used in packets.
	/// </remarks>
	public enum SkillHitType
	{
		Melee,
		Force,
		Magic,
		Pad,
		Installation,
		Companion,
		Companion_Flying,
		Fire,
	}

	/// <summary>
	/// A skill's knock back type, which determines how the target is
	/// knocked back by the skill.
	/// </summary>
	/// <remarks>
	/// Based on the numeric property CT_KnockDownHitType. The names are
	/// largely guessed based on how the values are used.
	/// </remarks>
	public enum KnockBackType : short
	{
		None = 0,
		Motion = 1,
		NoMotion = 2,
		KnockBack = 3,
		KnockDown = 4,
		Type18 = 18,
	}

	/// <summary>
	/// Visual hit effect type displayed on the target.
	/// </summary>
	public enum HitType : short
	{
		Normal = 0,
		Guard = 5,
		Force = 6,
		Heal = 7,
		HealNoEffect = 8,
		Shield = 9,
		Safety = 10,
		Telekinesis = 11,
		Poison = 12,
		Stabdoll = 13,
		Block = 14,
		Dodge = 15,
		Ice = 16,
		Countdown = 19,
		Fire = 21,
		JangpanHit = 22,
		JangpanBless = 23,
		JangpanCure = 24,
		JangpanHeal = 25,
		PoisonGreen = 26,
		Reflect = 27,
		NoEffect = 28,
		Endure = 29,
		PoisonViolet = 30,
		Lightning = 31,
		PlantGuard = 32,
		Holy = 33,
		Dark = 34,
		Bleeding = 35,
		Concentrate = 36,
		Earth = 37,
		Retreat = 38,
		Soul = 39,
		BasicNotCancelCast = 40,
		NoHit = 100,
		Magic = 1001,
		Melee = 1002,
		WuguBlood = 1003,
		WuguPoison = 1004,
		TwinkleStar = 2001,
	}
}
