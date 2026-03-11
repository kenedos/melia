using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Wizards.Chronomancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Chronomancer_QuickCast)]
	public class Chronomancer_QuickCastOverride : ISelfSkillHandler
	{
		private const float BuffRange = 300;
		private const int BuffDurationSeconds = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			var duration = TimeSpan.FromSeconds(BuffDurationSeconds);

			caster.StartBuff(BuffId.QuickCast_Buff, skill.Level, 0f, duration, caster);

			if (caster is Character character)
			{
				var members = character.GetPartyMembersInRange(BuffRange);
				foreach (var member in members)
				{
					if (member == caster)
						continue;
					member.StartBuff(BuffId.QuickCast_Buff, skill.Level, 0f, duration, caster);
				}
			}
		}
	}
}
