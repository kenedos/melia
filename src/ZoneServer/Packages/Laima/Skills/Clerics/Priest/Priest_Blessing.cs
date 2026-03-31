using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Priest
{
	/// <summary>
	/// Handler for the Priest skill Blessing.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Priest_Blessing)]
	public class Priest_BlessingOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int BuffDurationSeconds = 300;
		private const int BuffRange = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			caster.StartBuff(BuffId.Blessing_Buff, skill.Level, 0f, TimeSpan.FromSeconds(BuffDurationSeconds), caster, skill.Id);

			if (caster is Character character)
			{
				var party = character.Connection.Party;
				var members = caster.Map.GetPartyMembersInRange(character, BuffRange, true);

				if (party != null)
				{
					foreach (var member in members)
					{
						if (member == caster)
							continue;
						member.StartBuff(BuffId.Blessing_Buff, skill.Level, 0f, TimeSpan.FromSeconds(BuffDurationSeconds), caster, skill.Id);
					}
				}
			}
		}
	}
}
