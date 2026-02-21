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

namespace Melia.Zone.Skills.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handler for the Paladin skill Resist Elements.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Paladin_ResistElements)]
	public class Paladin_ResistElementsOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int BuffDurationSeconds = 300;
		private const int BuffRange = 300;

		/// <summary>
		/// Start casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			// Apply buff to caster first
			caster.StartBuff(BuffId.ResistElements_Buff, skill.Level, 0f, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

			// Find and buff party members in range
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
						member.StartBuff(BuffId.ResistElements_Buff, skill.Level, 0f, TimeSpan.FromSeconds(BuffDurationSeconds), caster);
					}
				}
			}
		}
	}
}
