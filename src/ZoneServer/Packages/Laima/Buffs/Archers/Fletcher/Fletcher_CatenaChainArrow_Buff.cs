using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Archers.Fletcher
{
	[Package("laima")]
	[BuffHandler(BuffId.Fletcher_CatenaChainArrow_Buff)]
	public class Fletcher_CatenaChainArrow_BuffOverride : BuffHandler
	{
		private static readonly SkillId[] FletcherSkills = new[]
		{
			SkillId.Fletcher_BodkinPoint,
			SkillId.Fletcher_BarbedArrow,
			SkillId.Fletcher_CrossFire,
			SkillId.Fletcher_Singijeon,
			SkillId.Fletcher_BodkinPoint_2,
			SkillId.Fletcher_BarbedArrow_2,
			SkillId.Fletcher_CrossFire_2,
			SkillId.Fletcher_Singijeon_2,
		};

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			InvalidateFletcherCooldowns(buff.Target);
			ReduceActiveFletcherCooldowns(buff);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Fletcher_CatenaChainArrow_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Fletcher_CatenaChainArrow_Buff, out var buff))
				return;

			if (skill.Data.Attribute == AttributeType.None || skill.Data.Attribute == AttributeType.Melee)
				modifier.AttackAttribute = AttributeType.Magic;
		}

		public override void OnEnd(Buff buff)
		{
			InvalidateFletcherCooldowns(buff.Target);
		}

		private static void ReduceActiveFletcherCooldowns(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var skillLevel = buff.NumArg1;
			var catenaReduction = 0.30f + 0.03f * skillLevel;

			if (character.TryGetActiveAbilityLevel(AbilityId.Fletcher37, out var fletcher37Level))
				catenaReduction *= 1f + 0.005f * fletcher37Level;

			catenaReduction = Math.Min(catenaReduction, 0.90f);

			var cooldownComponent = character.Components.Get<CooldownComponent>();
			if (cooldownComponent == null)
				return;

			foreach (var skillId in FletcherSkills)
			{
				if (!character.Skills.TryGet(skillId, out var skill))
					continue;

				var cdGroup = skill.Data.CooldownGroup;
				var remaining = cooldownComponent.GetRemain(cdGroup);
				if (remaining <= TimeSpan.Zero)
					continue;

				var baseCooldown = skill.Data.CooldownTime;
				var reducedCooldown = TimeSpan.FromTicks((long)(baseCooldown.Ticks * (1f - catenaReduction)));

				if (remaining > reducedCooldown)
				{
					cooldownComponent.ReduceCooldown(cdGroup, remaining - reducedCooldown);
				}
			}
		}

		private static void InvalidateFletcherCooldowns(ICombatEntity target)
		{
			if (target is not Character character)
				return;

			foreach (var skillId in FletcherSkills)
			{
				if (character.Skills.TryGet(skillId, out var skill))
				{
					if (skill.Properties.TryGet<CFloatProperty>(PropertyName.CoolDown, out var cdProp))
						cdProp.Invalidate();
				}
			}
		}
	}
}
