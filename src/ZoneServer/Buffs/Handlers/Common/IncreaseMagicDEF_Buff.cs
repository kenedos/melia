using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Increase Magic Defense, Increases Magic Defense. (Can be removed when the user leaves the party or logs-out).
	/// </summary>
	[BuffHandler(BuffId.IncreaseMagicDEF_Buff)]
	public class IncreaseMagicDEF_Buff : BuffHandler
	{
		private const string ModPropertyName = PropertyName.MDEF_BM;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = (ICombatEntity)buff.Caster;
			var target = buff.Target;

			float modValue;
			if (caster != null)
			{
				var mna = caster.Properties.GetFloat(PropertyName.MNA);

				if (buff.Source == BuffOrigin.FromAutoSeller)
				{
					var mnaBM = caster.Properties.GetFloat(PropertyName.MNA_BM);
					var mnaItemBM = caster.Properties.GetFloat(PropertyName.MNA_ITEM_BM);
					mna -= (mnaBM + mnaItemBM);
				}

				modValue = MathF.Floor(240 + ((buff.NumArg1 - 1) * 80) + ((buff.NumArg1 / 3) * MathF.Pow(mna, 0.9f)));
			}
			else
			{
				modValue = buff.NumArg1;
			}

			if (buff.Source == BuffOrigin.FromAutoSeller)
			{
				modValue = MathF.Floor(modValue * 0.7f);
			}

			AddPropertyModifier(buff, target, ModPropertyName, modValue);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, ModPropertyName);
		}
	}
}
