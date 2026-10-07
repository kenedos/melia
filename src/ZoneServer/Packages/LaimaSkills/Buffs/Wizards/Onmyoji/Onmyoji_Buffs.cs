using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Wizards.Onmyoji;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for Soul Fox Shikigami, which lasts as long as the fox and
	/// takes it with it when it ends.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.FireFoxShikigami_Buff)]
	public class Onmyoji_FireFoxShikigami_BuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var caster = buff.Target;

			if (!buff.Vars.TryGet<Mob>(Onmyoji_FireFoxShikigamiOverride.FoxVar, out var fox) || fox.IsDead || fox.Map != caster.Map)
				caster.StopBuff(buff.Id);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Vars.TryGet<Mob>(Onmyoji_FireFoxShikigamiOverride.FoxVar, out var fox) && !fox.IsDead)
				fox.Map?.RemoveMonster(fox);
		}
	}

	/// <summary>
	/// Handler for Greenwood Shikigami's slow, which halves movement speed,
	/// and with Greenwood Shikigami: Poison strikes the target with the
	/// skill every 0.9 seconds.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.GreenwoodShikigami_Debuff)]
	public class Onmyoji_GreenwoodShikigami_DebuffOverride : BuffHandler
	{
		private const float SpeedReduction = 0.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -target.Properties.GetFloat(PropertyName.MSPD) * SpeedReduction);
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead || target.IsDead || !caster.IsAbilityActive(AbilityId.Onmyoji5))
				return;

			if (!caster.TryGetSkill(SkillId.Onmyoji_GreenwoodShikigami, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}

	/// <summary>
	/// Handler for Howling White Tiger: Virtuous Roar, which raises movement
	/// speed by 10.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.WhiteTigerHowling_Buff)]
	public class Onmyoji_WhiteTigerHowling_BuffOverride : BuffHandler
	{
		private const float SpeedBonus = 10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, SpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
