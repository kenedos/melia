using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Kriwi
{
	/// <summary>
	/// Handler for the Kriwi skill Divine Stigma.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Kriwi_DivineStigma)]
	public class Krivis_DivineStigmaOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int DebuffDurationMilliseconds = 8000;

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
		/// Handles skill behavior
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

			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos, targets));
		}
		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			var target = targets.FirstOrDefault();
			var targetPos = target?.Position ?? originPos.GetRelative(farPos, distance: 50);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 50f, 5));
			targets = caster.GetTargets();
			await skill.Wait(TimeSpan.FromMilliseconds(550));

			var targetCount = (skill.Level / 2) + 2;
			foreach (var currentTarget in targets)
			{
				if (targetCount == 0)
					break;

				var splashHitResult = SCR_SkillHit(caster, currentTarget, skill);

				if (splashHitResult.Damage <= 0)
					continue;

				var duration = DebuffDurationMilliseconds;

				if (caster is Character character && character.TryGetActiveAbilityLevel(AbilityId.Kriwi19, out var abilLv))
				{
					// +1 second per ability level against demons
					if (currentTarget.Race == RaceType.Velnias)
						duration += 1000 * abilLv;
				}

				Send.ZC_NORMAL.SkillTargetAttachForce(caster, currentTarget, TimeSpan.FromSeconds(0.25), 1, "I_cleric_devinestigma_force_dark#Dummy_effect_lada", 0.5f, EffectLocation.Top);
				await skill.Wait(TimeSpan.FromMilliseconds(150));
				var stigmaDamageIncrease = 0.3f;
				currentTarget.StartBuff(BuffId.DivineStigma_Debuff, skill.Level, stigmaDamageIncrease, TimeSpan.FromMilliseconds(duration), caster);
				currentTarget.StartBuff(BuffId.Fire, skill.Level, splashHitResult.Damage, TimeSpan.FromMilliseconds(duration), caster);

				targetCount--;
			}
		}
	}
}
