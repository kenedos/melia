using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Invulnerable buff, which raises accuracy and keeps
	/// the Zealot from being knocked back or down.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Invulnerable_Buff)]
	public class Zealot_Invulnerable_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPairedPropertyModifier(buff, buff.Target, PropertyName.HR_BM, PropertyName.HR_RATE_BM, GetCaptionRatio(buff, 1));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePairedPropertyModifier(buff, buff.Target, PropertyName.HR_BM, PropertyName.HR_RATE_BM);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;
	}
}
