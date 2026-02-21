using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Wizards.Chronomancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Chronomancer_Pass)]
	public class Chronomancer_PassOverride : IMeleeGroundSkillHandler
	{
		private const float BuffRange = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (caster.IsBuffActive(BuffId.Pass_Debuff))
			{
				caster.ServerMessage(Localization.Get("Time Travel Sickness prevents using Pass."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var skillLevel = skill.Level;
			var buffDuration = TimeSpan.FromSeconds(5);
			var sicknessDuration = TimeSpan.FromSeconds(30 + skillLevel * 2);

			Send.ZC_SYNC_START(caster, skillHandle, 1);
			caster.StartBuff(BuffId.Pass_Buff, skillLevel, 0, buffDuration, caster);
			caster.StartBuff(BuffId.Pass_Debuff, skillLevel, 0, sicknessDuration, caster);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(100));

			if (caster is Character character)
			{
				var party = character.Connection.Party;
				if (party != null)
				{
					var members = caster.Map.GetPartyMembersInRange(character, BuffRange, true);
					foreach (var member in members)
					{
						if (member == caster)
							continue;

						if (member.IsBuffActive(BuffId.Pass_Debuff))
							continue;

						member.StartBuff(BuffId.Pass_Buff, skillLevel, 0, buffDuration, caster);
						member.StartBuff(BuffId.Pass_Debuff, skillLevel, 0, sicknessDuration, caster);
					}
				}
			}
		}
	}
}
