using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[SkillHandler(SkillId.Thaumaturge_Quicken)]
	public class Thaumaturge_QuickenOverride : IMeleeGroundSkillHandler
	{
		private const float BuffRange = 300;
		private const int BuffDurationSeconds = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets?.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var duration = TimeSpan.FromSeconds(BuffDurationSeconds);

			caster.StartBuff(BuffId.Quicken_Buff, skill.Level, 0f, duration, caster);

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
						member.StartBuff(BuffId.Quicken_Buff, skill.Level, 0f, duration, caster);
					}
				}
			}
		}
	}
}
