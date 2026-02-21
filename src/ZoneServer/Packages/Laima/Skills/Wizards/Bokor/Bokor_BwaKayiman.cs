using System;
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
	/// Summons circle around the caster counter-clockwise, dealing damage to enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_BwaKayiman)]
	public class Bokor_BwaKayimanOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float CircleRadius = 50f;
		private const int PadRadius = 30;
		private const float RotationSpeed = 10f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);

			if (caster is not Character character)
				return;


			if (!caster.TryGetBuff(BuffId.PowerOfDarkness_Buff, out var darkForceBuff) || darkForceBuff.OverbuffCounter < 10)
			{
				caster.ServerMessage(Localization.Get("Requires at least 10 stacks of Dark Force."));
				return;
			}

			darkForceBuff.OverbuffCounter -= 10;
			darkForceBuff.NotifyUpdate();

			var summons = character.Summons.GetSummons();
			if (summons.Count == 0)
			{
				caster.ServerMessage(Localization.Get("No summons available."));
				return;
			}

			var angleIncrement = 360f / summons.Count;
			var currentAngle = 0f;

			for (int i = 0; i < summons.Count; i++)
			{
				var summon = summons[i];

				summon.StartBuff(BuffId.BwaKayiman_Fluting, TimeSpan.FromSeconds(10));

				summon.Vars.Set("BwaKayiman_CurrentAngle", currentAngle);

				var radians = currentAngle * (float)(Math.PI / 180.0);
				var offsetX = 40f * (float)Math.Cos(radians);
				var offsetZ = 40f * (float)Math.Sin(radians);

				var targetPos = new Position(
					caster.Position.X + offsetX,
					caster.Position.Y,
					caster.Position.Z + offsetZ
				);

				summon.Components.Get<MovementComponent>()?.MoveTo(targetPos);

				var radius = PadRadius;
				if (summon.EffectiveSize >= SizeType.L)
					radius *= 2;
				var pad = new Pad(PadName.Bokor_BwaKayiman_Fluting, caster, skill, new CircleF(targetPos, radius));
				pad.Position = targetPos;
				pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
				pad.Variables.Set("BwaKayiman_SummonHandle", summon.Handle);

				caster.Map.AddPad(pad);

				currentAngle += angleIncrement;
			}

			skill.Run(this.UpdateSummonPositions(skill, caster));
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);

			if (caster is not Character character)
				return;

			var summons = character.Summons.GetSummons();
			foreach (var summon in summons)
			{
				if (summon.IsDead)
					continue;

				summon.StopBuff(BuffId.BwaKayiman_Fluting);

				summon.Vars.Remove("BwaKayiman_CurrentAngle");
			}
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
		}

		private async Task UpdateSummonPositions(Skill skill, ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			while (caster.IsCasting(skill))
			{
				var summons = character.Summons.GetSummons();

				foreach (var summon in summons)
				{
					if (summon.IsDead)
						continue;

					if (!summon.Vars.TryGet("BwaKayiman_CurrentAngle", out float currentAngle))
						continue;

					currentAngle += 5f;
					if (currentAngle >= 360f)
						currentAngle -= 360f;

					summon.Vars.Set("BwaKayiman_CurrentAngle", currentAngle);

					var radians = currentAngle * (float)(Math.PI / 180.0);
					var offsetX = 40f * (float)Math.Cos(radians);
					var offsetZ = 40f * (float)Math.Sin(radians);

					var targetPos = new Position(
						caster.Position.X + offsetX,
						caster.Position.Y,
						caster.Position.Z + offsetZ
					);

					summon.Components.Get<MovementComponent>()?.MoveTo(targetPos);
				}

				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}

	}
}
