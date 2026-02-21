using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handle for the Singijeon_Debuff. It stores damage on application
	/// and explodes when the buff expires, damaging the target and nearby enemies.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Singijeon_Debuff)]
	public class Singijeon_DebuffOverride : BuffHandler
	{
		private const string VarDamage = "Melia.Skill.Singijeon.Damage";
		private const int SplashRadius = 65;
		private const int MaxSplashTargets = 9;
		private const int ShockDuration = 10000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			if (buff.Caster is not ICombatEntity caster)
				return;

			Send.ZC_NORMAL.PlayTextEffect(target, caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");

			if (caster.TryGetActiveAbilityLevel(AbilityId.Fletcher7, out var abilityLevel))
			{
				target.StartBuff(BuffId.UC_shock, abilityLevel, 0, TimeSpan.FromMilliseconds(ShockDuration), caster);
			}

			if (caster.TryGetSkill(SkillId.Fletcher_Singijeon, out var skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				buff.Vars.SetFloat(VarDamage, skillHitResult.Damage);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead)
				return;

			if (!buff.Vars.TryGetFloat(VarDamage, out var storedDamage) || storedDamage <= 0)
				return;

			if (buff.Caster is not ICombatEntity caster)
				return;

			if (!caster.TryGetSkill(SkillId.Fletcher_Singijeon, out var skill))
				return;

			target.PlayEffect("F_archer_explosiontrap_hit_explosion", 1);

			target.TakeSkillHit(caster, skill, HitType.Fire);

			var splashTargets = target.Map.GetAttackableEnemiesIn(
				caster,
				new CircleF(target.Position, SplashRadius),
				MaxSplashTargets,
				[target]
			);

			foreach (var splashTarget in splashTargets)
			{
				splashTarget.TakeSkillHit(caster, skill, HitType.Fire);
			}
		}
	}
}
