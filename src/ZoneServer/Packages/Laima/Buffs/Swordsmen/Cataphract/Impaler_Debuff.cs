using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsmen.Cataphract
{
	/// <summary>
	/// Handler for the Impaler Debuff. Pins the target to the caster's spear
	/// and reduces their physical defense by 30%.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Impaler_Debuff)]
	public class Impaler_DebuffOverride : BuffHandler
	{
		private const float DefenseReduction = -0.30f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;
			var target = buff.Target;

			target.AttachToObject(caster, "Dummy_Impaler", "None", holdAi: true);
			target.Lock(LockType.Attack);
			target.Lock(LockType.Movement);

			AddPropertyModifier(buff, target, PropertyName.DEF_RATE_BM, DefenseReduction);
		}

		public override void OnEnd(Buff buff)
		{
			var caster = buff.Caster;
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.DEF_RATE_BM);

			target.Unlock(LockType.Attack);
			target.Unlock(LockType.Movement);
			target.Position = caster.Position;
			Send.ZC_DETACH_TO_OBJ(target, caster);
		}
	}
}
