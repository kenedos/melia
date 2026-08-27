using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Shock Absorption, Creates a protective barrier that
	/// temporarily absorbs incoming damage.
	/// </summary>
	[BuffHandler(BuffId.Mon_Shield)]
	public class Mon_Shield : BuffHandler
	{
		private const string ShieldValueKey = "Melia.Monster.Shield";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var shield = (int)MathF.Floor(buff.Target.MaxHp * 0.10f);
			buff.Vars.SetInt(ShieldValueKey, shield);
			Send.ZC_UPDATE_SHIELD(buff.Target, shield);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Vars.Remove(ShieldValueKey);
			Send.ZC_UPDATE_SHIELD(buff.Target, 0);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Mon_Shield)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Mon_Shield, out var buff))
				return;

			var remainingShield = buff.Vars.GetInt(ShieldValueKey);

			if (remainingShield <= 0)
			{
				target.RemoveBuff(BuffId.Mon_Shield);
				return;
			}

			var damageToAbsorb = Math.Min(remainingShield, (int)skillHitResult.Damage);
			skillHitResult.Damage -= damageToAbsorb;
			remainingShield -= damageToAbsorb;

			buff.Vars.SetInt(ShieldValueKey, remainingShield);
			Send.ZC_UPDATE_SHIELD(target, remainingShield);

			if (skillHitResult.Damage <= 0)
			{
				skillHitResult.Effect = HitEffect.SAFETY;
				skillHitResult.Result = HitResultType.Miss;
			}

			if (remainingShield <= 0)
			{
				target.RemoveBuff(BuffId.Mon_Shield);
			}
		}
	}
}
