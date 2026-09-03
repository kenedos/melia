using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Prophecy buff, which makes the target immune to
	/// removable debuffs. The immunity itself is applied by the buff
	/// component's debuff resistance check, and the buff ends once it
	/// has prevented its share of debuffs.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Prophecy_Buff)]
	public class Oracle_Prophecy_BuffOverride : BuffHandler, IBuffOnDebuffResistedHandler
	{
		private const int DebuffsPerLevel = 2;
		private const string RemainingVar = "Melia.Prophecy.Remaining";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Vars.SetInt(RemainingVar, (int)buff.NumArg1 * DebuffsPerLevel);
		}

		public void OnDebuffResisted(Buff buff, BuffId buffId, IActor caster)
		{
			var remaining = buff.Vars.GetInt(RemainingVar) - 1;
			buff.Vars.SetInt(RemainingVar, remaining);

			if (remaining <= 0)
				buff.Target.StopBuff(BuffId.Prophecy_Buff);
		}
	}
}
