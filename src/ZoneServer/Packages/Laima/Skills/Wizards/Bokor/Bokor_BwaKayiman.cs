using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.Pads;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Bwa Kayiman.
	/// Zombies form a conga line behind the caster, dealing damage to enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_BwaKayiman)]
	public class Bokor_BwaKayimanOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int PadRadius = 30;
		private const float FollowSpacing = 40f;
		private const int UpdateIntervalMs = 200;
		private const int MaxPositionHistory = 50;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if (!caster.TryGetBuff(BuffId.PowerOfDarkness_Buff, out var darkForceBuff) || darkForceBuff.OverbuffCounter < 10)
			{
				caster.ServerMessage(Localization.Get("Requires at least 10 stacks of Dark Force."));
				return;
			}

			darkForceBuff.OverbuffCounter -= 10;
			darkForceBuff.NotifyUpdate();

			skill.IncreaseOverheat();

			var summons = character.Summons.GetSummons();
			if (summons.Count == 0)
			{
				caster.ServerMessage(Localization.Get("No summons available."));
				return;
			}

			// Initialize position history with positions behind the caster
			var positionHistory = new List<Position>();
			var behindDir = caster.Direction.Backwards;
			for (var i = 0; i < summons.Count; i++)
			{
				var offset = FollowSpacing * (i + 1);
				positionHistory.Add(caster.Position.GetRelative(behindDir, offset));
			}

			skill.Vars.Set("Melia.Skills.BwaKayiman.PositionHistory", positionHistory);

			// Store ordered summon handles so positions stay fixed
			var summonHandles = new List<int>();
			for (var i = 0; i < summons.Count; i++)
				summonHandles.Add(summons[i].Handle);
			skill.Vars.Set("Melia.Skills.BwaKayiman.SummonOrder", summonHandles);

			// Place each summon at its initial line position
			for (var i = 0; i < summons.Count; i++)
			{
				var summon = summons[i];

				summon.StartBuff(BuffId.BwaKayiman_Fluting, TimeSpan.FromSeconds(10));

				summon.Components.Get<MovementComponent>()?.MoveTo(positionHistory[i]);

				var radius = PadRadius;
				if (summon.EffectiveSize >= SizeType.L)
					radius *= 2;

				var pad = new Pad(PadName.Bokor_BwaKayiman_Fluting, caster, skill, new CircleF(positionHistory[i], radius));
				pad.Position = positionHistory[i];
				pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
				pad.Variables.Set("Melia.Skills.BwaKayiman.SummonHandle", summon.Handle);

				caster.Map.AddPad(pad);
			}

			skill.Run(this.UpdateCongaLine(skill, caster));
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster is not Character character)
				return;

			var summons = character.Summons.GetSummons();
			foreach (var summon in summons)
			{
				if (summon.IsDead)
					continue;

				summon.StopBuff(BuffId.BwaKayiman_Fluting);
			}

			skill.Vars.Remove("Melia.Skills.BwaKayiman.PositionHistory");
			skill.Vars.Remove("Melia.Skills.BwaKayiman.SummonOrder");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
		}

		private async Task UpdateCongaLine(Skill skill, ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			while (caster.IsCasting(skill))
			{
				if (!skill.Vars.TryGet("Melia.Skills.BwaKayiman.PositionHistory", out List<Position> positionHistory))
					break;

				if (!skill.Vars.TryGet("Melia.Skills.BwaKayiman.SummonOrder", out List<int> summonHandles))
					break;

				positionHistory.Insert(0, caster.Position);
				if (positionHistory.Count > MaxPositionHistory)
					positionHistory.RemoveAt(positionHistory.Count - 1);

				// Move each summon using the fixed order assigned at start
				for (var i = 0; i < summonHandles.Count; i++)
				{
					var monster = character.Map.GetMonster(summonHandles[i]);
					if (monster is not ICombatEntity summon || summon.IsDead)
						continue;

					var targetPos = this.GetTrailPosition(positionHistory, (i + 1) * FollowSpacing);
					summon.Components.Get<MovementComponent>()?.MoveTo(targetPos);
				}

				await skill.Wait(TimeSpan.FromMilliseconds(UpdateIntervalMs));
			}
		}

		/// <summary>
		/// Walks along the position history trail and returns the position
		/// at the given distance from the front (caster's current position).
		/// </summary>
		private Position GetTrailPosition(List<Position> history, float targetDistance)
		{
			var distanceTraveled = 0f;

			for (var i = 0; i < history.Count - 1; i++)
			{
				var segmentLength = (float)history[i].Get2DDistance(history[i + 1]);
				if (segmentLength < 0.1f)
					continue;

				if (distanceTraveled + segmentLength >= targetDistance)
				{
					// Interpolate between these two points
					var remaining = targetDistance - distanceTraveled;
					var t = remaining / segmentLength;
					return new Position(
						(float)(history[i].X + (history[i + 1].X - history[i].X) * t),
						(float)(history[i].Y + (history[i + 1].Y - history[i].Y) * t),
						(float)(history[i].Z + (history[i + 1].Z - history[i].Z) * t)
					);
				}

				distanceTraveled += segmentLength;
			}

			// If trail isn't long enough, return the last known position
			return history[history.Count - 1];
		}
	}
}
