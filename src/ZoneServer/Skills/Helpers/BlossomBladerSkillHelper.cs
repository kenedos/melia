using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Blossom Blader's Flowering and StartUp.
	/// </summary>
	public static class BlossomBladerSkillHelper
	{
		/// <summary>
		/// Adds a stack of the caster's Flowering to the target, up to the
		/// skill's stack limit.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		public static void ApplyFlowering(ICombatEntity caster, ICombatEntity target)
		{
			if (target.IsDead || !caster.TryGetSkill(SkillId.BlossomBlader_Flowering, out var flowering))
				return;

			if (target.TryGetBuff(BuffId.Flowering_Debuff, out var buff) && buff.Caster != caster)
				target.StopBuff(BuffId.Flowering_Debuff);

			var maxStacks = (int)flowering.Properties.GetFloat(PropertyName.CaptionRatio2);
			if (target.GetOverbuffCount(BuffId.Flowering_Debuff) >= maxStacks)
				return;

			target.StartBuff(BuffId.Flowering_Debuff, flowering.Level, 0, flowering.Properties.CaptionTime, caster, flowering.Id);
		}

		/// <summary>
		/// Returns the number of the caster's Flowering stacks on the target.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static int GetFloweringStacks(ICombatEntity caster, ICombatEntity target)
		{
			if (!target.TryGetBuff(BuffId.Flowering_Debuff, out var buff) || buff.Caster != caster)
				return 0;

			return buff.OverbuffCounter;
		}

		/// <summary>
		/// Returns the extra hit StartUp: Blossom Shower grants after a fully
		/// charged StartUp.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static int GetBlossomShowerHits(ICombatEntity caster)
			=> caster.IsBuffActive(BuffId.StartUp_Abil_Buff) ? 1 : 0;
	}
}
