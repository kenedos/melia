using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Priest
{
	/// <summary>
	/// Handler for the Priest skill Revive.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Priest_Revive)]
	public class Priest_ReviveOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float BuffRange = 250f;
		private const int BuffDurationSeconds = 300;

		/// <summary>
		/// Starts the dynamic casting of the skill.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Ends the dynamic casting of the skill.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the execution of the Revive skill.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var buffDuration = TimeSpan.FromSeconds(BuffDurationSeconds);
			var healAmount = this.CalculateHealAmount(caster, caster, skill);

			caster.StartBuff(BuffId.Cleric_Revival_Buff, skill.Level, healAmount, buffDuration, caster);

			// Buff party members
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

						healAmount = this.CalculateHealAmount(caster, member, skill);
						member.StartBuff(BuffId.Cleric_Revival_Buff, skill.Level, healAmount, buffDuration, caster);
					}
				}
			}
		}

		/// <summary>
		/// Calculates the heal amount for the target.
		/// </summary>
		private float CalculateHealAmount(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var healAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			return healAmount;
		}
	}
}
