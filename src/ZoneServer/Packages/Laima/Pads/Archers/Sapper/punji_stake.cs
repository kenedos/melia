using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Sapper
{
	[Package("laima")]
	[PadHandler(PadName.punji_stake)]
	public class punji_stakeOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const int TrapMaxHP = 10;
		private const string ActivatedKey = "Melia.PunjiStake.Activated";

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(25f);
			pad.SetUpdateInterval(100);
			pad.Trigger.LifeTime = TimeSpan.FromMinutes(2);

			pad.Variables.SetBool(ActivatedKey, false);

			var trap = (Mob)PadAttachMonster(pad, "skill_sapper_trap1", pad.Position, 0, 0, 0, 0, "HitProof#YES", "None", 1, true, "None", "None", true, "SCR_INIT_SAPPER_TRAP");
			if (trap != null)
			{
				var propertyOverrides = new PropertyOverrides();
				propertyOverrides.Add(PropertyName.HPCount, TrapMaxHP);
				trap.ApplyOverrides(propertyOverrides);
				trap.Properties.InvalidateAll();
				trap.HealToFull();
				trap.StartBuff(BuffId.Cover_Buff, TimeSpan.FromMinutes(2));
				trap.Died += (mob, killer) => pad.Destroy();
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			pad?.Monster?.Kill(null);
			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (pad.IsDead || skill == null)
				return;

			if (pad.Variables.GetBool(ActivatedKey))
				return;

			if (!creator.IsEnemy(initiator))
				return;

			// Reveal trap and play activation animation
			if (pad.Monster != null)
			{
				pad.Monster.StopBuff(BuffId.Cover_Buff);
				Send.ZC_NORMAL.PlayAnimationOnEffect_6D(pad.Monster, pad.Handle);
			}

			var hits = new List<SkillHitInfo>();
			var debuffDuration = TimeSpan.FromSeconds(2 + 0.3 * skill.Level);

			// Hit the initiator
			var skillHitResult = SCR_SkillHit(creator, initiator, skill);
			initiator.TakeDamage(skillHitResult.Damage, creator);

			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);

			if (initiator.IsKnockdownable())
			{
				skillHit.KnockBackInfo = new KnockBackInfo(initiator, HitType.KnockBack, 100, 10, pad.Direction);
				skillHit.HitInfo.Type = HitType.KnockBack;
				initiator.ApplyKnockback(creator, skill, skillHit);
			}

			initiator.StartBuff(BuffId.PunjiStake_Debuff, debuffDuration, creator);

			hits.Add(skillHit);

			// Hit all enemies in range
			var targets = creator.SelectObjects(pad.Position, 60, RelationType.Enemy);
			var targetList = targets.Take(7);
			foreach (var target in targetList)
			{
				if (target == initiator)
					continue;

				var targetHitResult = SCR_SkillHit(creator, target, skill);
				target.TakeDamage(targetHitResult.Damage, creator);

				var targetHit = new SkillHitInfo(creator, target, skill, targetHitResult);

				if (targetHitResult.Damage > 0)
				{
					if (target.IsKnockdownable())
					{
						targetHit.KnockBackInfo = new KnockBackInfo(target, HitType.KnockBack, 100, 10, pad.Direction);
						targetHit.HitInfo.Type = HitType.KnockBack;
						target.ApplyKnockback(creator, skill, targetHit);
					}

					target.StartBuff(BuffId.PunjiStake_Debuff, debuffDuration, creator);
				}
				
				hits.Add(targetHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);

			pad.Variables.SetBool(ActivatedKey, true);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(2);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
		}
	}
}
