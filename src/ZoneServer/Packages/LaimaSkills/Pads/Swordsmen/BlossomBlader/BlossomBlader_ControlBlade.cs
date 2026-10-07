using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for the [Arts] Control Blade: Hiten Blade pad, whose swords
	/// slash 5 + half the AoE Attack Ratio enemies in it every 0.25 seconds
	/// for half of Control Blade's damage.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.BlossomBlader_ControlBlade)]
	public class BlossomBlader_ControlBladeOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int BaseTargets = 5;
		private const int SlashInterval = 250;
		private const float DamageRate = 0.5f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(SlashInterval);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			var maxTargets = BaseTargets + (int)(skill.Properties.GetFloat(PropertyName.SkillSR) / 2);
			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(maxTargets))
			{
				var modifier = new SkillModifier();
				modifier.DamageMultiplier *= DamageRate;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

				if (skillHitResult.Damage > 0)
					BlossomBladerSkillHelper.ApplyFlowering(caster, target);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
