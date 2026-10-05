using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Wizards.Sage;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Logging;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Sage
{
	[Package("laima")]
	[PadHandler(PadName.Sage_DimensionCompression_Abil)]
	public class Sage_DimensionCompressionPadOverride : ICreatePadHandler, IUpdatePadHandler, IDestroyPadHandler
	{
		private const float PullRadius = 180f;
		private const int MaximumTargets = 15;
		private const int UpdateMilliseconds = 250;
		private static readonly ConditionalWeakTable<Pad, Field> Fields = new();

		private sealed class Field
		{
			public readonly HashSet<ICombatEntity> Damaged = new();
			public readonly Stopwatch Clock = Stopwatch.StartNew();
			public Position FixedPosition;
			public double DurationMilliseconds;
			public long LastPulse = -UpdateMilliseconds;
			public bool Closed;
		}

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var field = Fields.GetValue(pad, _ => new Field());
			field.FixedPosition = pad.Position;
			field.DurationMilliseconds = GetDurationMilliseconds(pad);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(field.DurationMilliseconds);

			Log.Debug(
	$"[Sage Dimension Compression] PAD CREATED: SkillLevel={pad.Skill?.Level}, FieldDuration={field.DurationMilliseconds}ms, LifeTime={pad.Trigger.LifeTime.TotalMilliseconds}ms"
);

			pad.Trigger.MaxActorCount = MaximumTargets;
			pad.Trigger.MaxConcurrentUseCount = MaximumTargets;
			pad.SetRange(PullRadius);
			pad.SetUpdateInterval(UpdateMilliseconds);
			Send.ZC_NORMAL.PadUpdate(pad, true);
			this.Updated(sender, args);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			if (!Fields.TryGetValue(pad, out var field))
				return;

			try
			{
				lock (field)
				{
					if (field.Closed || pad.IsDead)
						return;

					if (pad.Creator is not ICombatEntity caster || caster.IsDead || pad.Map == null || caster.Map != pad.Map || pad.Skill == null || field.Clock.Elapsed.TotalMilliseconds >= field.DurationMilliseconds)
					{
						field.Closed = true;
						pad.Destroy();
						return;
					}

					var elapsed = field.Clock.ElapsedMilliseconds;

					if (elapsed - field.LastPulse < UpdateMilliseconds)
						return;

					field.LastPulse = elapsed;

					var skill = pad.Skill;
					var center = field.FixedPosition;

					if (pad.Position.Get2DDistance(center) > 0.1f)
					{
						pad.Position = center;
						Send.ZC_NORMAL.PadUpdate(pad, true);
					}

					var character = caster as Character;
					var damageMultiplier = 1f;

					if (character != null)
						damageMultiplier *= Sage_DimensionCompressionEnhanceAbility.GetDamageMultiplier(character);

					var targets = pad.Map.GetAttackableEnemiesIn(caster, pad.Area)
						.Where(t => !t.IsDead && t.Map == pad.Map && caster.IsEnemy(t))
						.Distinct()
						.OrderBy(t => t.Position.Get2DDistance(center))
						.Take(MaximumTargets)
						.ToList();

					var hits = new List<SkillHitInfo>();

					foreach (var target in targets)
					{
						if (pad.IsDead || caster.IsDead || caster.Map != pad.Map)
							break;

						if (target.IsDead || target.Map != pad.Map)
							continue;

						if (field.Damaged.Add(target))
						{
							var result = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(Math.Max(1, skill.Data.MultiHitCount)));
							result.Damage *= damageMultiplier;
							target.TakeDamage(result.Damage, caster);

							if (!target.IsDead && result.Result != HitResultType.Dodge && result.Damage > 0f && character != null)
							{
								var confusionDuration = Sage_DimensionCompressionAftermathAbility.GetConfusionDuration(character);

								if (confusionDuration > TimeSpan.Zero)
									target.StartBuff(BuffId.Confuse, skill.Level, 0, confusionDuration, character, skill.Id);
							}

							hits.Add(new SkillHitInfo(caster, target, skill, result, TimeSpan.Zero, TimeSpan.Zero));
						}

						if (!target.IsDead && target.Map == pad.Map && target.IsKnockdownable() && target.Position.Get2DDistance(center) > 2f)
						{
							target.SetPosition(center);
							Send.ZC_MOVE_STOP(target, center, 1);
						}
					}

					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(caster, hits);
				}
			}
			catch (Exception ex)
			{
				Log.Error("Sage Dimension Compression: {0}", ex);

				lock (field)
					field.Closed = true;

				pad.Destroy();
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			if (Fields.TryGetValue(pad, out var field))
			{
				lock (field)
				{
					field.Closed = true;
					field.Damaged.Clear();
				}

				Fields.Remove(pad);
			}

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		private static double GetDurationMilliseconds(Pad pad)
		{
			if (pad.Skill == null)
				return 4000;

			var level = Math.Clamp(pad.Skill.Level, 1, 10);
			return (4.0 + 0.4 * level) * 1000.0;
		}
	}
}
