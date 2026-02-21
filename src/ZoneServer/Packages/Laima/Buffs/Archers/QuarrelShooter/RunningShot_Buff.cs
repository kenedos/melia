using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
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
	public class RunningShot_BuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		private const float MovingShotBonusPerLevel = 0.1f;

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

		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.IsNormalAttack || skill.Id == SkillId.Bow_Hanging_Attack || skill.Id == SkillId.Cannon_Attack || skill.Id == SkillId.DoubleGun_Attack)
			{
				var factor = buff.NumArg2;
				modifier.DamageMultiplier += factor / 100f;
				modifier.HitCount += 1;
			}
		}

		private float GetMovingShotBonus(Buff buff)
		{
			var baseValue = 0.2f;
			var skillLevel = buff.NumArg1;
			var evasion = buff.Target.Properties.GetFloat(PropertyName.DR);

			return Math.Max(baseValue, baseValue + (evasion / 100) * skillLevel * MovingShotBonusPerLevel);
		}
	}
}
