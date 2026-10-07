using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Scouts.Shinobi;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for a Bunshin clone, which deals 25% of the Shinobi's damage,
	/// takes half damage and can't be knocked back or down.
	/// </summary>
	/// <remarks>
	/// The clone leaves the world when the buff ends, blowing up with
	/// [Arts] Bunshin no Jutsu: Mijin no Jutsu at the Shinobi's SP cost.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Bunshin_Buff)]
	public class Shinobi_Bunshin_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const float DamageDealtRate = 0.25f;
		private const float DamageTakenRate = 0.5f;
		private const float TaiDamageTakenRate = 0.7f;

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not DummyCharacter clone || clone.Map == null)
				return;

			var owner = clone.Owner;
			if (owner != null && owner.IsAbilityActive(AbilityId.Shinobi11) && owner.TryGetSkill(SkillId.Shinobi_Mijin_no_jutsu, out var mijin) && owner.TrySpendSp(mijin))
			{
				var cloneMijin = new Skill(clone, SkillId.Shinobi_Mijin_no_jutsu_Abil, mijin.Level);
				Shinobi_MijinNoJutsuOverride.Explode(cloneMijin, clone, clone.Position);
			}

			Send.ZC_LEAVE(clone);
			clone.Map.RemoveCharacter(clone);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Bunshin_Buff)]
		public void OnAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is DummyCharacter attackingClone && attacker.IsBuffActive(BuffId.Bunshin_Buff))
			{
				skillHitResult.Damage *= DamageDealtRate;

				if (skillHitResult.Damage > 0)
					ShinobiSkillHelper.OnCloneHit(attackingClone, target, skill.Id);
			}

			if (target is DummyCharacter clone && target.IsBuffActive(BuffId.Bunshin_Buff))
			{
				skillHitResult.Damage *= DamageTakenRate;

				if (clone.Owner?.IsAbilityActive(AbilityId.Shinobi17) == true)
					skillHitResult.Damage *= TaiDamageTakenRate;
			}
		}
	}

	/// <summary>
	/// Handler for Bunshin no Jutsu on the Shinobi, which drains stamina
	/// while the clones are out and takes them away when it ends.
	/// </summary>
	/// <remarks>
	/// Bunshin no Jutsu: Endurance slows the drain by 0.05 seconds per
	/// level, and Bunshin no Jutsu: Tai makes the Shinobi faster, sturdier
	/// and immune to knockback.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Bunshin_Debuff)]
	public class Shinobi_Bunshin_DebuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const int StaminaDrain = 1000;
		private const int DrainInterval = 1000;
		private const int EnduranceIntervalPerLevel = 50;
		private const float TaiMoveSpeed = 10f;
		private const float TaiDamageTakenRate = 0.7f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var enduranceLevel = target.TryGetActiveAbilityLevel(AbilityId.Shinobi6, out var level) ? level : 0;

			buff.SetUpdateTime(DrainInterval + enduranceLevel * EnduranceIntervalPerLevel);

			if (target.IsAbilityActive(AbilityId.Shinobi17))
				UpdatePropertyModifier(buff, target, PropertyName.MSPD_BM, TaiMoveSpeed);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			character.ModifyStamina(-StaminaDrain);

			if (character.Properties.Stamina <= 0)
				character.StopBuff(BuffId.Bunshin_Debuff);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);

			foreach (var clone in ShinobiSkillHelper.GetClones(buff.Target))
				clone.StopBuff(BuffId.Bunshin_Buff);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> target.IsAbilityActive(AbilityId.Shinobi17) ? KnockResult.Prevent : KnockResult.Allow;

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> target.IsAbilityActive(AbilityId.Shinobi17) ? KnockResult.Prevent : KnockResult.Allow;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Bunshin_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target.IsBuffActive(BuffId.Bunshin_Debuff) && target.IsAbilityActive(AbilityId.Shinobi17))
				skillHitResult.Damage *= TaiDamageTakenRate;
		}
	}
}
