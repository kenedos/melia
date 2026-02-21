using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Shared.Network.NormalOp;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Pyromancer_FlameGround)]
	public class Pyromancer_FlameGroundOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(100f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(24000);
			pad.Trigger.MaxUseCount = 1;
			var maxActors = 20;
			pad.Trigger.MaxActorCount = maxActors;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);

			PadRemoveBuff(pad, RelationType.Enemy, 0, 0, BuffId.FlameGround_Debuff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
			var abilityLevel = pad.NumArg2;
			if (!creator.IsEnemy(initiator)) return;

			PadTargetBuffCheckAbility(pad, initiator, RelationType.Enemy, AbilityId.Pyromancer27, 0, 0, BuffId.FlameGround_Debuff, (int)abilityLevel, 10000, 1, 100);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
			if (!creator.IsEnemy(initiator)) return;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.FlameGround_Debuff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f, true, 1, target => target.MoveType != MoveType.Flying);
		}
	}
}
