using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Bracelet of Protection - Darkness II, Receive 5% less Darkness property damage per stack..
	/// </summary>
	[BuffHandler(BuffId.BRC02_120_buff)]
	public class BRC02_120_buff : BuffHandler
	{
		private const string VarName = "Melia.DarkDefModifier";
		private const float BuffBonus = 5f;

		/// <summary>
		/// Starts buff, modifying the target's dark defense.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modifier = buff.Vars.GetFloat(VarName);
			var bonus = BuffBonus * buff.OverbuffCounter;

			buff.Vars.SetFloat(VarName, modifier + bonus);

			target.Properties.Modify(PropertyName.Dark_Def_BM, bonus);

			if (target is Character character)
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Dark_Def_BM);
		}

		/// <summary>
		/// Ends the buff, resetting the dark defense.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Vars.TryGetFloat(VarName, out var modifier))
			{
				target.Properties.Modify(PropertyName.Dark_Def_BM, -modifier);

				if (target is Character character)
					Send.ZC_OBJECT_PROPERTY(character, PropertyName.Dark_Def_BM);
			}
		}
	}
}
