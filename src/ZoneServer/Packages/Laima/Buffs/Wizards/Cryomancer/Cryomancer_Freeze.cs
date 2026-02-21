using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Frozen, Frozen solid..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Cryomancer_Freeze)]
	public class Cryomancer_FreezeOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;
			var target = buff.Target;

			if (target.Faction == FactionType.IceWall)
				return;

			Send.ZC_SHOW_EMOTICON(target, "I_emo_freeze", buff.Duration);
			Send.ZC_NORMAL.StatusEffect(target, (int)buff.RemainingDuration.TotalMilliseconds, "Freeze", "Cryomancer_Freeze");
		}

		public override void OnExtend(Buff buff)
		{
			var target = buff.Target;

			if (target.Faction == FactionType.IceWall)
				return;
			target.AddState(StateType.Frozen, buff.Duration);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (target.Faction == FactionType.IceWall)
				return;

		}
	}
}
