using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for the Cannoneer skill Sweeping Cannon, which mounts the
	/// cannon on a turret and sweeps a long line ahead four times.
	/// </summary>
	/// <remarks>
	/// [Arts] Sweeping Cannon: Siege Shot also destroys the enemies' ground
	/// effects in the line.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Cannoneer_SweepingCannon)]
	public class Cannoneer_SweepingCannonOverride : IGroundSkillHandler
	{
		private const int HitCount = 4;
		private const float Length = 250f;
		private const float Width = 75f;
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(800);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Sweep(skill, caster));
		}

		/// <summary>
		/// Sweeps the line ahead of the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Sweep(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(HitDelay);

			if (caster.IsDead)
				return;

			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var hits = new List<SkillHitInfo>();

			foreach (var target in caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitCount));
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			if (!caster.IsAbilityActive(AbilityId.Cannoneer25))
				return;

			foreach (var pad in caster.Map.GetPads(a => a.Creator is ICombatEntity creator && caster.IsEnemy(creator) && area.IsInside(a.Position)))
				pad.Destroy();
		}
	}
}
