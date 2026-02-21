using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Bracelet of Protection - Frost, Receive 2.5% less Ice property damage per stack..
	/// </summary>
	[BuffHandler(BuffId.BRC01_141_buff)]
	public class BRC01_141_buff : BuffHandler
	{
		private const string VarName = "Melia.IceDefModifier";
		private const float BuffBonus = 2.5f;

		/// <summary>
		/// Starts buff, modifying the target's dark resistance.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modifier = buff.Vars.GetFloat(VarName);
			var bonus = BuffBonus * buff.OverbuffCounter;

			buff.Vars.SetFloat(VarName, modifier + bonus);

			target.Properties.Modify(PropertyName.Ice_Def_BM, bonus);

			if (target is Character character)
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Ice_Def_BM);
		}

		/// <summary>
		/// Ends the buff, resetting the dark resistance.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Vars.TryGetFloat(VarName, out var modifier))
			{
				target.Properties.Modify(PropertyName.ResIce_BM, -modifier);

				if (target is Character character)
					Send.ZC_OBJECT_PROPERTY(character, PropertyName.ResIce_BM);
			}
		}
	}
}
