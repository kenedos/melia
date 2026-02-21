using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using System;

namespace Melia.Zone.Buffs.Handlers
{
	[Package("laima")]
	[BuffHandler(BuffId.SafetyZone_Buff)]
	public class SafetyZone_BuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Finds safety zone pad this entity is on
			// Note: The normal behaviour is to only allow one safety zone
			// per character, this is not currently implemented but should
			// not be an issue.
			var pads = buff.Target.Map.GetPadsAt(buff.Target.Position, 50f);
			Pad pad = null;
			foreach (var padFound in pads)
			{
				var padOwner = padFound.Creator;
				if (padOwner != null && padOwner == buff.Caster)
				{
					pad = padFound;
				}
			}
			// Cannot find the pad, commit sudoku
			if (pad == null)
			{
				target.RemoveBuff(BuffId.SafetyZone_Buff);
				return;
			}

			var remainingShieldHp = pad.Variables.GetFloat("Melia.SafetyZone.Shield.HP");
			if (remainingShieldHp <= 0)
			{
				pad.Destroy();
				return;
			}

			var reduceDamage = Math.Min(remainingShieldHp, skillHitResult.Damage);
			skillHitResult.Damage -= reduceDamage;

			pad.Variables.SetFloat("Melia.SafetyZone.Shield.HP", remainingShieldHp - reduceDamage);

			if (skillHitResult.Damage <= 0)
			{
				skillHitResult.Effect = HitEffect.SAFETY;
				skillHitResult.Result = HitResultType.Miss;
			}
		}
	}
}
