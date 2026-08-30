using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Discerning Evil debuff, which damages the target in
	/// regular intervals. The damage grows with the number of debuffs the
	/// target is under.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DiscernEvil_Buff)]
	public class Pardoner_DiscernEvil_BuffOverride : BuffHandler
	{
		private const float DamageBonusPerDebuff = 0.02f;
		private const int MaxCountedDebuffs = 5;

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead)
				return;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.Pardoner_DiscernEvil, out var skill))
				return;

			var modifier = new SkillModifier();
			modifier.DamageMultiplier += this.GetDebuffCount(target) * DamageBonusPerDebuff * buff.NumArg1;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var hitInfo = new HitInfo(caster, target, skillHitResult.Damage, HitResultType.Hit);
			hitInfo.Type = HitType.Holy;

			Send.ZC_HIT_INFO(caster, target, hitInfo);
		}

		/// <summary>
		/// Returns the number of debuffs on the target that count towards
		/// the damage bonus.
		/// </summary>
		/// <param name="target"></param>
		private int GetDebuffCount(ICombatEntity target)
		{
			var count = target.Components.Get<BuffComponent>()
				.GetAll(a => a.Data.Type == BuffType.Debuff && a.Id != BuffId.DiscernEvil_Buff)
				.Count;

			return count > MaxCountedDebuffs ? MaxCountedDebuffs : count;
		}
	}
}
