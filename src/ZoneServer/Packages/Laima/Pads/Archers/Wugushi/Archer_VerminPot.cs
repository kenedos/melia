using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Wugushi
{
	[Package("laima")]
	[PadHandler(PadName.Archer_VerminPot)]
	public class Archer_VerminPotOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const int MaxTargets = 3;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(40f);
			pad.SetUpdateInterval(750);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);

			PadCreateMonster(pad, "hidden_monster4", pad.Position, 0f, 0, 15f, "HitProof#YES", "None", 1, true, "None", "None", false, "SCR_ARRIVE_THROWGUPOT");
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			if (initiator.IsBuffActive(BuffId.Archer_VerminPot_Debuff))
				return;

			var damage = (int)SCR_SkillHit(creator, initiator, skill).Damage;
			if (damage <= 0)
				return;

			AddPadBuff(creator, initiator, pad, BuffId.Archer_VerminPot_Debuff, skill.Level, damage, 15000, 1, 100);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var targets = pad.Trigger.GetAttackableEntities(creator)
				.OrderBy(t => t.IsBuffActive(BuffId.Archer_VerminPot_Debuff) ? 1 : 0)
				.Take(MaxTargets);

			foreach (var target in targets)
			{
				if (target.IsBuffActive(BuffId.Archer_VerminPot_Debuff))
					continue;

				var damage = (int)SCR_SkillHit(creator, target, skill).Damage;
				if (damage <= 0)
					continue;

				AddPadBuff(creator, target, pad, BuffId.Archer_VerminPot_Debuff, skill.Level, damage, 15000, 1, 100);
			}
		}
	}
}
