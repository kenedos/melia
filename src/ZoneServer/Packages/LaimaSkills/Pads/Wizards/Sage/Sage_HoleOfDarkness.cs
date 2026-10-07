using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Sage
{
	/// <summary>
	/// Handler for Hole of Darkness, which blinds the enemies entering it for
	/// 15 seconds and strikes up to the skill's ratio of them with Dark
	/// damage every 0.5 seconds for 3 seconds.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Sage_HoleOfDarkness)]
	public class Sage_HoleOfDarknessOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float Range = 80f;
		private const int UpdateInterval = 500;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan BlindDuration = TimeSpan.FromSeconds(15);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = LifeTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;

			if (args.Creator.IsEnemy(args.Initiator))
				args.Initiator.StartBuff(BuffId.Blind, 1, 0, BlindDuration, args.Creator, pad.Skill.Id);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(maxTargets))
			{
				var modifier = new SkillModifier();
				modifier.AttackAttribute = AttributeType.Dark;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
