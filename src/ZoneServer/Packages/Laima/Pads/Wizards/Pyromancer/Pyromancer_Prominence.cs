using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.prominence)]
	public class Pyromancer_ProminenceOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int SimultaneousSalamanders = 5;
		private const float EffectRadius = 150f;
		private const float DamageWidth = 40f;
		private const int MaxTargetsPerSalamander = 4;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(EffectRadius);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
			pad.Trigger.MaxActorCount = 20;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (creator?.Map == null)
				return;

			for (var j = 0; j < SimultaneousSalamanders; j++)
			{
				var start = GetRandomPerimeterPosition(pad.Position, EffectRadius);
				var end = GetRandomPerimeterPosition(pad.Position, EffectRadius);
				this.CreateSalamanderEffect(creator, skill, start, end);
			}
		}

		private async void CreateSalamanderEffect(ICombatEntity caster, Skill skill, Position start, Position end)
		{
			var handleId = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_NORMAL.Skill_124(caster, handleId, "I_wizard_Prominence_force_fire3", 1.2f, "F_wizard_prominence_fire", 1, 90, 5, DamageWidth, 0.35f, 5, 0.35f, start, 2);
			Send.ZC_NORMAL.Skill_124(caster, handleId, start, end);

			this.ApplyLineDamage(caster, skill, start, end);

			await Task.Delay(2000);
			Send.ZC_NORMAL.Skill_124(caster, handleId);
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

				hitCount++;
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private static Position GetRandomPerimeterPosition(Position center, float radius)
		{
			var angle = RandomProvider.Get().NextDouble() * Math.PI * 2;
			var x = center.X + radius * (float)Math.Cos(angle);
			var z = center.Z + radius * (float)Math.Sin(angle);
			return new Position(x, center.Y, z);
		}
	}
}
