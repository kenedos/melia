using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Plague Doctor skill Modafinil, which raises the
	/// movement speed of the caster and their party.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PlagueDoctor_Modafinil)]
	public class PlagueDoctor_ModafinilOverride : IGroundSkillHandler
	{
		private const float BuffRange = 250f;

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

			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, BuffRange))
				ally.StartBuff(BuffId.Modafinil_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
		}
	}
}
