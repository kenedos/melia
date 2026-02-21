using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handler for the Momentary Block Buff
	/// </summary>
	/// <remarks>
	/// This buff is granted by certain skills and provides
	/// increased block chance for a duration.
	/// </remarks>
	[BuffHandler(BuffId.Skill_MomentaryBlock_Buff)]
	public class Skill_MomentaryBlock_Buff : BuffHandler
	{
		private const float BlkBonus = 300f;
		private const float BlkBonusPerLevel = 40f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var baseValue = BlkBonus;
			var byLevel = BlkBonusPerLevel * buff.NumArg1;

			var value = baseValue + byLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, value);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
		}

		/// <summary>
		/// Returns true if the entity has the buff and an attack was blocked
		/// at some point.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool WasAttackBlocked(ICombatEntity entity)
		{
			if (!entity.TryGetBuff(BuffId.Skill_MomentaryBlock_Buff, out var buff))
				return false;

			if (!buff.Vars.TryGetBool("Melia.BlockedAttack", out var value))
				return false;

			return value;
		}
	}
}
