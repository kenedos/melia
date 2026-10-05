using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Wizards.Elementalist
{
	/// <summary>
	/// Handler override for the Blizzard Storm debuff.
	/// Periodically deals Ice property damage and reduces movement speed.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.StormDust_Debuff)]
	public class StormDust_DebuffOverride : BuffHandler
	{
		private const string ModPropertyName = PropertyName.MSPD_BM;
		private const int DamageTickInterval = 500;

		public override void OnStart(Buff buff)
		{
			var target = buff.Target;

			buff.SetUpdateTime(DamageTickInterval);

			if (buff.Caster is ICombatEntity caster
				&& caster.TryGetActiveAbilityLevel(AbilityId.Elementalist28, out var abilityLevel))
			{
				var modValue = abilityLevel * 2;
				AddPropertyModifier(buff, target, ModPropertyName, -modValue);
			}
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target.IsDead)
				return;

			var attacker = buff.Caster;
			var target = buff.Target;

			if (attacker is ICombatEntity caster
				&& caster.TryGetSkill(SkillId.Elementalist_StormDust, out var skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hitInfo = new HitInfo(caster, target, skillHitResult.Damage, HitResultType.Hit);
				hitInfo.Type = HitType.Ice;

				Send.ZC_HIT_INFO(caster, target, hitInfo);
			}
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, ModPropertyName);

			if (buff.Caster is ICombatEntity caster
				&& caster.TryGetActiveAbilityLevel(AbilityId.Elementalist29, out var abilityLevel)
				&& !buff.Vars.GetBool("Melia.Extended"))
			{
				var duration = TimeSpan.FromSeconds(2.5 * abilityLevel);
				var newBuff = buff.Target.StartBuff(BuffId.StormDust_Debuff, buff.NumArg1, buff.NumArg2, duration, caster);
				if (newBuff != null)
					newBuff.Vars.SetBool("Melia.Extended", true);
			}
		}
	}
}
