using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scout
{
	/// <summary>
	/// Handle for the Double Attack Buff, which increases the target's
	/// critical rate.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: Chance for a basic attack to hit twice, read by the attack
	/// handlers rather than applied here.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.DoubleAttack_Buff)]
	public class DoubleAttack_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = GetCaptionRatio(buff, 1) / 100f;

			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM);
		}
	}
}
