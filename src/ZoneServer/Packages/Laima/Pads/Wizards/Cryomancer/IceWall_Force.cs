using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.HandlersOverride
{
	[Package("laima")]
	[PadHandler(PadName.IceWall_Force)]
	public class IceWall_ForceOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(20f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1000);
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

			var damageRate = 1f;
			if (ZoneServer.Instance.World.IsPVP)
				damageRate = 0.5f;
			PadTargetDamage(pad, initiator, RelationType.Enemy, damageRate, -1000, 0);

			PadBuffCheckBuffEnemy(pad, RelationType.Enemy, 0, 0, BuffId.Cryomancer_Freeze, BuffId.Cryomancer_Freeze, 1, 0, 3000, 1, 100);
		}
	}
}
