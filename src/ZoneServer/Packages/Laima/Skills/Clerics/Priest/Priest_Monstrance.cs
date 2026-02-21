using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Priest
{
	/// <summary>
	/// Handler for the Priest skill Monstrance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Priest_Monstrance)]
	public class Priest_MonstranceOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int DebuffRadius = 150;
		private const int DebuffDurationMilliseconds = 20000;
		private const int BaseTargetCount = 3;
		private const int TargetCountPerLevel = 1;
		private const float BaseDamageRate = 0.2f;
		private const float DamageRatePerLevel = 0.01f;
		private const float AbilityBonus = 0.005f;

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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, originPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));

			// Find targets
			var targetPos = originPos.GetRelative(caster.Direction, distance: 100);
			var splashArea = new Circle(targetPos, DebuffRadius);
			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			// Calculate damage
			var damageBonus = this.CalculateDamageBonus(caster, skill);
			var targetCount = BaseTargetCount + (skill.Level * TargetCountPerLevel);

			// Prioritize targets that don't have the debuff active
			var prioritizedTargets = targetList
				.OrderByDescending(t => !t.IsBuffActive(BuffId.Monstrance_Debuff))
				.Take(targetCount);

			foreach (var target in prioritizedTargets)
			{
				target.StartBuff(BuffId.Monstrance_Debuff, skill.Level, damageBonus, TimeSpan.FromMilliseconds(DebuffDurationMilliseconds), caster);
				await skill.Wait(TimeSpan.FromMilliseconds(50));
			}
		}

		/// <summary>
		/// Calculates the damage bonus for the Monstrance skill.
		/// </summary>
		private float CalculateDamageBonus(ICombatEntity caster, Skill skill)
		{
			var damageBonus = BaseDamageRate + (DamageRatePerLevel * skill.Level);
			var byAbility = 1f;

			if (caster.TryGetActiveAbilityLevel(AbilityId.Priest27, out var abilityLevel))
				byAbility += abilityLevel * AbilityBonus;

			return damageBonus * byAbility;
		}
	}
}
