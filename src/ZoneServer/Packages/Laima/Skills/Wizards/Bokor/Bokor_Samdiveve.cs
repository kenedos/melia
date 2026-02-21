using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Samdiveve.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Samdiveve)]
	public class Bokor_SamdiveveOverride : IMeleeGroundSkillHandler
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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var targetPos = caster.Position.GetRelative(caster.Direction, 25);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Cleric_Samdiveve);
		}
	}
}
