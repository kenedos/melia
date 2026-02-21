using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu Anila Effect Pad.
	/// Damages enemies on contact as it moves forward.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Sadhu_Anila_Effect_Pad)]
	public class Sadhu_Anila_Effect_PadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		/// <summary>
		/// Called when the pad is created.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(50f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(3000);
			pad.Trigger.MaxConcurrentUseCount = 3;
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
		/// Deals damage to enemies that the pad passes through.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var target = args.Initiator;

			if (!creator.IsEnemy(target))
				return;

			if (pad.Trigger.AtCapacity)
				return;

			var skill = pad.Skill;
			var modifier = SkillModifier.MultiHit(3);
			var skillHitResult = SCR_SkillHit(creator, target, skill, modifier);

			target.TakeDamage(skillHitResult.Damage, creator);

			var hit = new HitInfo(creator, target, skill, skillHitResult, TimeSpan.FromMilliseconds(200));
			Send.ZC_HIT_INFO(creator, target, hit);
		}
	}
}
