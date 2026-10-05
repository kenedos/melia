using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Death Sentence: Slow, which decreases the
	/// target's movement speed by 30%.
	/// </summary>
	/// <remarks>
	/// Applied via Oracle8 ability when DeathVerdict hits.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.DeathVerdict_Slow_Debuff)]
	public class Oracle_DeathVerdict_Slow_DebuffOverride : BuffHandler
	{
		private const float MspdDebuffRate = 0.3f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var reduceMspd = target.Properties.GetFloat(PropertyName.MSPD) * MspdDebuffRate;

			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -reduceMspd);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
