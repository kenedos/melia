using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handler for the Confuse debuff, which confuses and immobilizes targets.
	/// </summary>
	[BuffHandler(BuffId.Confuse)]
	public class Confuse : BuffHandler
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
