using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handle for Silence, prevents attacks
	/// (Should we prevent magic attacks only instead? that'd be cool, albeit
	/// not official behaviour).
	/// </summary>
	[BuffHandler(BuffId.Common_Silence, BuffId.UC_silence)]
	public class Common_Silence : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;
			var target = buff.Target;

			Send.ZC_SHOW_EMOTICON(target, "I_emo_silence", buff.Duration);
			buff.Target.AddState(StateType.Silenced);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Silenced);
		}
	}
}
