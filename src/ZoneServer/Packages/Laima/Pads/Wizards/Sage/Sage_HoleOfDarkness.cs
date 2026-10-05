using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Logging;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Packages.Laima.Abilities.Wizards.Sage;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Sage
{
	[Package("laima")]
	[PadHandler(PadName.Sage_HoleOfDarkness)]
	public class Sage_HoleOfDarknessOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public const string HiddenEffectName = "Sage.Rupture.Hidden";
		private const int TickMilliseconds = 200;
		private static readonly object Sync = new();
		private static readonly ConditionalWeakTable<Pad, Field> Fields = new();

		private sealed class Field
		{
			public readonly HashSet<ICombatEntity> Targets = new();
			public int MaximumTicks;
			public int Ticks;
			public long Started;
			public bool Closed;
			public bool Enlarged;
			public ICombatEntity Owner;
		}

		public static void Configure(Pad pad, int stacks)
		{
			if (stacks < 1 || stacks > 10) throw new ArgumentOutOfRangeException(nameof(stacks));
			lock (Sync) Fields.Add(pad, new Field { MaximumTicks = stacks * 5 });
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(stacks * 1000);
			pad.Trigger.MaxActorCount = 20;
			pad.Trigger.MaxConcurrentUseCount = 20;
		}

		/// <summary>One enlargement per allied Rupture whose center is inside the Ultimate area.</summary>
		public static int EnlargeNearby(ICombatEntity caster, Melia.Shared.World.Position center, float range)
		{
			if (caster == null || caster.IsDead || caster.Map == null) return 0;
			var count = 0;
			lock (Sync)
			{
				foreach (var entry in Fields)
				{
					var pad = entry.Key;
					var field = entry.Value;
					if (field.Closed || field.Enlarged || field.Started == 0 || pad.IsDead || pad.Map != caster.Map) continue;
					if (field.Owner == null || field.Owner.IsDead || field.Owner.Map != pad.Map || caster.IsEnemy(field.Owner)) continue;
					if (pad.Position.Get2DDistance(center) > range) continue;
					var elapsed = (Stopwatch.GetTimestamp() - field.Started) * 1000.0 / Stopwatch.Frequency;
					if (elapsed >= field.MaximumTicks * TickMilliseconds) continue;
					// Absolute values and the flag prevent repeated casts from multiplying the radius again.
					pad.SetRange(180f);
					pad.Trigger.MaxActorCount = 30;
					pad.Trigger.MaxConcurrentUseCount = 30;
					field.Enlarged = true;
					Send.ZC_NORMAL.PadUpdate(pad, true);
					count++;
				}
			}
			return count;
		}

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			if (!Fields.TryGetValue(pad, out var field))
			{
				pad.Destroy();
				return;
			}
			pad.SetRange(120f);
			pad.SetUpdateInterval(TickMilliseconds);
			field.Owner = args.Creator;
			field.Started = Stopwatch.GetTimestamp();
			Send.ZC_NORMAL.PadUpdate(pad, true);
			// First tick on creation: N stacks produce up to 4*N ticks before expiry.
			this.Updated(sender, args);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			if (!Fields.TryGetValue(pad, out var field)) return;
			try
			{
				lock (Sync)
				{
					if (field.Closed) return;
					if (pad.IsDead || pad.Map == null || args.Creator == null || args.Creator.IsDead || args.Creator.Map != pad.Map)
					{
						Close(pad, field);
						pad.Destroy();
						return;
					}

					var elapsed = (Stopwatch.GetTimestamp() - field.Started) * 1000.0 / Stopwatch.Frequency;
					if (elapsed >= field.MaximumTicks * TickMilliseconds)
					{
						Close(pad, field);
						pad.Destroy();
						return;
					}

					var due = Math.Min(field.MaximumTicks, 1 + (int)(elapsed / TickMilliseconds));
					if (field.Ticks >= due) return;

					var caster = args.Creator;
					var character = caster as Character;
					var damageMultiplier = character != null ? Sage_HoleOfDarknessEnhanceAbility.GetDamageMultiplier(character) : 1f;
					var candidates = pad.Map.GetAttackableEnemiesIn(caster, pad.Area)
						.Where(t => t != null && !t.IsDead && t.Map == pad.Map && (t is Character || t is Mob))
						.Distinct().ToList();
					var available = new HashSet<ICombatEntity>(candidates);

					foreach (var target in field.Targets.ToArray())
					{
						if (!available.Contains(target)) Release(pad, field, target);
					}

					foreach (var target in candidates.OrderBy(t => t.Position.Get2DDistance(pad.Position)))
					{
						if (field.Targets.Count >= pad.Trigger.MaxActorCount) break;
						if (!field.Targets.Contains(target)) Acquire(pad, field, target);
					}

					// Skip missed intervals under load rather than deliver a burst of catch-up damage.
					field.Ticks = due;
					var hits = new List<SkillHitInfo>();

					foreach (var target in field.Targets.ToArray())
					{
						if (field.Closed || pad.IsDead) break;
						if (target.IsDead || target.Map != pad.Map)
						{
							Release(pad, field, target);
							continue;
						}

						var result = SCR_SkillHit(caster, target, pad.Skill, SkillModifier.Default);
						result.Damage *= damageMultiplier;
						var rawDamage = result.Damage;

						// TakeDamage applies the Rupture vulnerability. Never multiply the input by 1.30 here.
						target.TakeDamage(rawDamage, caster);

						// Display the pre-shield hit including the same vulnerability, without applying it again.
						if (rawDamage > 0)
							result.Damage = rawDamage * 1.30f;

						hits.Add(new SkillHitInfo(caster, target, pad.Skill, result));
						if (target.IsDead) Release(pad, field, target);
					}

					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(caster, hits);
				}
			}
			catch (Exception ex)
			{
				lock (Sync) Close(pad, field);
				pad.Destroy();
				Log.Error("Sage Rupture: field closed after an error: {0}", ex);
			}
		}

		private static void Acquire(Pad pad, Field field, ICombatEntity target)
		{
			if (!field.Targets.Add(target)) return;

			target.AddState(StateType.SageRupture);

			if (pad.Creator is Character character && character.TryGetActiveAbility(AbilityId.Sage20, out _))
			{
				target.AddState(StateType.SageBlackHole);
				target.AddEffect(HiddenEffectName, ColorEffect.FromRgba(1f, 1f, 1f, 0f));
			}
		}

		private static void Release(Pad pad, Field field, ICombatEntity target)
		{
			if (!field.Targets.Remove(target))
				return;

			target.RemoveState(StateType.SageRupture);
			target.RemoveState(StateType.SageBlackHole);

			if (target.Components.TryGet<EffectsComponent>(out var effects))
				effects.RemoveColorEffectAndRestore(HiddenEffectName);
		}

		private static void Close(Pad pad, Field field)
		{
			if (field.Closed) return;
			field.Closed = true;
			foreach (var target in field.Targets.ToArray())
			{
				try { Release(pad, field, target); }
				catch (Exception ex) { Log.Error("Sage Rupture: failed to release a target: {0}", ex); }
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			lock (Sync)
			{
				if (Fields.TryGetValue(pad, out var field)) Close(pad, field);
			}
			Send.ZC_NORMAL.PadUpdate(pad, false);
		}
	}
}
