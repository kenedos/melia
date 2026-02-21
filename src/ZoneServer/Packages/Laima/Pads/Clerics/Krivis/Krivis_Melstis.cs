using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Melstis)]
	public class Krivis_MelstisOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		/// <summary>
		/// Initializes the Melstis pad.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(45f);
			pad.SetUpdateInterval(500);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(15);

			// Store the total SP to be recharged over the pad's lifetime
			var totalSpRecharge = this.GetTotalSpRecharge(creator, skill.Level);
			pad.Variables.Set("TotalSpRecharge", totalSpRecharge);
		}

		/// <summary>
		/// Cleans up the Melstis pad.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Periodically recharges SP of allied players within the pad.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var totalSpRecharge = pad.Variables.Get<float>("TotalSpRecharge");
			var spRechargePerTick = totalSpRecharge / 30; // 30 ticks over 15 seconds

			var targets = pad.Trigger.GetAlliedEntities(creator);
			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
			{
				if (target is Character character)
				{
					var spToRecharge = Math.Min(spRechargePerTick, character.MaxSp - character.Sp);
					character.Heal(0, spToRecharge);
				}
			}
		}

		/// <summary>
		/// Calculates the total SP to be recharged over the pad's lifetime.
		/// </summary>
		private float GetTotalSpRecharge(ICombatEntity creator, int skillLevel)
		{
			var basePercentage = 0.20f; // 20% at level 1
			var percentagePerLevel = 0.04f; // 4% increase per level
			var totalPercentage = basePercentage + (skillLevel - 1) * percentagePerLevel;

			var byAbility = 1f;
			if (creator.TryGetActiveAbilityLevel(AbilityId.Kriwi30, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;
			totalPercentage *= byAbility;

			if (creator is Character character)
				return character.MaxSp * totalPercentage;
			else
				return creator.Level * 10 * totalPercentage;
		}
	}
}
