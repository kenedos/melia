using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Running Shot, Running Shot applied..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.RunningShot_Buff)]
	public class RunningShot_BuffOverride : BuffHandler
	{

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// TODO: Move this to WhileActive, because target's evasion
			// could change and we need to update move speed accordingly
			var movingShotBonus = this.GetMovingShotBonus(buff);

			AddPropertyModifier(buff, buff.Target, PropertyName.MovingShot_BM, movingShotBonus);

			if (buff.Target is Character character)
				Send.ZC_MOVE_SPEED(character);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MovingShot_BM);

			if (buff.Target is Character character)
				Send.ZC_MOVE_SPEED(character);
		}

		/// <summary>
		/// Doubles the attack, which the client shows as a second shot.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.RunningShot_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.RunningShot_Buff, out var buff))
				return;

			if (!this.IsRunningShotAttack(skill))
				return;

			var factor = buff.NumArg2;

			// Halved because the hit count doubles the damage before splitting
			// it, landing the pair on 2x plus the factor rather than 2x times it.
			modifier.DamageMultiplier += factor / 200f;
			modifier.HitCount += 1;
		}

		/// <summary>
		/// Returns whether the buff's extra shot applies to the given skill.
		/// </summary>
		/// <param name="skill"></param>
		private bool IsRunningShotAttack(Skill skill)
			=> skill.IsNormalAttack || skill.Id == SkillId.Bow_Hanging_Attack || skill.Id == SkillId.Cannon_Attack || skill.Id == SkillId.DoubleGun_Attack;

		private float GetMovingShotBonus(Buff buff)
		{
			var baseValue = 0.2f;
			var skillLevel = buff.NumArg1;
			var evasion = buff.Target.Properties.GetFloat(PropertyName.DR);

			return Math.Max(baseValue, baseValue + (evasion / 100) * (GetCaptionRatio(buff, 1) / 100f));
		}
	}
}
