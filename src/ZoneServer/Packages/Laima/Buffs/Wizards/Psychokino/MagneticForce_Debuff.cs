using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.HandlersOverrides
{
	/// <summary>
	/// Handle for the Magnetic Force, Receive damage after a while.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.MagneticForce_Debuff)]
	public class MagneticForce_DebuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var caster = (ICombatEntity)buff.Caster;
			var skillLv = buff.NumArg1;
			var target = buff.Target;

			// Stun chance
			if (caster.TryGetActiveAbilityLevel(AbilityId.Psychokino4, out var abilLv)
				&& RandomProvider.Get().Next(100) < abilLv * 5)
				target.StartBuff(BuffId.Stun, 1, 0, TimeSpan.FromSeconds(4), caster);

			if (caster.TryGetSkill(SkillId.Psychokino_MagneticForce, out var skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hitInfo = new HitInfo(caster, target, skillHitResult.Damage, HitResultType.Hit);

				Send.ZC_HIT_INFO(caster, target, hitInfo);
			}
		}
	}
}
