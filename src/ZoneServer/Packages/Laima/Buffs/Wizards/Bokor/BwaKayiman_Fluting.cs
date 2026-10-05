using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the BwaKayiman_Fluting buff applied to the caster during channeling.
	/// This buff indicates the caster is channeling Bwa Kayiman and increases movement speed.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.BwaKayiman_Fluting)]
	public class BwaKayiman_FlutingOverride : BuffHandler
	{
		private const float MovementSpeedBonus = 30;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Fluting);
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MovementSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Fluting);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
