using System;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs.Handlers.Swordsmen.Barbarian;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the Barbarian skill Embowel.
	/// Dual-hit single packet attack that knocks down all targets hit.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_Embowel)]
	public class Barbarian_EmbowelOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Handles the Embowel skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 40, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aniTime = TimeSpan.FromMilliseconds(175);

			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var t in targetList.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;
				var skillHitResult = SCR_SkillHit(caster, t, skill, modifier);

				t.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, t, skill, skillHitResult, aniTime, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			foreach (var hit in hits)
			{
				if (hit.HitInfo.Damage > 0 && hit.Target.IsKnockdownable())
				{
					hit.KnockBackInfo = new KnockBackInfo(caster, hit.Target, KnockBackType.KnockDown, 180, 80, KnockDirection.CasterForward);
					hit.HitInfo.KnockBackType = KnockBackType.KnockDown;
					hit.Target.ApplyKnockdown(caster, skill, hit);
				}
			}

			caster.StartBuff(BuffId.Embowel_Buff, skill.Level, 0, TimeSpan.FromSeconds(6), caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
