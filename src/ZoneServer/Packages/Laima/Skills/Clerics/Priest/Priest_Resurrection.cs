using System;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Logging;
using System.Linq;
using Melia.Zone.World.Actors.Characters;
using Melia.Shared.Data.Database;

namespace Melia.Zone.Skills.Handlers.Priest
{
	/// <summary>
	/// Handler for the Priest skill Resurrection.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Priest_Resurrection)]
	public class ResurrectionOverride : IMeleeGroundSkillHandler
	{
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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var length = 110;
			var width = 60;
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var allies = caster.Map.GetDeadAlliedEntitiesIn(caster, splashArea);
			foreach (var target in allies)
			{
				if (target is Character player)
				{
					player.Resurrect(ResurrectOptions.TryAgain);
				}
			}
		}
	}
}
