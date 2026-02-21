using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Detecting, Enemies can see you even in stealth mode..
	/// </summary>
	[BuffHandler(BuffId.UC_Detected_Debuff)]
	public class UC_Detected_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (target.TryGetBuffByKeyword(BuffTag.Cloaking, out var cloakBuff))
				target.RemoveBuff(cloakBuff.Id);
		}
	}
}
