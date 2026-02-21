using System.Collections.Generic;
using System;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using System.Linq;
using Melia.Zone.Skills.SplashAreas;
using System.Threading.Tasks;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Priest
{
	/// <summary>
	/// Handler for the Priest skill Turn Undead.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Priest_TurnUndead)]
	public class Priest_TurnUndeadOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Start casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 200;
			var damageDelay = 1000;

			var hits = new List<SkillHitInfo>();
			await this.SkillAttackWithInstantKill(caster, skill, splashArea, hitDelay, damageDelay, hits);
		}

		/// <summary>
		/// Performs the skill attack with a chance to instantly kill targets
		/// based on damage dealt. Only hits demon-race enemies.
		/// </summary>
		private async Task SkillAttackWithInstantKill(ICombatEntity caster, Skill skill, ISplashArea splashArea, int hitDelay, int damageDelay, List<SkillHitInfo> hits)
		{
			var rng = new Random();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).ToList();

			var targetCount = skill.Level + 3;
			foreach (var target in targets)
			{
				if (target.Race != RaceType.Velnias)
					continue;

				if (targetCount == 0)
					break;

				var skillHitResult = SCR_SkillHit(caster, target, skill);

				var killChance = (skillHitResult.Damage / target.MaxHp) * 100;
				if (rng.Next(100) < killChance)
				{
					skillHitResult.Damage = target.MaxHp + skillHitResult.Damage;
					target.Kill(caster);
				}
				else
					target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(damageDelay), TimeSpan.FromMilliseconds(hitDelay));
				hits.Add(hit);

				targetCount--;
			}

			await skill.Wait(TimeSpan.FromMilliseconds(hitDelay));
			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
