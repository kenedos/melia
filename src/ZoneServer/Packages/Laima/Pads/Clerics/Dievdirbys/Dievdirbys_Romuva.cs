using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Romuva)]
	public class Dievdirbys_RomuvaOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(100f);
			pad.SetUpdateInterval(1000); // Update every 1 second

			// Check if Dievdirbys31 ability (Grieztas - damage mode) is active
			var isDamageMode = creator.IsAbilityActive(AbilityId.Dievdirbys31);
			pad.Variables.Set("Melia.RomuvaDamageMode", isDamageMode);

			// Match statue lifetime: 15 + (skill.Level * 2) seconds, or 20 seconds with Dievdirbys31 ability
			var lifeTime = 15 + skill.Level * 2;
			if (isDamageMode)
				lifeTime = 20;

			pad.Trigger.LifeTime = TimeSpan.FromSeconds(lifeTime);

			// Calculate max targets based on skill level: 4 + Floor(Skill Level * 0.6)
			var maxTargets = 4 + (int)Math.Floor(skill.Level * 0.6);
			pad.Trigger.MaxActorCount = maxTargets;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);

			// Only remove silence buff if not in damage mode
			var isDamageMode = pad.Variables.GetBool("Melia.RomuvaDamageMode");
			if (!isDamageMode)
			{
				// Remove silence buff from all enemies when pad is destroyed
				PadRemoveBuff(pad, RelationType.Enemy, 0, 0, BuffId.Common_Silence);
			}
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			// Only affect enemies
			if (!creator.IsEnemy(initiator)) return;

			// Check if in damage mode - if so, damage is dealt in Update, not on entry
			var isDamageMode = pad.Variables.GetBool("Melia.RomuvaDamageMode");
			if (isDamageMode)
				return;

			// Apply silence debuff to enemies - duration matches update interval to be refreshed continuously
			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, 0, BuffId.Common_Silence, 1, 0, 2500, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			// Only affect enemies
			if (!creator.IsEnemy(initiator)) return;

			// Check if in damage mode - if so, no buff to remove
			var isDamageMode = pad.Variables.GetBool("Melia.RomuvaDamageMode");
			if (isDamageMode)
				return;

			// Remove silence buff when enemy leaves the pad area
			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Common_Silence, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			// Check if in damage mode
			var isDamageMode = pad.Variables.GetBool("Melia.RomuvaDamageMode");

			if (isDamageMode)
			{
				// Deal damage to enemies in range every second
				// Damage is based on skill factor, enhanced by Dievdirbys19 ability
				PadDamageEnemy(pad, 1, 0, 0, "None", 1, 0f, 0f);
			}
			else
			{
				// Refresh silence debuff on all enemies in range every second
				// Duration is 1.5 seconds to ensure buff stays active between updates
				PadBuffCheckBuffEnemy(pad, RelationType.Enemy, 0, 0, BuffId.Common_Silence, BuffId.Common_Silence, 1, 0, 2500, 1, 100);
			}
		}
	}
}
