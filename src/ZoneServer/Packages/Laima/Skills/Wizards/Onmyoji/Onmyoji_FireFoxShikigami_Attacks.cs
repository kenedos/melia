using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	public class FireFox_Proc_BuffOverride
	{
		private const int NormalFoxId = 300002;
		private const int GrownFoxId = 300007;
		private const float ProcChance = 15f; // 15% de chance por hit

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.FireFoxShikigami_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || target == null || target.IsDead)
				return;

			// Processa apenas se o ataque causou dano efetivo
			if (skillHitResult.Damage > 0)
			{
				var rng = RandomProvider.Get().Next(100);
				if (rng < ProcChance)
				{
					var fox = character.Summons.GetSummons(s => !s.IsDead && ((int)s.Id == NormalFoxId || (int)s.Id == GrownFoxId)).FirstOrDefault();
					if (fox == null || fox.Map != character.Map)
						return;

					if (character.TryGetSkill(SkillId.Onmyoji_FireFoxShikigami, out var foxSkill))
					{
						fox.TurnTowards(target.Position);
						_ = FireFoxAttack.Execute(foxSkill, fox, target);
					}
				}
			}
		}
	}

	internal static class FireFoxAttack
	{
		private const int GrownFoxId = 300007;
		private const int BaseHits = 2;
		private const int AdditionalHits = 3;
		private const float GrownFoxDamageMultiplier = 1.5f;
		private const string OwnerKey = "Melia.Summoner.Owner";

		public static async Task Execute(Skill animationSkill, ICombatEntity caster, ICombatEntity target)
		{
			if (caster is not Summon summon || target == null || target.IsDead || !summon.Vars.TryGet<Character>(OwnerKey, out var owner) || owner.IsDead)
				return;

			if (!owner.TryGetSkill(SkillId.Onmyoji_FireFoxShikigami, out var damageSkill))
				return;

			var modifier = SkillModifier.Default;
			modifier.HitCount = BaseHits + (owner.IsAbilityActive(AbilityId.Onmyoji17) ? AdditionalHits : 0);
			modifier.AttackAttribute = (int)summon.Id == GrownFoxId ? AttributeType.Fire : AttributeType.Soul;
			if ((int)summon.Id == GrownFoxId)
				modifier.FinalDamageMultiplier *= GrownFoxDamageMultiplier;

			var result = SCR_SkillHit(summon, target, damageSkill, modifier);

			if (owner.IsAbilityActive(AbilityId.Onmyoji20))
				result.Damage *= 1.10f;

			if (result.Result != HitResultType.Dodge && result.Damage > 0f)
				target.TakeDamage(result.Damage, summon);

			var hit = new SkillHitInfo(summon, target, damageSkill, result, TimeSpan.FromMilliseconds(100), TimeSpan.Zero);
			hit.HitInfo.Type = (int)summon.Id == GrownFoxId ? HitType.Fire : HitType.Soul;
			Send.ZC_SKILL_HIT_INFO(summon, new List<SkillHitInfo> { hit });
			summon.SetAttackState(false);

			if (animationSkill != null)
				await animationSkill.Wait(TimeSpan.FromMilliseconds(100));
		}
	}

	[Package("laima")]
	[SkillHandler(SkillId.Mon_pcskill_FireFoxShikigami_Skill_1)]
	public class Onmyoji_FireFoxShikigamiSoulAttackOverride : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (target == null || target.IsDead)
				return;

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
			skill.Run(FireFoxAttack.Execute(skill, caster, target));
		}
	}

	[Package("laima")]
	[SkillHandler(SkillId.Mon_pcskill_FireFoxShikigami_Skill_2)]
	public class Onmyoji_FireFoxShikigamiFireAttackOverride : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (target == null || target.IsDead)
				return;

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
			skill.Run(FireFoxAttack.Execute(skill, caster, target));
		}
	}
}
