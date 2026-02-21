using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Elementalist_FrostCloud)]
	public class Elementalist_FrostCloudOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float FreezeChance = 30f;
		private const int FreezeDurationMs = 4000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(50f);
			pad.SetUpdateInterval(300);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(5400);
			pad.Trigger.MaxUseCount = 1;
			pad.Trigger.MaxActorCount = 12 + (skill.Level * 2);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
			PadBuffCheckBuffEnemy(pad, RelationType.Enemy, 0, 0, BuffId.Cryomancer_Freeze, BuffId.Cryomancer_Freeze, 1, 0, FreezeDurationMs, 1, (int)FreezeChance);
		}
	}
}
