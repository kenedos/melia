using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for Control Blade on an enemy, whose summoned swords strike
	/// it and the enemies next to it every 0.25 seconds, 11 times.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.ControlBlade_Debuff)]
	public class BlossomBlader_ControlBlade_DebuffOverride : BuffHandler
	{
		private const int Strikes = 11;
		private const float SplashRange = 30f;
		private const string StrikesVar = "Melia.BlossomBlader.ControlBladeStrikes";

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.BlossomBlader_ControlBlade, out var skill))
				return;

			var strikes = buff.Vars.GetInt(StrikesVar);
			if (strikes >= Strikes + BlossomBladerSkillHelper.GetBlossomShowerHits(caster))
				return;

			buff.Vars.SetInt(StrikesVar, strikes + 1);

			var victims = caster.Map.GetAttackableEnemiesInPosition(caster, target.Position, SplashRange);
			victims.Remove(target);
			victims.Insert(0, target);

			var hits = new List<SkillHitInfo>();

			foreach (var victim in victims.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, victim, skill);
				victim.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, victim, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

				if (skillHitResult.Damage > 0)
					BlossomBladerSkillHelper.ApplyFlowering(caster, victim);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
