using Melia.Shared.Game.Const;

namespace Melia.Zone.Skills.Combat
{
	/// <summary>
	/// An extra line of damage an effect added to a hit, displayed as a
	/// number of its own beside the hit's damage.
	/// </summary>
	public class ExtraHitLine
	{
		/// <summary>
		/// Gets or sets the damage the line deals.
		/// </summary>
		public float Damage { get; set; }

		/// <summary>
		/// Gets or sets the hit effect displayed for the line.
		/// </summary>
		public HitEffect Effect { get; set; } = HitEffect.Impact;

		/// <summary>
		/// Gets or sets the skill the line is attributed to, or None to
		/// attribute it to the skill that was used.
		/// </summary>
		/// <remarks>
		/// Official gives an added line its own id rather than the attack's -
		/// a Spell Shop Sacrament hit arrives as SpellShop_Sacrament_AddBlow,
		/// not as the basic attack it rode in on.
		/// </remarks>
		public SkillId SkillId { get; set; } = SkillId.None;
	}
}
