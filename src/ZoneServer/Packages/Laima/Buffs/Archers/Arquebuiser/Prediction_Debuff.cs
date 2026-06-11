using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Prediction DeBuff, which decreases the target's AOE Defense ratio as much the AOE Attack Ratio of the caster.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Prediction_Debuff)]
	public class Prediction_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var casterAOEAttackRatio = this.GetCasterAOEAttackRatio(buff);
			AddPropertyModifier(buff, buff.Target, PropertyName.SDR_BM, -casterAOEAttackRatio);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SDR_BM);
		}

		private float GetCasterAOEAttackRatio(Buff buff)
		{
			var caster = buff.Caster as ICombatEntity;
			return caster.Properties.GetFloat(PropertyName.SR);
		}
	}
}
