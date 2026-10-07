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
	/// Handler for the Flag of Revenge, which damages enemies inside it
	/// every second and builds the Templar's Uplift.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Templer_RevengeBanner)]
	public class Templer_RevengeBannerOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
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
			pad.Trigger.MaxActorCount = (int)pad.Skill.Properties.GetFloat(PropertyName.CaptionRatio);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
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

			PadDamageEnemy(pad);

			if (creator.Position.InRange2D(pad.Position, FlagRange))
				TemplarSkillHelper.ProgressUplift(creator);
		}
	}
}
