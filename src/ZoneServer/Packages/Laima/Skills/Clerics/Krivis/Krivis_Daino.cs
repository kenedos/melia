using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Kriwi
{
	/// <summary>
	/// Handler for the Kriwi skill Daino.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Kriwi_Daino)]
	public class Krivis_DainoOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int BuffDurationSeconds = 300;
		private const int BuffRange = 300;
		private const float AbilityBonus = 0.005f;

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

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill));
		}
		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(590));

			var healBonus = 0.30f + 0.03f * skill.Level;

			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Kriwi32, out var level))
				byAbility += level * AbilityBonus;
			healBonus *= byAbility;

			caster.StartBuff(BuffId.Daino_Buff, skill.Level, healBonus, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

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

						member.StartBuff(BuffId.Daino_Buff, skill.Level, healBonus, TimeSpan.FromSeconds(BuffDurationSeconds), caster);
					}
				}
			}
		}
	}
}
