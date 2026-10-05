using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Pads.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Handler for Sweeping's purified area.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Miko_HoukiBroom)]
	public class Miko_HoukiBroomPadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float PadRange = 40f;
		private const int UpdateInterval = 100;
		private static readonly TimeSpan PadDuration = TimeSpan.FromMinutes(1);
		private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds(2);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);

			pad.SetRange(PadRange);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = PadDuration;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var targets = pad.Trigger.GetAlliedEntities(creator);

			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
				this.RemoveSweepingEffects(target);

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var creator = args.Creator;
			var target = args.Initiator;

			if (!target.IsAlly(creator))
				return;

			this.ApplySweepingEffects(target, creator);
			this.RemoveRemovableDebuffs(target);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			this.RemoveSweepingEffects(args.Initiator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var targets = pad.Trigger.GetAlliedEntities(creator);

			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
			{
				this.ApplySweepingEffects(target, creator);
				this.RemoveRemovableDebuffs(target);
			}
		}

		private void ApplySweepingEffects(ICombatEntity target, ICombatEntity creator)
		{
			target.StartBuff(BuffId.HoukiBroom_Buff, 1, 0f, EffectDuration, creator);
			target.StartBuff(BuffId.PadImmune_Buff, 1, 0f, EffectDuration, creator);
			target.StartBuff(BuffId.HoukiBroomSkllvup_Buff, 1, 0f, EffectDuration, creator);
		}

		private void RemoveSweepingEffects(ICombatEntity target)
		{
			target.RemoveBuff(BuffId.HoukiBroom_Buff);
			target.RemoveBuff(BuffId.PadImmune_Buff);
			target.RemoveBuff(BuffId.HoukiBroomSkllvup_Buff);
		}

		private void RemoveRemovableDebuffs(ICombatEntity target)
		{
			target.Components.Get<BuffComponent>()?.RemoveAll(buff => buff.Data.Type == BuffType.Debuff && buff.Data.RemoveBySkill);
		}
	}
}
