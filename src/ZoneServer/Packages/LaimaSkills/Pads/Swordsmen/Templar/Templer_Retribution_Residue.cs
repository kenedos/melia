using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Swordsmen.Templar;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for Retribution: Retention's lingering energy, which deals
	/// 10% of Retribution's damage to enemies inside it every second.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Templer_Retribution_Residue)]
	public class Templer_Retribution_ResidueOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float ResidueRange = 80f;
		private const float DamageRate = 0.10f;
		private static readonly TimeSpan ResidueDuration = TimeSpan.FromSeconds(10);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(ResidueRange);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = ResidueDuration;
			pad.Trigger.MaxActorCount = (int)pad.Skill.Properties.GetFloat(PropertyName.CaptionRatio);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			PadDamageEnemy(args.Trigger, DamageRate, multiHits: Templer_RetributionOverride.HitCount);
		}
	}
}
