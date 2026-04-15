using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Helper class for Sorcerer summon commands.
	/// Provides methods to order summons to attack, hold position, or follow.
	/// </summary>
	public static class SorcererSummonCommands
	{
		private const string OrderScriptKey = "Melia.OrderScript";
		private const string OrderCancellationKey = "Melia.OrderCancellation";

		/// <summary>
		/// Gets all valid sorcerer summons for the caster (main boss card summon and evocation summon).
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static List<Summon> GetSorcererSummons(ICombatEntity caster)
		{
			if (caster is not Character character)
				return new List<Summon>();

			var summons = character.Summons.GetSummons();

			// Filter to only sorcerer-controlled summons (those with SORCERER_MON property)
			// In the original game, this checks for Sorcerer_bosscardName1 and Sorcerer_bosscardName2
			return summons.Where(s => IsSorcererSummon(s)).ToList();
		}

		/// <summary>
		/// Checks if a summon is a sorcerer-controlled summon.
		/// </summary>
		/// <param name="summon"></param>
		/// <returns></returns>
		private static bool IsSorcererSummon(Summon summon)
		{
			// Check for SORCERER_MON property or SORCERER_SUMMONING property
			if (summon.Vars.TryGetInt("SORCERER_MON", out var isSorcererMon) && isSorcererMon == 1)
				return true;

			if (summon.Vars.TryGetInt("SORCERER_SUMMONING", out var isSummoning) && isSummoning == 1)
				return true;

			// Also check EVOCATION_MON for evocation summons
			if (summon.Vars.TryGetInt("EVOCATION_MON", out var isEvocationMon) && isEvocationMon == 1)
				return true;

			return false;
		}

		/// <summary>
		/// Orders summons to attack at a specific ground position.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <param name="range"></param>
		public static void OrderAttackGround(Skill skill, ICombatEntity caster, Position targetPos, float range = 100f)
		{
			var summons = GetSorcererSummons(caster);
			if (summons.Count == 0)
				return;

			foreach (var summon in summons)
			{
				// Cancel any existing order
				CancelCurrentOrder(summon);

				// Start new attack ground order
				var cts = new CancellationTokenSource();
				summon.Vars.Set(OrderCancellationKey, cts);
				summon.Vars.SetString(OrderScriptKey, "ORDER_ATTACK_GROUND");

				_ = ExecuteAttackGroundOrder(skill, summon, targetPos, range, cts.Token);
			}
		}

		/// <summary>
		/// Executes the attack ground order for a summon.
		/// </summary>
		private static async Task ExecuteAttackGroundOrder(Skill skill, Summon summon, Position targetPos, float range, CancellationToken cancellationToken)
		{
			const int maxWaitTime = 20000; // 20 seconds max wait
			var waitedTime = 0;

			try
			{
				// Wait for any current skill to finish
				while (waitedTime < maxWaitTime && !cancellationToken.IsCancellationRequested)
				{
					// Check if summon is using a skill
					if (!summon.IsUsingSkill())
						break;

					await skill.Wait(TimeSpan.FromMilliseconds(500));
					waitedTime += 500;
				}

				// Main attack loop
				while (!cancellationToken.IsCancellationRequested && !summon.IsDead)
				{
					var currentPos = summon.Position;
					var distToTarget = currentPos.Get2DDistance(targetPos);

					// Check for nearby enemies
					var enemies = summon.Map.GetAttackableEnemiesInPosition(summon, summon.Position, range)
						.Where(e => summon.CanAttack(e))
						.ToList();

					if (enemies.Count > 0)
					{
						var target = enemies.First();

						// Stop moving and attack
						summon.StopMove();

						// Use normal attack skill
						var normalSkill = summon.GetNormalAttackSkill();
						if (normalSkill != null)
						{
							// Queue attack on target
							summon.InsertHate(target);
						}

						await skill.Wait(TimeSpan.FromMilliseconds(500));

						// Wait for attack to complete
						while (summon.IsUsingSkill() && !cancellationToken.IsCancellationRequested)
						{
							await skill.Wait(TimeSpan.FromMilliseconds(500));
						}
					}

					// Check if we need to move to target position
					if (distToTarget > 5)
					{
						// Move towards target position
						summon.MoveTo(targetPos);
					}
					else
					{
						// Reached destination, clear order
						ClearOrderState(summon);
						return;
					}

					await skill.Wait(TimeSpan.FromMilliseconds(200));
				}
			}
			catch (OperationCanceledException)
			{
				// Order was cancelled, clean up
			}
			finally
			{
				ClearOrderState(summon);
			}
		}

		/// <summary>
		/// Orders summons to hold at a specific position and attack enemies in range.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		public static void OrderHold(Skill skill, ICombatEntity caster, Position targetPos)
		{
			var summons = GetSorcererSummons(caster);
			if (summons.Count == 0)
				return;

			foreach (var summon in summons)
			{
				// Cancel any existing order
				CancelCurrentOrder(summon);

				// Start new hold order
				var cts = new CancellationTokenSource();
				summon.Vars.Set(OrderCancellationKey, cts);
				summon.Vars.SetString(OrderScriptKey, "ORDER_HOLD_ATTACK");

				_ = ExecuteHoldOrder(skill, summon, targetPos, cts.Token);
			}
		}

		/// <summary>
		/// Executes the hold order for a summon.
		/// </summary>
		private static async Task ExecuteHoldOrder(Skill skill, Summon summon, Position holdPos, CancellationToken cancellationToken)
		{
			const float holdRange = 100f;

			try
			{
				// First, move to the hold position
				while (!cancellationToken.IsCancellationRequested && !summon.IsDead)
				{
					var currentPos = summon.Position;
					var distToHold = currentPos.Get2DDistance(holdPos);

					if (distToHold <= 5)
						break;

					summon.MoveTo(holdPos);
					await skill.Wait(TimeSpan.FromMilliseconds(500));
				}

				// Now hold position and attack enemies
				while (!cancellationToken.IsCancellationRequested && !summon.IsDead)
				{
					var currentPos = summon.Position;

					// Look for enemies in range
					var enemies = summon.Map.GetAttackableEnemiesInPosition(summon, summon.Position, holdRange)
						.Where(e => summon.CanAttack(e))
						.ToList();

					if (enemies.Count > 0)
					{
						var target = enemies.First();

						// Attack the target
						var normalSkill = summon.GetNormalAttackSkill();
						if (normalSkill != null)
						{
							summon.InsertHate(target);
						}

						await skill.Wait(TimeSpan.FromMilliseconds(500));

						// Wait for attack to complete
						while (summon.IsUsingSkill() && !cancellationToken.IsCancellationRequested)
						{
							await skill.Wait(TimeSpan.FromMilliseconds(500));
						}

						// Return to hold position if drifted
						var distToHold = summon.Position.Get2DDistance(holdPos);
						if (distToHold > 3)
						{
							summon.MoveTo(holdPos);
						}
					}

					await skill.Wait(TimeSpan.FromMilliseconds(200));
				}
			}
			catch (OperationCanceledException)
			{
				// Order was cancelled
			}
			finally
			{
				ClearOrderState(summon);
			}
		}

		/// <summary>
		/// Starts the Obey control mode, allowing the player to directly control their summon.
		/// </summary>
		/// <param name="caster"></param>
		public static void StartObeyControl(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			var summons = GetSorcererSummons(caster);
			if (summons.Count == 0)
				return;

			// Get the main summon (boss card summon)
			var mainSummon = summons.FirstOrDefault();
			if (mainSummon == null)
				return;

			// Cancel any existing orders
			CancelCurrentOrder(mainSummon);

			// Apply the Obey status buff to the summon
			mainSummon.StartBuff(BuffId.Sorcerer_Obey_Status_Buff, TimeSpan.Zero, caster);

			// Apply the PC defense buff to the caster
			caster.StartBuff(BuffId.Sorcerer_Obey_PC_DEF_Buff, TimeSpan.Zero, caster);

			// Send control object packet to client
			// This enables the Summoning_Buff UI for controlling the summon
			character.ToggleControl("Summoning_Buff", true);
			// Send.ZC_CONTROL_OBJECT(character, mainSummon, true, "Summoning_Buff");
		}

		/// <summary>
		/// Stops the Obey control mode.
		/// </summary>
		/// <param name="caster"></param>
		public static void StopObeyControl(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			var summons = GetSorcererSummons(caster);
			var mainSummon = summons.FirstOrDefault();

			if (mainSummon != null)
			{
				mainSummon.StopBuff(BuffId.Sorcerer_Obey_Status_Buff);
				character.ToggleControl("None", false);
				//Send.ZC_CONTROL_OBJECT(character, mainSummon, false, "None");
			}

			caster.StopBuff(BuffId.Sorcerer_Obey_PC_DEF_Buff);
		}

		/// <summary>
		/// Cancels the current order for a summon.
		/// </summary>
		/// <param name="summon"></param>
		private static void CancelCurrentOrder(Summon summon)
		{
			if (summon.Vars.TryGet<CancellationTokenSource>(OrderCancellationKey, out var cts))
			{
				cts.Cancel();
				cts.Dispose();
			}

			ClearOrderState(summon);
		}

		/// <summary>
		/// Clears the order state variables for a summon.
		/// </summary>
		/// <param name="summon"></param>
		private static void ClearOrderState(Summon summon)
		{
			summon.Vars.Remove(OrderCancellationKey);
			summon.Vars.SetString(OrderScriptKey, "None");
		}

		/// <summary>
		/// Commands familiar bats to attack a target.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="buff"></param>
		public static void CommandBatsToAttack(ICombatEntity caster, ICombatEntity target, Buff buff)
		{
			if (caster is not Character character)
				return;

			var bats = character.Summons.GetSummons(s => s.Id == MonsterId.Familiar);
			if (bats.Count == 0)
			{
				character.StopBuff(BuffId.sorcerer_bat);
				return;
			}

			Summon previousBat = null;
			foreach (var bat in bats)
			{
				var stopState = bat.Vars.GetInt("SORCERER_BAT_STOP", 0);
				if (stopState == 0)
				{
					bat.Vars.SetInt("SORCERER_BAT_STOP", 1);
					_ = ExecuteBatAttack(bat, target, buff, previousBat);
					previousBat = bat;
				}
			}
		}

		/// <summary>
		/// Executes the bat attack sequence.
		/// </summary>
		private static async Task ExecuteBatAttack(Summon bat, ICombatEntity target, Buff buff, Summon previousBat)
		{
			try
			{
				// Wait for previous bat to start attacking (chain attack effect)
				if (previousBat != null)
				{
					while (!previousBat.IsDead)
					{
						if (target == null || target.IsDead)
						{
							ResetBatState(bat);
							return;
						}
						await Task.Delay(TimeSpan.FromMilliseconds(500));
					}
				}

				// Move towards target
				while (!bat.IsDead)
				{
					if (target == null || target.IsDead)
					{
						ResetBatState(bat);
						return;
					}

					var dist = bat.Position.Get2DDistance(target.Position);
					if (dist <= 20)
						break;

					bat.MoveTo(target.Position);
					await Task.Delay(TimeSpan.FromMilliseconds(500));
				}

				// Attack and die (kamikaze attack)
				if (!bat.IsDead && target != null && !target.IsDead)
				{
					var caster = buff.Caster as ICombatEntity;
					if (caster != null && caster.CanDamage(target))
					{
						// Play explosion effect
						Send.ZC_NORMAL.PlayEffect(bat, "I_explosion012_dark", 0.5f);

						// Deal damage
						var skill = bat.GetSkill(SkillId.Mon_pcskill_summon_Familiar_Skill_1);
						if (skill != null)
						{
							// TODO: Calculate and apply damage
						}

						// Kill the bat
						bat.Kill(caster);
					}
				}
			}
			catch (Exception)
			{
				// Handle any errors gracefully
			}
			finally
			{
				ResetBatState(bat);
			}
		}

		/// <summary>
		/// Resets the bat's attack state.
		/// </summary>
		private static void ResetBatState(Summon bat)
		{
			bat.Vars.SetInt("SORCERER_BAT_STOP", 0);
			bat.StopMove();
		}
	}
}
