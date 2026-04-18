using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[PadHandler(PadName.Necromancer_DirtyWall)]
	public class Necromancer_DirtyWallOverride : ICreatePadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			pad.SetRange(30f);
			pad.SetUpdateInterval(750);

			var lifeTime = 20000 + skill.Level * 2000;
			if (ZoneServer.Instance.World.IsPVP)
				lifeTime = 900000;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(lifeTime);
			PadCreateMonster(pad, "pcskill_dirtypole", pad.Position, 90f, 0, 60000f, "!SCR_SUMMON_DIRTYWALL#1", "None", 1, false, "NECRO_DIRTYWALL", "INIT_DIRTYWALL_MONSTER");
			PadCreateObstacle(pad, 15f, 15f);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			var buff = BuffId.Common_Rotten;
			if (creator.GetAbilityLevel(AbilityId.Necromancer16) > 0)
				buff = BuffId.Common_HighRotten;
			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, 0, buff, skill.Level, 0, 14000 + skill.Level * 1000, 0, 100, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
		}
	}
}
