using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Bullet Marker's Overheating, Outrage
	/// and Tracer Bullet.
	/// </summary>
	public static class BulletMarkerSkillHelper
	{
		private const int MaxOverheating = 40;
		private const int SkillOverheating = 2;
		private static readonly TimeSpan OverheatingDuration = TimeSpan.FromSeconds(35);

		/// <summary>
		/// Returns true if the caster is in Double Gun Stance, telling them
		/// otherwise.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static bool CheckDoubleGunStance(ICombatEntity caster)
		{
			if (caster.IsBuffActive(BuffId.DoubleGunStance_Buff))
				return true;

			caster.ServerMessage(Localization.Get("Double Gun Stance must be active."));
			return false;
		}

		/// <summary>
		/// Spends a stack of Outrage for a Bullet Marker skill and returns
		/// true, or grants 2 stacks of Overheating if there is no Outrage.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static bool TryConsumeOutrage(ICombatEntity caster)
		{
			if (!caster.TryGetBuff(BuffId.Outrage_Buff, out var outrage))
			{
				AddOverheating(caster, SkillOverheating);
				return false;
			}

			outrage.DecreaseOverbuff();

			if (outrage.OverbuffCounter <= 0)
				caster.StopBuff(BuffId.Outrage_Buff);
			else
				outrage.NotifyUpdate();

			return true;
		}

		/// <summary>
		/// Adds stacks of Overheating while in Double Gun Stance and not in
		/// Outrage, up to 40.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="stacks"></param>
		public static void AddOverheating(ICombatEntity caster, int stacks)
		{
			if (!caster.IsBuffActive(BuffId.DoubleGunStance_Buff) || caster.IsBuffActive(BuffId.Outrage_Buff))
				return;

			stacks = Math.Min(stacks, MaxOverheating - caster.GetOverbuffCount(BuffId.Overheating_Buff));

			for (var i = 0; i < stacks; i++)
				caster.StartBuff(BuffId.Overheating_Buff, 1, 0, OverheatingDuration, caster, SkillId.Bulletmarker_Outrage);
		}

		/// <summary>
		/// Returns a modifier for a Bullet Marker skill, carrying Tracer
		/// Bullet's accuracy and minimum critical chance.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static SkillModifier CreateModifier(ICombatEntity caster)
		{
			var modifier = new SkillModifier();

			if (caster.TryGetSkill(SkillId.Bulletmarker_TracerBullet, out var tracerBullet))
			{
				modifier.HitRateMultiplier += tracerBullet.Properties.GetFloat(PropertyName.CaptionRatio) / 100f;
				modifier.MinCritChance = Math.Max(modifier.MinCritChance, tracerBullet.Properties.GetFloat(PropertyName.CaptionRatio2));
			}

			return modifier;
		}
	}
}
