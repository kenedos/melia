using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Increase Magic DEF.
	/// Temporarily increases the Magic Defense of caster and party members.
	/// The increase applies proportionally to the caster's SPR.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_IncreaseMagicDEF)]
	public class Pardoner_IncreaseMagicDEFOverride : IGroundSkillHandler
	{
		private const float BuffDurationMs = 1800000f; // 30 minutes
		private const float BuffRange = 150f;
		private const int MaxTargets = 50;

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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var buffDuration = TimeSpan.FromMilliseconds(BuffDurationMs);

			// NumArg1 = skill level for calculating MDEF bonus
			// NumArg2 = caster's SPR at time of casting for buff calculation
			var casterSpr = caster.Properties.GetFloat(PropertyName.MNA);

			// Apply buff to caster first
			caster.StartBuff(BuffId.IncreaseMagicDEF_Buff, skill.Level, casterSpr, buffDuration, caster);

			// Find and buff party members in range
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

						member.StartBuff(BuffId.IncreaseMagicDEF_Buff, skill.Level, casterSpr, buffDuration, caster);
					}
				}
			}
		}
	}
}
