using System.Collections.Generic;
using Melia.Shared.Game.Const;

namespace Melia.Zone.Skills.Combat
{
	/// <summary>
	/// Contains information about a hit from a skill.
	/// </summary>
	public class SkillHitResult
	{
		/// <summary>
		/// Gets or sets the amount of damage that should be dealt.
		/// </summary>
		public float Damage { get; set; }

		/// <summary>
		/// Gets or sets the hit result type, which affects how the
		/// damage is displayed.
		/// </summary>
		public HitResultType Result { get; set; } = HitResultType.Hit;

		/// <summary>
		/// Gets or sets the hit effect displayed on the target.
		/// </summary>
		public HitEffect Effect { get; set; } = HitEffect.Impact;

		/// <summary>
		/// Gets or sets the number of hits that are displayed. The damage
		/// will be split evenly between the hits.
		/// </summary>
		public int HitCount { get; set; } = 1;

		/// <summary>
		/// Returns the extra damage lines effects added to this hit, in the
		/// order their hooks ran.
		/// </summary>
		/// <remarks>
		/// One SkillHitResult travels through every phase of one hit, so the
		/// list collects what every hook on the same hit added, and the next
		/// hit starts empty because it builds a new result. The lines are
		/// dealt and embedded by SkillDamageHelper.ApplyExtraLines, at the
		/// site that builds the hit's packet.
		/// </remarks>
		public List<ExtraHitLine> ExtraLines { get; } = new();

		/// <summary>
		/// Gets or sets the knock back parameters for this hit.
		/// </summary>
		/// <remarks>
		/// The parameters are used to set up the knock back info
		/// automatically on knock back application if no other info was
		/// set set manually.
		/// </remarks>
		public KnockBackParameters KnockBack { get; set; } = new();

		/// <summary>
		/// Adds an extra line of damage to this hit, shown as a number of its
		/// own beside the hit's damage.
		/// </summary>
		/// <param name="damage"></param>
		public void AddExtraLine(float damage)
			=> this.AddExtraLine(damage, SkillId.None, this.Effect);

		/// <summary>
		/// Adds an extra line of damage to this hit, attributed to the given
		/// skill, shown as a number of its own beside the hit's damage.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="skillId"></param>
		public void AddExtraLine(float damage, SkillId skillId)
			=> this.AddExtraLine(damage, skillId, this.Effect);

		/// <summary>
		/// Adds an extra line of damage to this hit, attributed to the given
		/// skill, shown as a number of its own beside the hit's damage.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="skillId"></param>
		/// <param name="effect"></param>
		public void AddExtraLine(float damage, SkillId skillId, HitEffect effect)
		{
			if (damage <= 0)
				return;

			this.ExtraLines.Add(new ExtraHitLine { Damage = damage, SkillId = skillId, Effect = effect });
		}
	}
}
