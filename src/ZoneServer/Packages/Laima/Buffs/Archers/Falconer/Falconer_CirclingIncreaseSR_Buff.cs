using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handle for the Circling: Expand, AoE Attack Ratio increased..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CirclingIncreaseSR_Buff)]
	public class Falconer_CirclingIncreaseSR_BuffOverride : BuffHandler
	{
		private const float SrBonus = 10f;
		private const float Falconer11SrBonus = 3f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var total = SrBonus;
			if (buff.Caster is ICombatEntity caster && caster.IsAbilityActive(AbilityId.Falconer11))
				total += Falconer11SrBonus;

			AddPropertyModifier(buff, buff.Target, PropertyName.SR_BM, total);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SR_BM);
		}
	}
}
