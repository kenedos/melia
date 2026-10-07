using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for the Nak Muay's basic attacks in the Ram Muay stance,
	/// which strike twice and set off Muay Thai: Muay Boran.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.NakMuay_Attack, SkillId.NakMuay_Attack2)]
	public class NakMuay_AttackOverride : IMeleeGroundSkillHandler
	{
		private const int HitCount = 2;
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(330);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, 0, null, includeCaster: false);

			skill.Run(this.Attack(skill, caster, targets));
		}

		/// <summary>
		/// Strikes the targets once the swing lands.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private async Task Attack(Skill skill, ICombatEntity caster, IList<ICombatEntity> targets)
		{
			var speedRate = skill.Properties.GetFloat(PropertyName.SklSpdRate);
			var skillHitDelay = TimeSpan.FromMilliseconds(skill.Properties.HitDelay.TotalMilliseconds / speedRate);
			var aniTime = TimeSpan.FromMilliseconds(AniTime.TotalMilliseconds / speedRate / HitCount);

			await skill.Wait(skillHitDelay);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				if (target == null)
					continue;

				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitCount));
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);
				SkillDamageHelper.ApplyExtraLines(skillHit);

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			if (targets.Count > 0 && targets[0] != null)
				NakMuaySkillHelper.TryMuayBoran(caster, targets[0]);
		}
	}
}
