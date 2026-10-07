using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for the Rune Caster skill Rune of Destruction, an explosion
	/// at the target location that strikes the enemies in it 5 times.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Hagalaz)]
	public class RuneCaster_HagalazOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Radius = 75f;
		private const int HitCount = 5;
		private static readonly TimeSpan ExplosionDelay = TimeSpan.FromMilliseconds(300);
		private static readonly TimeSpan HitInterval = TimeSpan.FromMilliseconds(100);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.InSkillUseRange(skill, targetPos))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			RuneCasterSkillHelper.ApplySkilledCasting(caster);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Explode(skill, caster, targetPos));
		}

		/// <summary>
		/// Detonates the rune at the target position.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <returns></returns>
		private async Task Explode(Skill skill, ICombatEntity caster, Position targetPos)
		{
			await skill.Wait(ExplosionDelay);

			if (caster.IsDead)
				return;

			Send.ZC_GROUND_EFFECT(caster, targetPos, "F_wizard_Hagalaz_explosion", 1f, 0f, 0f);

			for (var i = 0; i < HitCount; i++)
			{
				if (i > 0)
					await skill.Wait(HitInterval);

				if (caster.IsDead)
					return;

				var hits = new List<SkillHitInfo>();

				foreach (var hitTarget in caster.Map.GetAttackableEnemiesIn(caster, new Circle(targetPos, Radius)).LimitBySDR(caster, skill))
				{
					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
