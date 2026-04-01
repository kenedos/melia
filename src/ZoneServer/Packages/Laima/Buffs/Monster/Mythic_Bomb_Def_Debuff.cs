using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_Bomb_Def_Debuff.
	/// Reduces target's DEF. When the debuffed target takes damage,
	/// the debuff is consumed, amplifying that hit's damage and
	/// knocking them down in a random direction with an explosion.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_Bomb_Def_Debuff)]
	public class Mythic_Bomb_Def_DebuffOverride : BuffHandler
	{
		/// <summary>
		/// When the debuffed player takes damage, consume the debuff,
		/// amplify the damage, and knock them down in a random direction.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Mythic_Bomb_Def_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Mythic_Bomb_Def_Debuff, out var buff))
				return;

			var overbuffCounter = 0;
			if (target.TryGetBuff(BuffId.Mythic_Bomb_Def_Debuff, out var bombDebuff))
				overbuffCounter = bombDebuff.OverbuffCounter;
			skillHitResult.Damage *= (1f + overbuffCounter * 0.2f);

			Send.ZC_NORMAL.PlayEffect(target, "F_archer_explosiontrap_hit_explosion", 1f);

			if (target.IsKnockdownable())
			{
				var dummySkill = new Skill(attacker, SkillId.Normal_Attack);
				var hitResult = new SkillHitResult { Damage = 0 };
				var skillHit = new SkillHitInfo(attacker, target, dummySkill, hitResult, TimeSpan.FromMilliseconds(500), TimeSpan.Zero);

				skillHit.KnockBackInfo = new KnockBackInfo(attacker, target, KnockBackType.KnockDown, 180, 60, KnockDirection.Random);
				skillHit.HitInfo.KnockBackType = KnockBackType.KnockDown;
				target.ApplyKnockdown(attacker, dummySkill, skillHit);

				Send.ZC_SKILL_HIT_INFO(attacker, skillHit);
			}

			target.StopBuff(BuffId.Mythic_Bomb_Def_Debuff);
		}
	}
}
