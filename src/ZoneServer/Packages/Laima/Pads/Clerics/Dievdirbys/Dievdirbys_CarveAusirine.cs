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
	[PadHandler(PadName.Dievdirbys_CarveAusirine)]
	public class Dievdirbys_CarveAusirineOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(45f);
			pad.SetUpdateInterval(3000);
			var lifetimeSeconds = 15 + skill.Level * 2;
		pad.Trigger.LifeTime = TimeSpan.FromSeconds(lifetimeSeconds);
			pad.Trigger.MaxUseCount = 20000;

			// Play looping effect around the statue for its entire duration
			var lifetimeMs = lifetimeSeconds * 1000;
			pad.PlayEffectToGround("F_cleric_ausirine_shot_ground_active", pad.Position, 1f, lifetimeMs, 0, 0);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
			PadRemoveBuff(pad, RelationType.Friendly, 0, 0, BuffId.Ausirine_Buff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!PadActivate(pad, initiator, RelationType.Friendly)) return;
			PadTargetBuff(pad, initiator, RelationType.Friendly, 0, 0, BuffId.Ausirine_Buff, skill.Level, 0, 15000, 1, 100, false);

			// Play visual effects on the target when they receive the buff
			pad.PlayEffectToGround("F_cleric_ausirine_shot_light", initiator.Position, 0.5f, 1000f, 0, 0);
			pad.PlayEffectToGround("F_cleric_ausirine_shot_ground", initiator.Position, 0.3f, 1000f, 0, 0);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			// Re-apply buff to all allies inside the pad every 3 seconds (refreshing the 15s duration)
			PadBuff(pad, RelationType.Friendly, 0, 0, BuffId.Ausirine_Buff, skill.Level, 0, 15000, 1, 100);
		}
	}
}
