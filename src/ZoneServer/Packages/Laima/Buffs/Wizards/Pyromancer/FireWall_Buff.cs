using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for FireWall_Buff, which grants additional fire attack to allies
	/// standing inside the FireWall when the caster has Pyromancer2 ability.
	/// </summary>
	/// <remarks>
	/// NumArg1: Pyromancer2 ability level (1-5)
	/// NumArg2: Caster's INT value at time of buff application
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.FireWall_Buff)]
	public class FireWall_BuffOverride : BuffHandler
	{
		private const float BaseFireAtk = 20f;
		private const float FireAtkPerAbilityLevel = 10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var abilityLevel = buff.NumArg1;
			var casterInt = buff.NumArg2;

			// Formula: 20 + (AbilityLevel * 10) + ((CasterINT * AbilityLevel) / 100)
			var fireAtk = BaseFireAtk + (abilityLevel * FireAtkPerAbilityLevel) + ((casterInt * abilityLevel) / 100f);
			var finalFireAtk = (float)Math.Floor(fireAtk);

			var propertyName = target is Character ? PropertyName.Fire_Atk_BM : PropertyName.ADD_FIRE_BM;
			AddPropertyModifier(buff, target, propertyName, finalFireAtk);
		}

		public override void OnEnd(Buff buff)
		{
			var propertyName = buff.Target is Character ? PropertyName.Fire_Atk_BM : PropertyName.ADD_FIRE_BM;
			RemovePropertyModifier(buff, buff.Target, propertyName);
		}
	}
}
