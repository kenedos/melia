using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Abilities.Handlers.Swordsmen.Rodelero;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Pad handler for Shield Charge.
	/// Follows caster and knocks down enemies that enter the pad.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Rodelero_ShieldCharge)]
	public class Rodelero_ShieldChargeOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const string VarHitHandles = "Melia.ShieldCharge.HitHandles";

		/// <summary>
		/// Called when the pad is created.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetUpdateInterval(200);
			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(30f);
			pad.Variables.Set(VarHitHandles, new HashSet<int>());
		}

		/// <summary>
		/// Called when the pad is destroyed.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Called when an entity enters the pad area.
		/// Deals damage and knocks down enemies away from caster.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			var hitHandles = pad.Variables.Get<HashSet<int>>(VarHitHandles);

			if (hitHandles.Count >= 4)
				return;

			if (!hitHandles.Add(initiator.Handle))
				return;

			var modifier = SkillModifier.Default;
			modifier.BonusPAtk = Rodelero31.GetBonusPAtk(creator);
			var skillHitResult = SCR_SkillHit(creator, initiator, skill, modifier);

			initiator.TakeDamage(skillHitResult.Damage, creator);

			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult, TimeSpan.FromMilliseconds(50), TimeSpan.Zero);
			skillHit.HitEffect = HitEffect.Impact;

			if (skillHitResult.Damage > 0 && initiator.IsKnockdownable())
			{
				skillHit.KnockBackInfo = new KnockBackInfo(creator.Position, initiator, HitType.KnockDown, 120, 80);
				skillHit.HitInfo.Type = HitType.KnockDown;
				initiator.ApplyKnockdown(creator, skill, skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(creator, new List<SkillHitInfo> { skillHit });
		}

		/// <summary>
		/// Called when an entity leaves the pad area.
		/// </summary>
		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			var hitHandles = pad.Variables.Get<HashSet<int>>(VarHitHandles);
			hitHandles.Remove(initiator.Handle);
		}

		/// <summary>
		/// Called periodically while the pad is active.
		/// Follows the caster and destroys if caster is dead.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			var position = creator.Position.GetRelative(creator.Direction, 20);
			pad.Position = position;

			if (creator.IsDead)
			{
				pad.Destroy();
				return;
			}

			var hitHandles = pad.Variables.Get<HashSet<int>>(VarHitHandles);
			var insideHandles = pad.Trigger.GetAttackableEntities(creator).Select(e => e.Handle).ToHashSet();
			hitHandles.RemoveWhere(h => !insideHandles.Contains(h));
		}
	}
}
