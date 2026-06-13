using System;
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

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Arquebuiser skill Dusty Salute.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Arquebusier_Salute)]
	public class Arquebusier_Salute : IGroundSkillHandler, IDynamicCasted
	{
		private readonly static TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private const float SplashRadius = 75;

		/// <summary>
		/// Called when the user starts casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

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

			caster.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_NORMAL.UpdateSkillEffect(caster, target, farPos, target.Position);
			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);
			Send.ZC_NORMAL.SkillProjectile(caster, farPos, "", 0.3f, "F_ground226", 1f, 75, TimeSpan.FromSeconds(0.5f), TimeSpan.FromSeconds(0), 1000, 1, TimeSpan.FromSeconds(0), 0, "None");
			var splashArea = new Circle(farPos, SplashRadius);
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
			await skill.Wait(TimeSpan.FromMilliseconds(500));

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
