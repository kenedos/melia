using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Cryomancer
{
	/// <summary>
	/// Handler for the Cryomancer skill Ice Wall.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cryomancer_IceWall)]
	public class Cryomancer_IceWallOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int IceWallDuration = 15;
		private const int IceWallSize = 25;

		/// <summary>
		/// Start casting.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.InSkillUseRange(skill, targetPos))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if (caster.Map.IsCity)
			{
				caster.ServerMessage(Localization.Get("Cannot use this skill in cities."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, targetPos);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos);

			this.ExecuteIceWall(caster, skill, targetPos);
		}

		/// <summary>
		/// Execute Fire Wall
		/// </summary>
		private void ExecuteIceWall(ICombatEntity caster, Skill skill, Position position)
		{
			var direction = caster.Position.GetDirection(position).AddDegreeAngle(90);
			var duration = TimeSpan.FromSeconds(IceWallDuration);
			var size = IceWallSize;

			var map = caster.Map;

			this.CreateIcewallRow(caster, skill, position, direction, duration, size, 5);
		}

		/// <summary>
		/// Creates a row of Ice Wall pads.
		/// </summary>
		private void CreateIcewallRow(ICombatEntity caster, Skill skill, Position centerPosition, Direction direction, TimeSpan duration, float size, int wallCount)
		{
			for (var i = 0; i < wallCount; i++)
			{
				var effectPosition = centerPosition;

				if (wallCount > 1)
				{
					var offset = (i - (wallCount - 1) / 2.0f) * size;
					effectPosition = centerPosition.GetRelative(direction, offset);
				}

				this.CreateIcewallPad(caster, skill, effectPosition, direction, duration, size);
			}
		}

		/// <summary>
		/// Creates a single Ice Wall pad.
		/// </summary>
		private void CreateIcewallPad(ICombatEntity caster, Skill skill, Position position, Direction direction, TimeSpan duration, float size)
		{
			var pad = new Pad(PadName.Cryomancer_IceWall, caster, skill, new Circle(position, size));
			pad.Position = position;
			pad.Direction = direction;
			pad.Trigger.LifeTime = duration;
			pad.Trigger.MaxUseCount = 1;

			caster.Map.AddPad(pad);
		}
	}
}
