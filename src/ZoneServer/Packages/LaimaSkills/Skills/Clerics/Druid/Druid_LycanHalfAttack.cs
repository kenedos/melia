using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the scratch of Lycanthropy: Human Form, the hybrid's
	/// basic attack, which has a 5% chance to make the target bleed.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Lycan_Half_Attack)]
	public class Druid_LycanHalfAttackOverride : IMeleeGroundSkillHandler
	{
		private const int BleedingChance = 5;
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(330);
		private static readonly TimeSpan BleedingDuration = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, 0, null, includeCaster: false);

			skill.Run(this.Scratch(skill, caster, targets));
		}

		/// <summary>
		/// Scratches the targets once the swing lands.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private async Task Scratch(Skill skill, ICombatEntity caster, IList<ICombatEntity> targets)
		{
			var speedRate = skill.Properties.GetFloat(PropertyName.SklSpdRate);
			var skillHitDelay = TimeSpan.FromMilliseconds(skill.Properties.HitDelay.TotalMilliseconds / speedRate);
			var aniTime = TimeSpan.FromMilliseconds(AniTime.TotalMilliseconds / speedRate);

			await skill.Wait(skillHitDelay);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				if (target == null)
					continue;

				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);
				SkillDamageHelper.ApplyExtraLines(skillHit);

				hits.Add(skillHit);

				if (skillHitResult.Damage > 0 && GameRandom.Get().Next(100) < BleedingChance)
					target.StartBuff(BuffId.UC_bleed, 1, skillHitResult.Damage * 0.1f, BleedingDuration, caster, skill.Id);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
