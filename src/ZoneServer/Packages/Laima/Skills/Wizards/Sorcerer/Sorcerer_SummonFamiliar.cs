using System;
using System.Linq;
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
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Summon Familiar.
	/// Summons 5 bat familiars that attack enemies in a kamikaze fashion.
	/// </summary>
	/// <remarks>
	/// Familiars:
	/// - 5 bats are summoned around the caster
	/// - Last for 60 seconds or until they attack
	/// - Attack by flying at enemies and exploding
	/// - With Sorcerer1 ability: Can deal splash damage on attack
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_SummonFamiliar)]
	public class Sorcerer_SummonFamiliarOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Number of bats to summon.
		/// </summary>
		private const int BatCount = 5;

		/// <summary>
		/// Lifetime of the bats in seconds.
		/// </summary>
		private const int BatLifetimeSeconds = 60;

		/// <summary>
		/// Handle Skill Behavior
		/// </summary>
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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SYNC_START(caster, skillHandle, 1);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, skill.Data.DefaultHitDelay);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			// Create the bats
			CreateFamiliarBats(character, skill);

			// Start the buff that manages bat lifecycle
			character.StartBuff(BuffId.sorcerer_bat, TimeSpan.FromSeconds(BatLifetimeSeconds));
		}

		/// <summary>
		/// Creates the familiar bats around the caster.
		/// </summary>
		private void CreateFamiliarBats(Character character, Skill skill)
		{
			var random = RandomProvider.Get();

			for (var i = 0; i < BatCount; i++)
			{
				// Create summon
				var summon = new Summon(character, (int)MonsterId.Familiar, RelationType.Friendly);

				// Position randomly around caster
				var spawnPos = character.Position.GetRandomInRange2D(20, random);

				summon.Position = spawnPos;
				summon.Direction = character.Direction;
				summon.Map = character.Map;
				summon.OwnerHandle = character.Handle;
				summon.Faction = FactionType.Law;

				// Set movement speed
				summon.Properties.SetFloat(PropertyName.FIXMSPD_BM, 80f);

				// Mark as familiar bat
				summon.Vars.SetInt("SORCERER_BAT_STOP", 0);

				// Add lifetime component
				summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromSeconds(BatLifetimeSeconds)));

				// Activate summon with special AI for bats
				summon.SetState(true);

				// Enable AI even when no players nearby
				summon.Vars.SetBool("EnableAIOutOfPC", true);

				// Hold script briefly on spawn
				HoldSummonMovement(skill, summon, 800);

				// Add to character's summon list
				character.Summons.AddSummon(summon);

				// Apply the PC_Summon buff
				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
				Send.ZC_SYNC_START(character, skillHandle, 1);
				Send.ZC_MSPD(character, summon, 0, summon.Properties.GetFloat(PropertyName.MSPD));
				summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);
				Send.ZC_SYNC_END(character, skillHandle, 0);
				Send.ZC_SYNC_EXEC_BY_SKILL_TIME(character, skillHandle, skill.Data.DefaultHitDelay);
			}
		}

		/// <summary>
		/// Temporarily prevents the summon from moving.
		/// </summary>
		private void HoldSummonMovement(Skill skill, Summon summon, int durationMs)
		{
			// Set a flag that the AI can check to prevent movement
			summon.Vars.SetBool("HoldMovement", true);

			// Schedule removal of the hold
			_ = RemoveHoldAfterDelay(skill, summon, durationMs);
		}

		/// <summary>
		/// Removes the movement hold after a delay.
		/// </summary>
		private async System.Threading.Tasks.Task RemoveHoldAfterDelay(Skill skill, Summon summon, int durationMs)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(durationMs));

			if (!summon.IsDead)
				summon.Vars.SetBool("HoldMovement", false);
		}
	}
}
