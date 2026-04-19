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
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Evocation.
	/// Summons a spirit based on the sub-card that explodes after a short duration.
	/// </summary>
	/// <remarks>
	/// Evocation:
	/// - Requires a sub-card (slot 2) equipped and main summon active
	/// - Creates a scaled version of the sub-card monster
	/// - After 2 seconds, plays death animation and deals AoE damage
	/// - Damage range: 30 + (skillLevel * 5)
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_Evocation)]
	public class Sorcerer_EvocationOverride : IGroundSkillHandler, IDynamicCasted
	{

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			// Check for equipped sub-card (slot 2)
			var etc = character.Etc.Properties;
			var subCardName = etc.GetString(PropertyName.Sorcerer_bosscardName2, "None");
			var subCardGuid = etc.GetString(PropertyName.Sorcerer_bosscardGUID2, "None");

			if (subCardName == "None" || subCardGuid == "None")
			{
				caster.ServerMessage(Localization.Get("No sub-card equipped."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			// Verify the sub-card is still in inventory
			var cardId = long.Parse(subCardGuid);
			var card = character.Inventory.GetItem(cardId);
			if (card == null)
			{
				etc.SetString(PropertyName.Sorcerer_bosscardName2, "None");
				etc.SetString(PropertyName.Sorcerer_bosscardGUID2, "None");
				etc.SetFloat(PropertyName.Sorcerer_bosscard2, 0);
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Sorcerer_bosscardName2, PropertyName.Sorcerer_bosscard2);
				caster.ServerMessage(Localization.Get("Sub-card no longer available."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			// Check if main summon is active
			var mainCardName = etc.GetString(PropertyName.Sorcerer_bosscardName1, "None");
			var mainSummons = character.Summons.GetSummons(s =>
				s.Vars.TryGetInt("SORCERER_SUMMONING", out var val) && val == 1);

			if (mainSummons.Count == 0)
			{
				caster.ServerMessage(Localization.Get("Main summon must be active first."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			// Check if we're in a city
			var mapType = character.Map.Data.Type;
			if (mapType == MapType.City)
			{
				caster.ServerMessage(Localization.Get("Cannot use in this area."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

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

			skill.Run(this.HandleSkill(character, skill, targetPos, subCardName, card));
		}

		private async Task HandleSkill(Character character, Skill skill, Position targetPos, string monsterClassName, Item card)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(950));

			// Get the monster data for the sub-card
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterClassName, out var monsterData))
			{
				character.ServerMessage(Localization.Get("Invalid monster data."));
				return;
			}

			// Calculate spawn position (in front of first target or caster)
			var spawnPos = targetPos;

			// Create the evocation summon
			var summon = new Summon(character, monsterData.Id, RelationType.Friendly);

			summon.Position = spawnPos;
			summon.Direction = character.Direction;
			summon.Map = character.Map;
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;
			summon.Level = character.Level;

			// Mark as evocation summon
			summon.Vars.SetInt("EVOCATION_MON", 1);

			// Check if it's a legend card
			var isLegendCard = card.Data.EquipExpGroup == EquipExpGroup.Legend_Card;
			if (isLegendCard)
				summon.Vars.SetInt("LEGEND_CARD", 1);

			// Calculate scale
			var scale = 0.7f + (skill.Level / 15f) * 0.3f;
			summon.Properties.SetFloat(PropertyName.Scale, scale);

			// Set movement speed (slower for evocation)
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);

			// Activate summon without AI (it just appears and explodes)
			summon.SetState(true, canMove: false, hasAi: false);

			// Add to character's summon list temporarily
			character.Summons.AddSummon(summon);

			// Attach effects based on card type
			if (isLegendCard)
				Send.ZC_NORMAL.AttachEffect(summon, "F_evocation_legend", 1f);
			else
				Send.ZC_NORMAL.AttachEffect(summon, "F_evocation_normal", 1f);

			// Set color (evocation summons are tinted)
			summon.AddEffect(ColorEffect.FromRgba(0.8f, 0.6f, 1.0f, 1.0f));

			// Start the death sequence
			await EvocationDeathSequence(summon, character, skill);
		}

		/// <summary>
		/// Executes the evocation death sequence - plays animation and deals damage.
		/// </summary>
		private async Task EvocationDeathSequence(Summon summon, Character owner, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			// Play death animation
			Send.ZC_PLAY_ANI(summon, "dead", false);

			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			if (summon.IsDead)
				return;

			// Deal AoE damage at summon's position
			var damageRange = 30f + (skill.Level * 5f);
			var pos = summon.Position;

			// Get enemies in range
			var enemies = summon.Map.GetAttackableEnemiesInPosition(owner, pos, damageRange)
				.Where(e => owner.CanDamage(e))
				.ToList();

			if (enemies.Count > 0)
			{
				// Calculate and apply damage
				foreach (var enemy in enemies)
				{
					var damage = SCR_CalculateSkillDamage(owner, enemy, skill);
					var hitResult = SCR_ApplyDamage(owner, enemy, skill, damage);

					// Play hit effect
					Send.ZC_NORMAL.PlayEffect(enemy, "F_explosion_medium", 1f);
				}
			}

			// Play explosion effect at summon position
			Send.ZC_NORMAL.SkillProjectile(owner, pos, "F_explosion_large", 1.5f, "None", 0, 0, TimeSpan.Zero);

			// Kill the summon
			summon.Kill(owner);
		}

		/// <summary>
		/// Helper method to calculate skill damage.
		/// </summary>
		private float SCR_CalculateSkillDamage(ICombatEntity attacker, ICombatEntity target, Skill skill)
		{
			// Basic damage calculation - should be expanded based on actual game formulas
			var atk = attacker.Properties.GetFloat(PropertyName.MATK);
			var def = target.Properties.GetFloat(PropertyName.MDEF);
			var skillFactor = skill.Level * 0.5f + 1.0f;

			return Math.Max(1, (atk - def) * skillFactor);
		}

		/// <summary>
		/// Helper method to apply damage.
		/// </summary>
		private SkillHitResult SCR_ApplyDamage(ICombatEntity attacker, ICombatEntity target, Skill skill, float damage)
		{
			var hitResult = new SkillHitResult();
			hitResult.Damage = damage;

			target.TakeDamage(damage, attacker);

			return hitResult;
		}
	}
}
