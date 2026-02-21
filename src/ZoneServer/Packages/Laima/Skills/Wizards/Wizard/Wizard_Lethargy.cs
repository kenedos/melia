using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Wizards.Wizard
{
	/// <summary>
	/// Handler for the Wizard skill Lethargy.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wizard_Lethargy)]
	public class Wizard_LethargyOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int DebuffRadius = 50;
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the skill, debuffing enemies in target area.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] designatedTargets)
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

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, null);

			if (caster.TryGetAbility(AbilityId.Wizard29, out var buffAbility))
				caster.StartBuff(BuffId.Lethargy_Abil_Buff, skill.Level, buffAbility.Level, TimeSpan.FromMinutes(30), caster);

			var splashArea = new Circle(targetPos, DebuffRadius);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targets)
			{
				var abilityLevel = 0;
				if (caster.TryGetAbility(AbilityId.Wizard27, out var ability))
					abilityLevel = ability.Level;

				target.StartBuff(BuffId.Lethargy_Debuff, skill.Level, abilityLevel, TimeSpan.FromSeconds(10), caster);
			}
		}
	}
}
