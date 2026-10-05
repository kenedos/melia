using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handle for the Aiming buff applied to enemies.
	/// Increases the target's effective hit radius for AoE attacks
	/// without affecting pathfinding.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Aiming_Buff)]
	public class Falconer_Aiming_BuffOverride : BuffHandler
	{
		private const float AimingHitRadiusBonus = 10.00f;
		private const float AimingHitRadiusBonusPerLevel = 2.0f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = buff.NumArg1;
			var bonus = AimingHitRadiusBonus + level * AimingHitRadiusBonusPerLevel;
			buff.Target.HitRadiusBonus = bonus;

			SendPreview(buff, true);
			Send.ZC_ALTER_HIT_RADIUS(buff.Caster, buff.Target, bonus, 0f);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.HitRadiusBonus = 0f;

			SendPreview(buff, false);
			Send.ZC_ALTER_HIT_RADIUS(buff.Caster, buff.Target, 0f, 0f);
		}

		private static void SendPreview(Buff buff, bool isEnabled)
		{
			Send.ZC_NORMAL.EnableHitRadiusPreview(buff.Target, isEnabled, buff.Caster);

			if (buff.Caster is not Character casterChar || casterChar.Connection.Party == null)
				return;

			var members = buff.Target.Map.GetPartyMembersInRange(casterChar, 0f, true);
			foreach (var member in members)
			{
				if (member == buff.Caster)
					continue;

				Send.ZC_NORMAL.EnableHitRadiusPreview(buff.Target, isEnabled, member);
			}
		}
	}
}
