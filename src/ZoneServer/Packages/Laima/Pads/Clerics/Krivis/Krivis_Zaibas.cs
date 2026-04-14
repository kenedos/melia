using System;
using System.Linq;
using System.Numerics;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Skills;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Zaibas)]
	public class Krivis_ZaibasOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var skill = args.Skill;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(50f);
			pad.SetUpdateInterval(150);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
			pad.Trigger.MaxActorCount = 4;
			pad.Trigger.MaxUseCount = 20;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var targets = pad.Trigger.GetAttackableEntities(creator);

			if (targets == null || !targets.Any())
				return;

			var modifier = new SkillModifier();
			modifier.BonusMAtk = pad.NumArg2;

			var targetCount = pad.Trigger.MaxActorCount;
			foreach (var target in targets)
			{
				if (targetCount <= 0)
					break;

				var skillHitResult = SCR_SkillHit(creator, target, skill, modifier);
				var damage = skillHitResult.Damage;
				target.TakeSimpleHit(damage, creator, skill.Id);

				this.TryApplyStagger(pad, creator, target, skill);

				pad.PlayEffectToGround("F_cleric_zaibas_shot_rize", target.Position, 1f, 3000f, 0, 0);
				pad.PlayEffectToGround("F_cleric_zaibas_shot_ground", target.Position, 0.5f, 500f, 0f, 0);

				targetCount--;
			}

			// After hitting all targets we can, we consume one use
			pad.Trigger.IncreaseUseCount();
		}
		private void TryApplyStagger(Pad pad, ICombatEntity creator, ICombatEntity target, Skill skill)
		{
			if (!target.IsKnockdownable())
				return;

			var staggerKey = "Melia.Zaibas.Stagger." + target.Handle;
			var now = DateTime.Now;

			if (pad.Variables.TryGet<DateTime>(staggerKey, out var lastStagger) && (now - lastStagger).TotalMilliseconds < 1000)
				return;

			pad.Variables.Set(staggerKey, now);

			var staggerResult = new SkillHitResult { Damage = 0, Result = HitResultType.Hit };
			var skillHit = new SkillHitInfo(creator, target, skill, staggerResult);
			skillHit.KnockBackInfo = new KnockBackInfo(pad.Position, target, KnockBackType.Motion, 0, 10);
			skillHit.HitInfo.KnockBackType = KnockBackType.Motion;
			target.ApplyKnockback(creator, skill, skillHit);
			Send.ZC_SKILL_HIT_INFO(creator, skillHit);
		}
	}
}
