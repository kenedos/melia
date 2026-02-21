using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Chronomancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Chronomancer_BackMasking)]
	public class Chronomancer_BackmaskingOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if (caster.Map.IsPVP)
			{
				caster.ServerMessage(Localization.Get("This skill cannot be used in PVP areas."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			this.RefreshFriendlyPads(caster, skill, farPos);
		}

		private void RefreshFriendlyPads(ICombatEntity caster, Skill skill, Position position)
		{
			var range = skill.Data.SplashRange * 4;

			var pads = caster.Map.GetPads(pad =>
				pad.Creator is ICombatEntity padCreator
				&& !padCreator.IsEnemy(caster)
				&& pad != null
				&& !pad.IsDead
				&& pad.Position.Get2DDistance(position) <= range
			);

			foreach (var pad in pads)
			{
				pad.Trigger.ResetLifeTime();
			}
		}
	}
}
