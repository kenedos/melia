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
	/// Handle for the Swap Debuff, which confuses and immobilizes targets.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Swap_Debuff)]
	public class Swap_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			Send.ZC_SHOW_EMOTICON(target, "I_emo_confuse", buff.Duration);
			target.AddState(StateType.Stunned, buff.Duration);
		}

		public override void OnExtend(Buff buff)
		{
			var target = buff.Target;

			target.AddState(StateType.Stunned, buff.Duration);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
