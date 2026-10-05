using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Snipers Serenity.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_SnipersSerenity)]
	public class Musketeer_SnipersSerenityOverride : ISelfSkillHandler, IDynamicCasted
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
			// TODO: Implement Additional SP consumption
			if (caster.IsBuffActive(BuffId.SnipersSerenity_Buff))
				caster.RemoveBuff(BuffId.SnipersSerenity_Buff);
			else if (!caster.IsJumping())
				caster.StartBuff(BuffId.SnipersSerenity_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(30000f), caster, skill.Id);
		}
	}
}
