using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Moksha.
	/// Attracts enemies towards pad center, deals periodic damage,
	/// then explodes at the end of duration.
	/// Requires spirit form.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_Moksha)]
	public class Sadhu_MokshaOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int MaxTargets = 9;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
				return;

			if (caster is not Character casterCharacter)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, ForceId.GetNew(), null);

			var pad = new Pad(PadName.Sadhu_Moksha_Pad, casterCharacter, skill, new Circle(caster.Position, 100));

			pad.Position = caster.Position;
			pad.Trigger.MaxActorCount = MaxTargets;
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(5);
			pad.Trigger.UpdateInterval = TimeSpan.FromMilliseconds(500);
			pad.Trigger.Subscribe(TriggerType.Update, this.OnUpdate);
			pad.Trigger.Subscribe(TriggerType.Destroy, this.OnDestroyPad);

			caster.Map.AddPad(pad);
		}

		private void OnUpdate(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = args.Skill;

			var padTargets = pad.Trigger.GetAttackableEntities(caster);
			var hits = new List<SkillHitInfo>();

			foreach (var target in padTargets.Take(MaxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);

				if (target.IsKnockdownable())
				{
					var pullDirection = target.Position.GetDirection(pad.Position);
					var pullFromPos = target.Position.GetRelative(pullDirection.Backwards, 40);

					skillHit.KnockBackInfo = new KnockBackInfo(pullFromPos, target, HitType.KnockBack, 60, 10);
					skillHit.KnockBackInfo.Speed = 1;
					skillHit.KnockBackInfo.VPow = 1;
					skillHit.HitInfo.Type = HitType.KnockBack;

					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private void OnDestroyPad(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			this.EndAttack(pad.Skill, creator, (ISplashArea)pad.Area);
		}

		private void EndAttack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targets.Take(MaxTargets))
			{
				var modifier = SkillModifier.MultiHit(6);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(50));
				Send.ZC_HIT_INFO(caster, target, hit);
			}
		}
	}
}
