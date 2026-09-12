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
	/// target is under, and the target's debuffs run longer for as long as
	/// it lasts.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DiscernEvil_Buff)]
	public class Pardoner_DiscernEvil_BuffOverride : BuffHandler, IBuffOnDebuffAppliedHandler
	{
		private const float DamageBonusPerDebuff = 0.05f;
		private const int MaxCountedDebuffs = 5;
		private const string VarExtended = "Melia.Buff.DiscernEvil.Extended";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			foreach (var debuff in buff.Target.Components.Get<BuffComponent>().GetAll(a => a.Data.Type == BuffType.Debuff))
				this.ExtendDebuff(buff, debuff);
		}

		public void OnDebuffApplied(Buff buff, Buff debuff)
		{
			this.ExtendDebuff(buff, debuff);
		}

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
			modifier.DamageMultiplier += this.GetDebuffCount(target) * DamageBonusPerDebuff;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var hitInfo = new HitInfo(caster, target, skillHitResult.Damage, HitResultType.Hit);
			hitInfo.Type = HitType.Holy;

			Send.ZC_HIT_INFO(caster, target, hitInfo);
		}

		/// <summary>
		/// Extends the given debuff's remaining duration once, by as long as
		/// Discerning Evil's own sentence runs.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="debuff"></param>
		private void ExtendDebuff(Buff buff, Buff debuff)
		{
			if (!debuff.HasDuration || debuff.Data.Tags.HasAny(BuffTag.IgnoreDiscernEvil))
				return;

			if (debuff.Vars.GetBool(VarExtended))
				return;

			debuff.IncreaseDuration(debuff.RemainingDuration + GetCaptionTime(buff));
			debuff.Vars.SetBool(VarExtended, true);
			debuff.NotifyUpdate();
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
