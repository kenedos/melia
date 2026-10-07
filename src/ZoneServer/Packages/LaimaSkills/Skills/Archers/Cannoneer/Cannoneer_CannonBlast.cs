using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for the Cannoneer skill Cannon Blast, a 1.5 second channel
	/// that fires the cannon ahead twice, knocking enemies back, while the
	/// Cannoneer can't be knocked back or down.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Cannoneer_CannonBlast)]
	public class Cannoneer_CannonBlastOverride : IDynamicCasted
	{
		private const int Shots = 2;
		private const int HitsPerShot = 2;
		private const float BlastLength = 130f;
		private const float BlastAngle = 60f;
		private const int KnockBackVelocity = 60;
		private static readonly TimeSpan ShotInterval = TimeSpan.FromMilliseconds(700);
		private static readonly TimeSpan ChannelDuration = TimeSpan.FromMilliseconds(1500);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.StartBuff(BuffId.Skill_SuperArmor_Buff, ChannelDuration);

			skill.Run(this.Fire(skill, caster));
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.Skill_SuperArmor_Buff);
		}

		/// <summary>
		/// Fires the cannon ahead of the caster for as long as they keep
		/// channeling.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Fire(Skill skill, ICombatEntity caster)
		{
			for (var shot = 0; shot < Shots; shot++)
			{
				await skill.Wait(ShotInterval);

				if (caster.IsDead || !caster.IsCasting(skill))
					return;

				var area = new Fan(caster.Position, caster.Direction, BlastLength, BlastAngle);
				var hits = new List<SkillHitInfo>();

				foreach (var target in caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill))
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitsPerShot));
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);

					if (skillHitResult.Damage > 0 && target.IsKnockdownable())
					{
						skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, KnockBackType.KnockBack, KnockBackVelocity, 10);
						skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
						target.ApplyKnockback(caster, skill, skillHit);
					}

					hits.Add(skillHit);
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
