using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for the Shinobi skill Mokuton no Jutsu, which leaves a log
	/// behind and hides the Shinobi, faster and sturdier while hidden.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Shinobi_Mokuton_no_jutsu)]
	public class Shinobi_MokutonNoJutsuOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(10);

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

			caster.StartBuff(BuffId.ShinobiCloaking_Buff, skill.Level, 0, Duration, caster, skill.Id);
			caster.StartBuff(BuffId.Mokuton_no_jutsu, skill.Level, 0, Duration, caster, skill.Id);
		}
	}
}
