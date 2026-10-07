using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Freeze Bullet, which gives basic pistol attacks the
	/// skill's ratio in percent to freeze the target for 2 seconds.
	/// </summary>
	/// <remarks>
	/// With Freeze Bullet: Silver Pellet they also have a 30% chance to add
	/// a Holy shot.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.FreezeBullet_Buff)]
	public class Bulletmarker_FreezeBullet_BuffOverride : BuffHandler
	{
		private const int SilverPelletChance = 30;
		private static readonly TimeSpan FreezeDuration = TimeSpan.FromSeconds(2);

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.FreezeBullet_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id != SkillId.DoubleGun_Attack && skill.Id != SkillId.Pistol_Attack && skill.Id != SkillId.Pistol_Attack2)
				return;

			if (skillHitResult.Damage <= 0 || !attacker.TryGetBuff(BuffId.FreezeBullet_Buff, out var buff))
				return;

			if (GameRandom.Get().Next(100) < GetCaptionRatio(buff, 1))
				target.StartBuff(BuffId.Freeze, 1, 0, FreezeDuration, attacker, buff.SkillId);

			if (!attacker.IsAbilityActive(AbilityId.Bulletmarker24) || GameRandom.Get().Next(100) >= SilverPelletChance)
				return;

			var silverPellet = new Skill(attacker, SkillId.Bulletmarker_SilverBulletAttack);
			var silverHitResult = SCR_SkillHit(attacker, target, silverPellet);

			skillHitResult.AddExtraLine(silverHitResult.Damage, SkillId.Bulletmarker_SilverBulletAttack);
		}
	}

	/// <summary>
	/// Handler for [Arts] Freeze Bullet: Fog's Chill, which slows the target
	/// by 10% per stack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.FreezeBullet_Cold_Debuff)]
	public class Bulletmarker_FreezeBullet_Cold_DebuffOverride : BuffHandler
	{
		private const float SlowPerStack = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -target.Properties.GetFloat(PropertyName.MSPD) * SlowPerStack * buff.OverbuffCounter);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
