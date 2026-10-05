using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Wizards.RuneCaster
{
	[Package("laima")]
	[BuffHandler(BuffId.RuneOfDestruction_MDef_Debuff)]
	public class RuneCaster_RuneOfDestructionMDefDebuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var reduction = this.GetReduction(buff);
			buff.NumArg2 = reduction;

			buff.Target.Properties.Modify(PropertyName.MDEF_BM, -reduction);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.Properties.Modify(PropertyName.MDEF_BM, buff.NumArg2);
		}

		private float GetReduction(Buff buff)
		{
			// Ability: MDEF -3% per ability level.
			var abilityLevel = buff.NumArg2;

			var currentMDef = buff.Target.Properties.GetFloat(PropertyName.MDEF);

			return currentMDef * (0.03f * abilityLevel);
		}
	}
}
