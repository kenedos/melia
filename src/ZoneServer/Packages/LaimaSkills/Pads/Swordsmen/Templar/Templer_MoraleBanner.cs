using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Flag of Morale, which raises the minimum critical
	/// chance of allies inside it and builds the Templar's Uplift.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Templer_MoraleBanner)]
	public class Templer_MoraleBannerOverride : ICreatePadHandler, IDestroyPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float FlagRange = 100f;
		private static readonly TimeSpan FlagDuration = TimeSpan.FromSeconds(30);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(FlagRange);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = FlagDuration;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);

			foreach (var entity in pad.Trigger.GetActors<ICombatEntity>())
				TemplarSkillHelper.ClearMoraleMinCrit(entity);

			TemplarSkillHelper.ClearMoraleMinCrit(args.Creator);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			TemplarSkillHelper.ClearMoraleMinCrit(args.Initiator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			if (creator.IsDead)
			{
				pad.Destroy();
				return;
			}

			var minCritChance = pad.Skill.Properties.GetFloat(PropertyName.CaptionRatio);

			foreach (var ally in PartySkillHelper.GetAlliesInRange(creator, pad.Position, FlagRange))
			{
				TemplarSkillHelper.SetMoraleMinCrit(ally, minCritChance);

				if (ally == creator)
					TemplarSkillHelper.ProgressUplift(creator);
			}

			if (!creator.Position.InRange2D(pad.Position, FlagRange))
				TemplarSkillHelper.ClearMoraleMinCrit(creator);
		}
	}
}
