using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the [Arts] Entity: Search pad, which reveals the hidden
	/// enemies around the Exorcist and leaves them exposed for 5 seconds.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Exorcist_Entity_Abil)]
	public class Exorcist_Entity_AbilOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int SearchInterval = 500;
		private static readonly TimeSpan ExposedDuration = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(SearchInterval);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;

			if (caster.IsDead)
			{
				pad.Destroy();
				return;
			}

			foreach (var target in pad.Trigger.GetAttackableEntities(caster))
			{
				if (!target.IsBuffActiveByKeyword(BuffTag.Cloaking))
					continue;

				target.StopBuffByTag(BuffTag.Cloaking);
				target.StartBuff(BuffId.Entity_Pad_Debuff, pad.Skill.Level, 0, ExposedDuration, caster, pad.Skill.Id);
			}
		}
	}
}
