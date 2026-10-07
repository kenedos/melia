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
	/// Handler for the Rune Caster skill Rune of Rock, which drops five
	/// giant rocks around the target location, one after another.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Stan)]
	public class RuneCaster_StanOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int RockCount = 5;
		private const int Spread = 100;
		private const float RockRadius = 40f;
		private const float FlyTime = 0.6f;
		private const float FallHeight = 200f;
		private static readonly TimeSpan RockInterval = TimeSpan.FromMilliseconds(500);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
				targetPos = farPos;

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

			var rockPositions = new List<Position>();
			for (var i = 0; i < RockCount; i++)
			{
				var rockPos = caster.Map.Ground.GetLastValidPosition(targetPos, targetPos.GetRandomInRange2D(Spread));
				caster.MissileFall(skill.Data.ClassName, "E_mannaz_stone", 2f, rockPos, RockRadius, (float)RockInterval.TotalSeconds * i, FlyTime, FallHeight, 2f, "F_ground185", 2f, 0f, "None", 1f);
				rockPositions.Add(rockPos);
			}

			skill.Run(this.Land(skill, caster, rockPositions));
		}

		/// <summary>
		/// Strikes the enemies under each rock as it lands.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="rockPositions"></param>
		/// <returns></returns>
		private async Task Land(Skill skill, ICombatEntity caster, List<Position> rockPositions)
		{
			await skill.Wait(TimeSpan.FromSeconds(FlyTime));

			for (var i = 0; i < rockPositions.Count; i++)
			{
				if (i > 0)
					await skill.Wait(RockInterval);

				if (caster.IsDead)
					return;

				var hits = new List<SkillHitInfo>();

				foreach (var hitTarget in caster.Map.GetAttackableEnemiesIn(caster, new Circle(rockPositions[i], RockRadius)).LimitBySDR(caster, skill))
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
