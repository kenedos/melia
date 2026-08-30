using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Caracole silence debuff, which stops the target from
	/// attacking.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Caracole_Silence_Debuff)]
	public class SchwarzerReiter_Caracole_Silence_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_SHOW_EMOTICON(buff.Target, "I_emo_silence", buff.Duration);
			buff.Target.AddState(StateType.Silenced);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Silenced);
		}
	}
}
