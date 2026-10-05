using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Dievdirbys
{
	/// <summary>
	/// Handler override for Scud buff - Movement speed boost from Vakarine statue
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Scud)]
	public class ScudBuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Movement speed increase: 5 + SkillLv
			var skillLevel = buff.NumArg1;
			var movementBonus = 5 + skillLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, movementBonus);

			// Send movement speed update to client
			if (buff.Target is Character character)
				Send.ZC_MOVE_SPEED(character);
		}

		public override void OnEnd(Buff buff)
		{
			// Remove movement speed bonus
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);

			// Send movement speed update to client
			if (buff.Target is Character character)
				Send.ZC_MOVE_SPEED(character);
		}
	}
}
