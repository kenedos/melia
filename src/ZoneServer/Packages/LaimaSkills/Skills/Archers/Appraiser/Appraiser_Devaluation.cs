using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Appraiser skill Devaluation, which lowers the
	/// defense of the enemy in front of the Appraiser.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Appraiser_Devaluation)]
	public class Appraiser_DevaluationOverride : IGroundSkillHandler
	{
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

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var devalued = target != null && !target.IsDead ? target : caster.Map.GetAttackableEnemiesIn(caster, splashArea).FirstOrDefault();
			devalued?.StartBuff(BuffId.Devaluation_Debuff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
		}
	}
}
