using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Pyromancer
{
	/// <summary>
	/// Handler for the Pyromancer skill Prominence.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pyromancer_Prominence)]
	public class Pyromancer_ProminenceOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int SalamanderCount = 10;
		private const int SimultaneousSalamanders = 5;
		private const float EffectRadius = 150f;
		private const float DamageWidth = 40f;
		private const int MaxTargetsPerSalamander = 4;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

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

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, null);

			this.InitiateSkill(skill, caster, targetPos);
			this.SpawnSalamanders(skill, caster, targetPos);
		}

		private void InitiateSkill(Skill skill, ICombatEntity caster, Position position)
		{
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_NORMAL.RunPad(caster, skill, "prominence", position, caster.Direction, -1, 25, skillHandle, 0);
			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);
		}

		private void SpawnSalamanders(Skill skill, ICombatEntity caster, Position position)
		{
			for (var i = 0; i < SalamanderCount; i++)
			{
				skill.Wait(TimeSpan.FromMilliseconds(1000 * (i + 1))).ContinueWith(_ =>
				{
					for (var j = 0; j < SimultaneousSalamanders; j++)
					{
						var start = this.GetRandomPerimeterPosition(caster.Map, position, EffectRadius);
						var end = this.GetRandomPerimeterPosition(caster.Map, position, EffectRadius);
						this.CreateSalamanderEffect(caster, skill, start, end);

					}
				});
			}
		}

		private void CreateSalamanderEffect(ICombatEntity caster, Skill skill, Position start, Position end)
		{
			var handleId = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_NORMAL.Skill_124(caster, handleId, "I_wizard_Prominence_force_fire3", 1.2f, "F_wizard_prominence_fire", 1, 90, 5, DamageWidth, 0.35f, 5, 0.35f, start, 2);
			Send.ZC_NORMAL.Skill_124(caster, handleId, start, end);

			this.ApplyLineDamage(caster, skill, start, end);
		}

		private void ApplyLineDamage(ICombatEntity caster, Skill skill, Position start, Position end)
		{
			var direction = start.GetDirection(end);
			var distance = start.Get2DDistance(end);
			var rectangle = new Square(start, direction, (float)distance, DamageWidth);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, rectangle);
			var hits = new List<SkillHitInfo>();
			var hitCount = 0;

			foreach (var target in targets)
			{
				if (hitCount >= MaxTargetsPerSalamander)
					break;

				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				hits.Add(skillHit);

				target.StartBuff(BuffId.Fire, skill.Level, skillHitResult.Damage, TimeSpan.FromSeconds(3), caster);
				hitCount++;
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private Position GetRandomPerimeterPosition(Map map, Position center, float radius)
		{
			var angle = RandomProvider.Get().NextDouble() * Math.PI * 2;
			var x = center.X + radius * (float)Math.Cos(angle);
			var z = center.Z + radius * (float)Math.Sin(angle);
			return new Position(x, center.Y, z);
		}
	}
}
