using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Rogue skill Feint.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rogue_Feint)]
	public class Rogue_FeintOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 25, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Apply(skill, caster, splashArea));
		}

		private async Task Apply(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(50));

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var duration = TimeSpan.FromSeconds(5);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				target.StartBuff(BuffId.Feint_Debuff, skill.Level, 0, duration, caster);
			}
		}
	}
}
