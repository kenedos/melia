using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Flag of Vitality, which heals allies inside it and
	/// builds the Templar's Uplift.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Templer_VitalityBanner)]
	public class Templer_VitalityBannerOverride : ICreatePadHandler, IDestroyPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float FlagRange = 100f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(FlagRange);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = pad.Skill.Properties.CaptionTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.VitalityBanner_Buff);
			args.Creator.StopBuff(BuffId.VitalityBanner_Buff);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.VitalityBanner_Buff);
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

			foreach (var ally in PartySkillHelper.GetAlliesInRange(creator, pad.Position, FlagRange))
			{
				if (!ally.IsBuffActive(BuffId.VitalityBanner_Buff))
					ally.StartBuff(BuffId.VitalityBanner_Buff, pad.Skill.Level, 0, pad.Trigger.RemainingLifeTime, creator, pad.Skill.Id);

				if (ally == creator)
					TemplarSkillHelper.ProgressUplift(creator);
			}

			if (!creator.Position.InRange2D(pad.Position, FlagRange))
				creator.StopBuff(BuffId.VitalityBanner_Buff);
		}
	}
}
