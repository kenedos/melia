using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Sanctuary, Physical and magic defense increased and additional damage while attack.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Sanctuary_Buff)]
	public class Sanctuary_BuffOverride : BuffHandler
	{
		private const string AtkModPropertyName = PropertyName.PATK_BM;
		private const string MAtkModPropertyName = PropertyName.MATK_BM;
		private const string DefModPropertyName = PropertyName.DEF_BM;
		private const string MDefModPropertyName = PropertyName.MDEF_BM;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = (ICombatEntity)buff.Caster;

			// Base: 150% + 15% per skill level
			var modValue = 1.5f + 0.15f * buff.NumArg1;

			// Paladin27 ability: +0.5% per level as final multiplier
			if (caster != null && caster.TryGetActiveAbilityLevel(AbilityId.Paladin27, out var abilityLevel))
				modValue *= (1 + abilityLevel * 0.005f);

			var pdef = target.Properties.GetFloat(PropertyName.DEF) * modValue;
			var mdef = target.Properties.GetFloat(PropertyName.MDEF) * modValue;

			// Flat defense bonus: 200 + 40 * skill level
			var flatDefBonus = 200 + 40 * buff.NumArg1;

			AddPropertyModifier(buff, target, AtkModPropertyName, pdef);
			AddPropertyModifier(buff, target, MAtkModPropertyName, mdef);
			AddPropertyModifier(buff, target, DefModPropertyName, pdef + flatDefBonus);
			AddPropertyModifier(buff, target, MDefModPropertyName, mdef + flatDefBonus);
		}

		/// <summary>
		/// Ends the buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, AtkModPropertyName);
			RemovePropertyModifier(buff, buff.Target, MAtkModPropertyName);
			RemovePropertyModifier(buff, buff.Target, DefModPropertyName);
			RemovePropertyModifier(buff, buff.Target, MDefModPropertyName);
		}
	}
}
