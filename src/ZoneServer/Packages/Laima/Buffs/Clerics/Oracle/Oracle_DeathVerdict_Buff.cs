using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Death Sentence buff, which increases damage
	/// taken by the target. On expiration, deals accumulated damage
	/// as Dark magic damage.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DeathVerdict_Buff)]
	public class Oracle_DeathVerdict_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			var addDamageRate = (30f + skillLevel * 5f) / 100f;
			buff.Vars.SetFloat("ADD_DAMAGE_RATE", addDamageRate);

			buff.Vars.SetFloat("CumulativeDamage", 0);

			if (buff.Caster is ICombatEntity caster)
			{
				var abilLevel = caster.GetAbilityLevel(AbilityId.Oracle8);
				if (abilLevel > 0)
				{
					var mspdReduce = target.Properties.GetFloat(PropertyName.MSPD) * 0.15f * abilLevel;
					AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -mspdReduce);
				}
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);

			if (!target.IsDead && buff.Caster is ICombatEntity caster)
			{
				var cumulativeDamage = buff.Vars.GetFloat("CumulativeDamage");
				if (cumulativeDamage > 0)
				{
					var oracle13Level = caster.GetAbilityLevel(AbilityId.Oracle13);
					if (oracle13Level > 0)
						cumulativeDamage *= (1f - oracle13Level * 0.1f);

					if (cumulativeDamage > 0)
						target.TakeSimpleHit(cumulativeDamage, caster, SkillId.Oracle_DeathVerdict);
				}
			}
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc_Defense, BuffId.DeathVerdict_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.DeathVerdict_Buff, out var buff))
				return;

			var addDamageRate = buff.Vars.GetFloat("ADD_DAMAGE_RATE");
			modifier.DamageMultiplier += addDamageRate;
		}
	}
}
