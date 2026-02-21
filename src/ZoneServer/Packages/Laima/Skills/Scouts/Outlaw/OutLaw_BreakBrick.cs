using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.OutLaw
{
	/// <summary>
	/// Handler for the Assassin skill Brick Smash
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.OutLaw_BreakBrick)]
	public class OutLaw_BreakBrickOverride : IMeleeGroundSkillHandler
	{
		public const float JumpDistance = 60f;

		/// <summary>
		/// Handles skill
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="designatedTarget"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] designatedTargets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetPos = caster.Position.GetRelative(caster.Direction, JumpDistance + 5);
			var area = new CircleF(targetPos, 45);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);

			skill.Run(this.Attack(skill, caster, area, farPos));
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="area"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, CircleF area, Position farPos)
		{
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;
			var hitDelay = TimeSpan.FromMilliseconds(400);

			// First perform the jump
			var targetPos = caster.Position.GetRelative(caster.Direction, JumpDistance);
			targetPos = caster.Map.Ground.GetLastValidPosition(caster.Position, targetPos);

			caster.Position = targetPos;
			Send.ZC_NORMAL.LeapJump(caster, targetPos, 0.15f, 0.1f, 0.1f, 0.2f, 0.1f, 3);

			// Outlaw4 has a 20% chance to activate.  If it does, it guarantees
			// the stun and makes the attack a forced critical
			var outlaw4Activates = caster.IsAbilityActive(AbilityId.Outlaw4) && RandomProvider.Get().Next(5) == 1;

			await skill.Wait(hitDelay);

			var hits = new List<SkillHitInfo>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(6);

				// 50% chance to Stun
				var stunChance = 5;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);

				if (skillHitResult.Damage > 0)
				{
					if (RandomProvider.Get().Next(10) <= stunChance)
						target.StartBuff(BuffId.Stun, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
				}
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
