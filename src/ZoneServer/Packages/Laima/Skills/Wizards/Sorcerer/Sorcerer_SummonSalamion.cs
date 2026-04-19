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
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Summon Salamion.
	/// Summons a fire spirit that heals the sorcerer and their summons.
	/// </summary>
	/// <remarks>
	/// Salamion:
	/// - Has MHP based on caster's MHP: MHP * (2.2 + skillLevel * 0.3)
	/// - Lasts for 300 seconds (5 minutes)
	/// - With Sorcerer17 ability: Periodically heals the summoner and other summons
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_SummonSalamion)]
	public class Sorcerer_SummonSalamionOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Salamion lifetime in seconds.
		/// </summary>
		private const int SummonLifetimeSeconds = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

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

			skill.Run(this.HandleSkill(character, skill, originPos, farPos));
		}

		private async Task HandleSkill(Character character, Skill skill, Position originPos, Position farPos)
		{
			// Kill any existing Salamion first
			KillExistingSalamons(character);

			await skill.Wait(TimeSpan.FromMilliseconds(1300));

			// Calculate spawn position
			var spawnPos = originPos.GetRelative(farPos, distance: 20f);

			// Create the Salamion summon
			var summon = new Summon(character, (int)MonsterId.Salamion, RelationType.Friendly);

			// Set custom name format
			summon.Name = $"!@#${{Auto_1}}_of_{{Auto_2}}$*$Auto_1$*${character.Name}$*$Auto_2$*$@dicID_^*$ETC_20150317_000235$*^#@!";

			// Set up summon properties
			summon.Position = spawnPos;
			summon.Direction = character.Direction;
			summon.Map = character.Map;
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;
			summon.Level = character.Level;

			// Mark as sorcerer summon (Salamion type)
			summon.Vars.SetInt("SORCERER_SUMMONSALOON", 1);
			summon.Vars.SetInt("SORCERER_MON", 1);

			// Calculate MHP based on caster's MHP
			var casterMHP = character.Properties.GetFloat(PropertyName.MHP) - character.Properties.GetFloat(PropertyName.MHP_BM);
			var summonMHP = casterMHP * (2.2f + (skill.Level * 0.3f));
			summon.Vars.SetFloat("SUMMON_SET_MHP_BY_CASTER", summonMHP);

			// Calculate stats
			CalculateSummonStats(summon, character, skill);

			// Set movement speed
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.FIXMSPD_BM, 80f);
			summon.InvalidateProperties();

			// Add lifetime component
			summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromSeconds(SummonLifetimeSeconds)));

			// Activate the summon
			summon.SetState(true);

			// Add to character's summon list
			character.Summons.AddSummon(summon);

			// Store handle reference on character for quick lookup
			character.Variables.Temp.SetInt("SORCERER_SUMMONSALOON", summon.Handle);

			// Apply PC_Summon buff
			summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);

			// Check for Sorcerer17 ability - enables healing buff
			if (character.IsAbilityActive(AbilityId.Sorcerer17))
			{
				// Apply the healing buff to Salamion
				var buffDuration = TimeSpan.FromSeconds(SummonLifetimeSeconds);
				summon.StartBuff(BuffId.SummonSalamion_Buff, buffDuration, character);
			}

			// Play summon animation
			Send.ZC_NORMAL.SummonPlayAnimation(character, "SORCERER_SUMMONSALOON", 1);

			// Send update to client
			Send.ZC_MSPD(character, summon, 0, summon.Properties.GetFloat(PropertyName.MSPD));
		}

		/// <summary>
		/// Calculates and sets the summon's stats based on skill level.
		/// </summary>
		private void CalculateSummonStats(Summon summon, Character caster, Skill skill)
		{
			// Salamion has minimal attack stats but focuses on support
			summon.Properties.Overrides.SetFloat(PropertyName.MINPATK, 40f);
			summon.Properties.Overrides.SetFloat(PropertyName.MAXPATK, 50f);
			summon.Properties.Overrides.SetFloat(PropertyName.MINMATK, 40f);
			summon.Properties.Overrides.SetFloat(PropertyName.MAXMATK, 50f);
		}

		/// <summary>
		/// Kills any existing Salamion summons.
		/// </summary>
		private void KillExistingSalamons(Character character)
		{
			var existingSalamons = character.Summons.GetSummons(s =>
				s.Vars.TryGetInt("SORCERER_SUMMONSALOON", out var value) && value == 1);

			foreach (var summon in existingSalamons)
			{
				summon.Kill(character);
			}

			// Clear the stored handle
			character.Variables.Temp.Remove("SORCERER_SUMMONSALOON");
		}
	}
}
