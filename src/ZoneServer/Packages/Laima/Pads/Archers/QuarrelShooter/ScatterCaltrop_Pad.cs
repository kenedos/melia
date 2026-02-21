using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.ScatterCaltrop_Pad)]
	public class ScatterCaltrop_PadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		/// <summary>
		/// Handles the creation of the ScatterCaltrop pad.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(50f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(20000);
			pad.Trigger.MaxUseCount = 1;
			pad.Trigger.MaxActorCount = 2 + skill.Level;
		}

		/// <summary>
		/// Handles the destruction of the ScatterCaltrop pad.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Handles an entity entering the ScatterCaltrop pad area.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator)) return;
			if (initiator.MoveType == MoveType.Flying) return;

			this.ApplyDebuff(pad, skill);
			this.ApplyDamage(pad);
		}

		/// <summary>
		/// Handles an entity leaving the ScatterCaltrop pad area.
		/// </summary>
		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
		}

		/// <summary>
		/// Handles periodic updates of the ScatterCaltrop pad.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			this.ApplyDebuff(pad, skill);
			this.ApplyDamage(pad);
		}

		private void ApplyDebuff(Pad pad, Skill skill)
		{
			PadBuff(pad, RelationType.Enemy, 0, 0, BuffId.ScatterCaltrop_Debuff, skill.Level, skill.Level, 2000, 1, 100, target => target.MoveType != MoveType.Flying);
		}

		private void ApplyDamage(Pad pad)
		{
			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f, true, 1, target => target.MoveType != MoveType.Flying);
		}
	}
}
