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
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Summon Servant.
	/// Summons a Russian Blue cat that creates buff pads for allies.
	/// </summary>
	/// <remarks>
	/// The servant:
	/// - Creates buff pads in sequence based on skill level
	/// - Available buffs: SR (Spirit Resistance), SP, STA, MDEF, Dark ATK
	/// - With Sorcerer13 ability at level 3+: Creates all pads immediately
	/// - Lifetime: 10 + (skillLevel * 5.5) seconds, or 10 seconds with Sorcerer13
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_SummonServant)]
	public class Sorcerer_SummonServantOverride : IGroundSkillHandler
	{
		/// <summary>
		/// List of buff pad class names the servant can create.
		/// </summary>
		private static readonly string[] BuffPads = new[]
		{
			"servantpad_SR",
			"servantpad_SP",
			"servantpad_STA",
			"servantpad_MDEF",
			"servantpad_DARKATK"
		};

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
			// Kill any existing servants first
			KillExistingServants(character);

			await skill.Wait(TimeSpan.FromMilliseconds(1300));

			// Calculate spawn position
			var spawnPos = originPos.GetRelative(farPos, distance: 80f);

			// Create the servant (Russian Blue cat)
			var summon = new Summon(character, MonsterId.RussianBlue, RelationType.Friendly);

			// Set up summon properties
			summon.Position = spawnPos;
			summon.Direction = character.Direction;
			summon.Map = character.Map;
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;
			summon.Level = character.Level;

			// Mark as sorcerer summon (Servant type)
			summon.Vars.SetInt("SORCERER_SUMMONSERVANT", 1);
			summon.Vars.SetInt("SKL_LEVEL", skill.Level);

			// Store skill level on character for reference
			character.Variables.Temp.SetInt("SORCERER_SERVANTSKILLLV", skill.Level);

			// Set movement speed (slower than other summons)
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 80f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 80f);

			// Calculate lifetime based on ability
			var hasQuickPads = character.IsAbilityActive(AbilityId.Sorcerer13) && skill.Level >= 3;
			var lifetime = hasQuickPads ? 10f : 10f + (skill.Level * 5.5f);

			// Add lifetime component
			summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromSeconds(lifetime)));

			// Activate the summon (without AI since it just creates pads)
			summon.SetState(true, canMove: true, hasAi: false);

			// Add to character's summon list
			character.Summons.AddSummon(summon);

			// Start the pad creation routine
			_ = CreateBuffPads(summon, character, skill, hasQuickPads);
		}

		/// <summary>
		/// Creates buff pads over time based on skill level.
		/// </summary>
		private async Task CreateBuffPads(Summon servant, Character owner, Skill skill, bool quickMode)
		{
			// Initial delay before starting pad creation
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			// Determine how many pads to create (limited by skill level and available pad types)
			var maxPads = Math.Min(skill.Level, BuffPads.Length);

			// Sleep time between pads (0 in quick mode, 5 seconds normally)
			var padDelay = quickMode ? 0 : 5000;
			var animDelay = quickMode ? 0 : 500;

			for (var i = 0; i < maxPads; i++)
			{
				// Check if servant is still alive
				if (servant.IsDead)
					break;

				// Small delay before animation in quick mode
				if (!quickMode)
					await skill.Wait(TimeSpan.FromMilliseconds(animDelay));

				// Play skill animation
				var pos = servant.Position;
				Send.ZC_PLAY_ANI(servant, "SKL", true);

				// Create the buff pad at servant's position
				var padName = BuffPads[i];
				CreateServantPad(servant, owner, padName, pos);

				// Wait between pads
				if (padDelay > 0 && i < maxPads - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(padDelay));
			}
		}

		/// <summary>
		/// Creates a servant buff pad at the specified position.
		/// </summary>
		private void CreateServantPad(Summon servant, Character owner, string padName, Position position)
		{
			// Create the pad using the pad system
			// The pad should apply its buff to allies who step on it
			var pad = new Pad(owner, owner.GetSkill(SkillId.Sorcerer_SummonServant), padName, position, servant.Direction);
			pad.Position = position;
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(30);
			pad.Trigger.Subscribe(TriggerType.Enter, this.OnPadEnter);

			owner.Map.AddPad(pad);
		}

		/// <summary>
		/// Called when an entity enters a servant pad.
		/// </summary>
		private void OnPadEnter(object sender, PadTriggerActorArgs args)
		{
			if (sender is not Pad pad)
				return;

			if (args.Initiator is not ICombatEntity target)
				return;

			if (pad.Creator is not ICombatEntity creator)
				return;

			// Only affect friendly targets
			if (!target.IsFriendlyTo(creator))
				return;

			// Apply the appropriate buff based on pad name
			var buffId = this.GetBuffForPad(pad.Name);
			if (buffId != BuffId.None)
			{
				target.StartBuff(buffId, TimeSpan.FromSeconds(30), pad.Creator);
			}
		}

		/// <summary>
		/// Gets the buff ID for a servant pad.
		/// </summary>
		private BuffId GetBuffForPad(string padName)
		{
			return padName switch
			{
				PadName.servantpad_SR => BuffId.ServantSR_Buff,
				PadName.servantpad_SP => BuffId.ServantSP_Buff,
				PadName.servantpad_STA => BuffId.ServantSTA_Buff,
				PadName.servantpad_MDEF => BuffId.ServantMDEF_Buff,
				PadName.servantpad_DARKATK => BuffId.ServantDARKATK_Buff,
				_ => BuffId.None
			};
		}

		/// <summary>
		/// Kills any existing servant summons.
		/// </summary>
		private void KillExistingServants(Character character)
		{
			var existingServants = character.Summons.GetSummons(s =>
				s.Vars.TryGetInt("SORCERER_SUMMONSERVANT", out var value) && value == 1);

			foreach (var summon in existingServants)
			{
				summon.Kill(character);
			}

			// Clear the stored skill level
			character.Variables.Temp.Remove("SORCERER_SERVANTSKILLLV");
		}
	}
}
