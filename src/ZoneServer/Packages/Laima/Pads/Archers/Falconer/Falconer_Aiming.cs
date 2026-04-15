using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using static Melia.Zone.Pads.Helpers.PadHelper;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Pad handler for Falconer's Aiming skill.
	/// Follows the hawk companion. Applies Aiming_Buff to enemies
	/// in range every tick, increasing their effective hit radius
	/// for AoE attacks.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Falconer_Aiming)]
	public class Falconer_AimingOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float AimingRange = 100f;
		private const int UpdateIntervalMs = 1000;
		private const int BuffDurationMs = 1500;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(AimingRange);
			pad.SetUpdateInterval(UpdateIntervalMs);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Aiming_Buff);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (creator.IsDead)
			{
				pad.Destroy();
				return;
			}

			var buffDuration = TimeSpan.FromMilliseconds(BuffDurationMs);
			var enemies = pad.Trigger.GetAttackableEntities(creator);

			foreach (var enemy in enemies)
			{
				if (enemy.IsDead)
					continue;

				enemy.StartBuff(BuffId.Aiming_Buff, skill.Level, 0f, buffDuration, creator, skill.Id);
			}
		}
	}
}
