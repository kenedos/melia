using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Elementalist
{
	[Package("laima")]
	[PadHandler(PadName.Elementalist_ChainReaction_Pad)]
	public class Elementalist_ChainReaction_PadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const int ElectricShockDurationMs = 4000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(20f);
			pad.SetUpdateInterval(750);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
			pad.Trigger.MaxUseCount = 1;
			pad.Trigger.MaxActorCount = 5;
			creator.PlaySound("skl_eff_lightningsphere_born");
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			if (skill == null)
				return;

			var hits = new List<SkillHitInfo>();

			// Damage the initiator and apply ElectricShock
			var modifier = SkillModifier.Default;
			if (initiator.IsBuffActiveByKeyword(BuffTag.Freeze))
				modifier.DamageMultiplier = 2f;

			var skillHitResult = SCR_SkillHit(creator, initiator, skill, modifier);
			initiator.TakeDamage(skillHitResult.Damage, creator);
			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);
			hits.Add(skillHit);

			if (skillHitResult.Damage > 0)
				initiator.StartBuff(BuffId.ElectricShock, 5, 0, TimeSpan.FromMilliseconds(ElectricShockDurationMs), creator);

			// Damage all enemies in range and apply ElectricShock
			var targets = creator.SelectObjects(pad.Position, 60, RelationType.Enemy);
			foreach (var target in targets)
			{
				if (target == initiator)
					continue;

				var targetModifier = SkillModifier.Default;
				if (target.IsBuffActiveByKeyword(BuffTag.Freeze))
					targetModifier.DamageMultiplier = 2f;

				var targetHitResult = SCR_SkillHit(creator, target, skill, targetModifier);
				target.TakeDamage(targetHitResult.Damage, creator);
				var targetHit = new SkillHitInfo(creator, target, skill, targetHitResult);
				hits.Add(targetHit);

				if (targetHitResult.Damage > 0)
					target.StartBuff(BuffId.ElectricShock, 5, 0, TimeSpan.FromMilliseconds(ElectricShockDurationMs), creator);
			}

			foreach (var hit in hits)
				Send.ZC_HIT_INFO(creator, hit.Target, hit.HitInfo);

			pad.Trigger.IncreaseUseCount();
		}
	}
}
