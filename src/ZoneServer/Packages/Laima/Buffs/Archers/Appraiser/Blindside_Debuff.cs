using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Appraiser
{
	/// <summary>
	/// Marks the target with Expose Weakness.
	/// NumArg1 stores the additional critical chance.
	/// NumArg2 stores the skill level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Blindside_Debuff)]
	public class Blindside_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
