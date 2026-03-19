using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
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
		private const string VarName = "Melia.BlockedAttack";
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

		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Skill_MomentaryBlock_Buff)]
		public void OnBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Skill_MomentaryBlock_Buff, out var buff))
				return;

			// Don't let magic or unblockable attacks trigger the buff's effect.
			// While we check for magic before a forced block as well, we don't
			// want to flag the buff as having blocked something.
			if (skill.Data.ClassType == SkillClassType.Magic || modifier.Unblockable)
				return;

			modifier.ForcedBlock = true;
			buff.Vars.SetBool(VarName, true);
		}
	}
}
