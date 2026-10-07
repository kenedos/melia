using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for Fumigate, which marks a cured ally, or with Fumigate:
	/// Perfusion sprays vapor around the caster every second.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: 1 while it is Fumigate: Perfusion's vapor
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Fumigate_Buff)]
	public class PlagueDoctor_Fumigate_BuffOverride : BuffHandler
	{
		private const int PerfusionInterval = 1000;
		private const float PerfusionRange = 80f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.NumArg2 == 1)
				buff.SetUpdateTime(PerfusionInterval);
		}

		public override void WhileActive(Buff buff)
		{
			var caster = buff.Target;

			if (buff.NumArg2 != 1 || caster.IsDead || !caster.TryGetSkill(SkillId.PlagueDoctor_Fumigate, out var skill))
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			var hits = new List<SkillHitInfo>();

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, PerfusionRange).Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

				target.Components.Get<BuffComponent>()?.RemoveRandomBuff();
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}

	/// <summary>
	/// Handler for Fumigate: Purification, which raises poison resistance
	/// inside the purified area and gives a 50% chance to resist removable
	/// debuffs.
	/// </summary>
	/// <remarks>
	/// NumArg1: Ability level
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Fumigate_Buff_ResAbil)]
	public class PlagueDoctor_Fumigate_Buff_ResAbilOverride : BuffHandler
	{
		private const float PoisonResistanceRatePerLevel = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var poisonResistance = buff.Target.Properties.GetFloat(PropertyName.ResPoison);
			AddPropertyModifier(buff, buff.Target, PropertyName.ResPoison_BM, poisonResistance * PoisonResistanceRatePerLevel * buff.NumArg1);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.ResPoison_BM);
		}
	}
}
