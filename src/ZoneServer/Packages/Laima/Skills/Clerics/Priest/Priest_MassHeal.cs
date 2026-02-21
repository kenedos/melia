using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.Scripting;

namespace Melia.Zone.Skills.Handlers.Priest
{
	/// <summary>
	/// Handler for the Priest skill Mass Heal.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Priest_MassHeal)]
	public class MassHealOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float HealRadius = 180f;

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

		/// <summary>
		/// Handles skill
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, caster.Position, caster.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var targetCount = (skill.Level / 2) + 4;

			var targetPos = caster.Position;

			var splashArea = new Circle(targetPos, HealRadius);
			var hits = new List<SkillHitInfo>();
			var targetList = caster.Map.GetAliveAlliedEntitiesIn(caster, splashArea);
			targetList.Add(caster);

			// Prioritize Characters and sort by lowest HP
			var prioritizedTargets = targetList
				.OrderByDescending(t => t is Character)
				.ThenBy(t => t.Hp / (float)t.MaxHp)
				.Take(targetCount);

			foreach (var target in prioritizedTargets)
			{
				var healAmount = this.CalculateHealAmount(caster, target, skill);

				target.StartBuff(BuffId.MassHeal_Buff, skill.Level, healAmount, TimeSpan.FromMilliseconds(1), caster);

				if (caster is Character character && character.TryGetActiveAbilityLevel(AbilityId.Cleric22, out var abilLv))
				{
					var healOverTimeAmount = abilLv * 0.05f * healAmount / 10;
					target.StartBuff(BuffId.MassHeal_Dot_Buff, 1, healOverTimeAmount, TimeSpan.FromMilliseconds(10000), caster);
				}
			}
		}

		/// <summary>
		/// Calculates the heal amount for the target.
		/// </summary>
		private float CalculateHealAmount(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var healAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			return healAmount;
		}
	}
}
