using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for the Pied Piper skill Wiegenlied, which lulls the enemies
	/// around the Pied Piper to sleep and calls a mouse.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PiedPiper_Wiegenlied)]
	public class PiedPiper_WiegenliedOverride : IGroundSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			PiedPiperSkillHelper.PlayWiegenlied(caster, skill);
			PiedPiperSkillHelper.SummonMouse(caster);
		}
	}
}
