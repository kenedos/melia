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
	[PadHandler(PadName.Cleric_Laima)]
	public class Cleric_LaimaOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(100f);

			// Check if Dievdirbys32 ability is active and store it for later use
			var abilityActive = creator.IsAbilityActive(AbilityId.Dievdirbys32);
			pad.Variables.SetBool("Dievdirbys32Active", abilityActive);

			// Limit movement speed debuff to 10 enemy targets (only when ability is active)
			if (abilityActive)
				pad.Trigger.MaxActorCount = 10;

			// Match statue lifetime: 15 + (skill.Level * 2) seconds, or 3 seconds with Dievdirbys32 ability
			var lifeTime = abilityActive ? 3 : (15 + skill.Level * 2);

			pad.Trigger.LifeTime = TimeSpan.FromSeconds(lifeTime);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			// Get the stored ability state from pad creation
			var abilityActive = pad.Variables.GetBool("Dievdirbys32Active");

			if (abilityActive)
			{
				// Ability ON: Only apply movement speed debuff to enemies
				PadTargetBuff(pad, initiator, RelationType.Enemy, 0, 0, BuffId.CarveLaima_MSPD_Debuff, skill.Level, 0, 0, 1, 100, false);
			}
			else
			{
				// Ability OFF: Only apply cooldown reduction buff to allies
				PadTargetBuffAfterBuffCheck(pad, initiator, RelationType.Party, 0, 0, BuffId.Ausirine_Buff, BuffId.CarveLaima_Buff, skill.Level, 0, 0, 1, 100, true);
			}
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			// Get the stored ability state from pad creation
			var abilityActive = pad.Variables.GetBool("Dievdirbys32Active");

			if (abilityActive)
			{
				// Ability ON: Remove movement speed debuff from enemies
				PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.CarveLaima_MSPD_Debuff, false);
			}
			else
			{
				// Ability OFF: Remove cooldown reduction buff from allies
				PadTargetBuffRemove(pad, initiator, RelationType.Party, 0, 0, BuffId.CarveLaima_Buff, false);
			}
		}
	}
}
