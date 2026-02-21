using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.QuarrelShooter_DeployPavise)]
	public class QuarrelShooter_DeployPaviseOverride : ICreatePadHandler, IDestroyPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float ObstacleSize = 20f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetRange(40f);
			pad.SetUpdateInterval(1000);
			var time = 30000f;
			if (ZoneServer.Instance.World.IsPVP)
				time = 90000;
			if (creator.IsAbilityActive(AbilityId.QuarrelShooter24))
				time *= 0.5f;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(time);

			// Creates Obstacle
			var obstacleSize = ObstacleSize;
			var obstacleShape = new Circle(pad.Position, obstacleSize);
			var obstacle = new DynamicObstacle(pad.Position, obstacleShape, "Pavise");
			pad.Variables.Set("Pavise", obstacle);
			pad.Map.AddObstacle(obstacle);
			PadCreateObstacle(pad, obstacleSize, obstacleSize);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.DeployPavise_Buff);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.DeployPavise_Buff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadBuff(pad, RelationType.Friendly, 0, 0, BuffId.DeployPavise_Buff, 1, 0, 0, 1, 100);
		}
	}
}
