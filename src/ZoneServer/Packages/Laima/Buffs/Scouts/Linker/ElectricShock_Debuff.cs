using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handle for the Electric Shock debuff. Target is connected and takes
	/// periodic lightning damage. If the target moves more than 200 units
	/// away from the caster, the debuff is removed.
	/// Each target has its own independent link that is cleaned up when
	/// that specific buff ends.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ElectricShock_Debuff)]
	public class ElectricShock_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const float MaxCasterDistance = 200f;

		/// <summary>
		/// Called when the buff is activated. Sets up damage snapshot.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);
		}

		/// <summary>
		/// Periodically checks distance from caster and deals damage.
		/// If target is too far from caster, removes the debuff.
		/// </summary>
		public override void WhileActive(Buff buff)
		{
			if (buff.Caster is not ICombatEntity caster)
				return;

			if (caster == buff.Target)
				return;

			if (caster.IsDead || buff.Target.Map == null || caster.Map == null || caster.Map != buff.Target.Map)
			{
				buff.Target.StopBuff(BuffId.ElectricShock_Debuff);
				return;
			}

			var distance = buff.Target.Position.Get2DDistance(caster.Position);
			if (distance > MaxCasterDistance)
			{
				buff.Target.StopBuff(BuffId.ElectricShock_Debuff);
				return;
			}

			base.WhileActive(buff);
		}

		/// <summary>
		/// When the buff ends, destroy the visual link effect for this specific target.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			if (buff.Caster != null && buff.Vars.TryGet<string>("Melia.Link.EffectName", out var effectName))
			{
				buff.Caster.RemoveEffect(effectName);
			}

			buff.Target.RemoveEffect("Melia.Link.Chain");
		}

		/// <summary>
		/// Returns the skill ID for damage display.
		/// </summary>
		protected override SkillId GetSkillId(Buff buff)
		{
			return SkillId.Linker_ElectricShock;
		}

		/// <summary>
		/// Returns the hit type for lightning damage visuals.
		/// </summary>
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Lightning;
		}
	}
}
