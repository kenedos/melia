using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Archers.Arquebusier;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Arquebuiser skill Dusty Salute.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Arquebusier_Salute)]
	public class Arquebusier_Salute : IGroundSkillHandler
	{
		private readonly static TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private readonly static TimeSpan ProjectileDelay = TimeSpan.FromMilliseconds(100);
		private readonly static TimeSpan ProjectileFlightTime = TimeSpan.FromMilliseconds(500);
		private const float SplashRadius = 75;
		private const float TargetCircleScale = 2.6f;

		/// <summary>
		/// Handles skill, applying a debuff to the target
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			// The salute lands where the player aimed, which is not
			// necessarily the far end of the skill's range
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
				targetPos = farPos;

			caster.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target, caster.Position, targetPos);
			Send.ZC_GROUND_EFFECT(caster, targetPos, "F_sys_target_pc", TargetCircleScale);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);

			var splashArea = new Circle(targetPos, SplashRadius);
			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes the actual attack in area.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			await skill.Wait(ProjectileDelay);
			Send.ZC_NORMAL.SkillProjectile(caster, splashArea.OriginPos, null, 0.3f, "E_archer_salute2", 1f, SplashRadius, ProjectileFlightTime, TimeSpan.Zero, 1000, 1, TimeSpan.Zero, 0, "None");

			await skill.Wait(ProjectileFlightTime);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{

				var skillHitResult = SCR_SkillHit(caster, target, skill);

				target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, target, skill, skillHitResult, skillHitResult.Result, DelayBetweenHits);
				Send.ZC_HIT_INFO(caster, target, hit);
				target.StartBuff(BuffId.Common_Slow, TimeSpan.FromSeconds(5));

				if (caster.IsAbilityActive(AbilityId.Arquebusier15))
				{
					skill.Run(this.DecreaseAccuracy(skill, target));
				}
			}

			Arquebusier_ArquebusBarrage.TryActivate(caster, splashArea.OriginPos);
		}

		/// <summary>
		/// Decrease the accuracy of the target.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="target"></param>
		private async Task DecreaseAccuracy(Skill skill, ICombatEntity target)
		{
			target.Properties.Modify(PropertyName.DR_BM, -10f);
			await skill.Wait(TimeSpan.FromSeconds(3));
			target.Properties.Modify(PropertyName.DR_BM, 10f);
		}
	}
}
