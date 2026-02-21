using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Logging;

namespace Melia.Zone.Pads.Handlers.Scouts.Assassin
{
	/// <summary>
	/// Handler for the Assassin_HallucinationSmoke,
	/// which creates a fog of... hallucinating... smoke?
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Assassin_HallucinationSmoke)]
	public class Assassin_HallucinationSmokeOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		/// <summary>
		/// Called when the pad is created.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = args.Skill;

			pad.SetUpdateInterval(1000);
			pad.Position = creator.Position;
			pad.Trigger.MaxActorCount = 8;
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(skill.Level + 5);
			Send.ZC_NORMAL.PadUpdate(args.Creator, args.Trigger, PadName.Assassin_HallucinationSmoke, -0.7853982f, 0, 30, true);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var target = args.Initiator;
			var skill = args.Skill;

			if (pad.Trigger.AtCapacity)
				return;

			if (!creator.IsAlly(target))
				return;

			target.StartBuff(BuffId.HallucinationSmoke_Buff, skill.Level, 0, TimeSpan.FromSeconds(4), creator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = args.Skill;

			var targets = pad.Trigger.GetAlliedEntities(creator);
			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
			{
				if (pad.Trigger.AtCapacity)
					return;

				if (target.IsDead)
					continue;

				target.StartBuff(BuffId.HallucinationSmoke_Buff, skill.Level, 0, TimeSpan.FromSeconds(4), creator);
			}
		}

		/// <summary>
		/// Called when the pad is destroyed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Creator, args.Trigger, PadName.Assassin_HallucinationSmoke, 0, 145.8735f, 30, false);
		}
	}
}
