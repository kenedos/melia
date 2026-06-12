using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Arquebuiser skill Linear Shooting.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Arquebusier_LuckyStrike)]
	public class Arquebusier_LuckyStrike : IGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Called when the user starts casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, applying a debuff to the target
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);
			var shape = new Square(caster.Position.GetRelative2D(caster.Direction, 15), caster.Direction, 75, 20);
			var maxTargetCount = caster.IsAbilityActive(AbilityId.Arquebusier22) ? 10 : 6;

			var pad = Pad.Create(PadName.shootpad_LuckyStrike, caster, skill, farPos, shape, new PadOptions
			{
				LifeTime = TimeSpan.FromSeconds(2),
				UpdateInterval = TimeSpan.FromMilliseconds(250),
				MaxActorCount = maxTargetCount,
			});

			caster.Map.AddPad(pad);
			Debug.ShowShape(caster.Map, shape);
			
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, target.Position);
			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);
		}

		/// <summary>
		/// Handler for the Linear Shooting pad.
		/// </summary>
		[PadHandler(PadName.shootpad_LuckyStrike)]
		public class shootpad_LuckyStrike : ICreatePadHandler, IEnterPadHandler, IDestroyPadHandler
		{
			private const float FlyDistance = 1000;
			private const float FlySpeedForward = 400;

			/// <summary>
			/// Called when the pad is created.
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="args"></param>
			public void Created(object sender, PadTriggerArgs args)
			{
				var pad = args.Trigger;
				var caster = args.Creator;

				pad.Movement.Speed = FlySpeedForward;
				pad.Trigger.MaxActorCount = caster.IsAbilityActive(AbilityId.Arquebusier22) ? 10 : 6;

				TaskHelper.CallSafe(this.FlyForward(pad, caster));
			}

			public void Entered(object sender, PadTriggerActorArgs args)
			{
				var pad = args.Trigger;
				var caster = args.Creator;
				var target = args.Initiator;
				var skill = pad.Skill;

				// [Arts] Shaped Charge
				// Increase AOE Ratio to 10
				var maxTargetCount = caster.IsAbilityActive(AbilityId.Arquebusier22) ? 10 : 6;

				if (!caster.CanDamage(target) || pad.Variables.GetInt("Targets") >= maxTargetCount) return;

				var skillHitResult = SCR_SkillHit(caster, target, skill);

				// [Arts] Shaped Charge
				// Decrease the final damage by 25%
				if (caster.IsAbilityActive(AbilityId.Arquebusier22))
				{
					skillHitResult.Damage *= 0.75f;
				}

				target.TakeDamage(skillHitResult.Damage, caster);
				var hit = new HitInfo(caster, target, skill, skillHitResult, HitResultType.Hit, TimeSpan.FromMilliseconds(100));

				// Linear Shooting: Amplify Restraint
				// Stuns the target for 2 second on critical hits
				if (caster.IsAbilityActive(AbilityId.Arquebusier20) && hit.ResultType == HitResultType.Crit)
				{
					target.StartBuff(BuffId.Stun, TimeSpan.FromSeconds(2));
				}

				Send.ZC_HIT_INFO(caster, target, hit);
				pad.Variables.SetInt("Targets", pad.Variables.GetInt("Targets") + 1);
			}

			public void Destroyed(object sender, PadTriggerArgs args)
			{
				var pad = args.Trigger;
				var creator = args.Creator;

				Send.ZC_NORMAL.PadUpdate(pad, false);
				pad.Variables.SetInt("Targets", 0);
			}

			/// <summary>
			/// Makes shield fly a certain distance forward, in the direction
			/// the creator is facing.
			/// </summary>
			/// <param name="pad"></param>
			/// <param name="creator"></param>
			/// <returns></returns>
			private async Task FlyForward(Pad pad, ICombatEntity caster)
			{
				var dest = caster.Position.GetRelative2D(caster.Direction, FlyDistance);
				var moveTime = pad.Movement.MoveTo(dest);

				await Task.Delay(moveTime);
			}
		}
	}
}
