using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Yggdrasil.Logging;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	public static class SageBlinkHelper
	{
		public const string CloneFlag = "Sage.Blink.Clone";
		private const float EchoRatio = 0.30f;
		private const float DurationSeconds = 15f;
		private const float ExplosionRadius = 90f;
		private const float ConfusionDurationSeconds = 3f;
		private static readonly ConditionalWeakTable<Character, State> States = new();
		[ThreadStatic] private static int SuppressionDepth;

		private sealed class State
		{
			public DummyCharacter Clone1;
			public DummyCharacter Clone2;
			public Skill Skill;
			public readonly Stopwatch Clock = Stopwatch.StartNew();
			public readonly Queue<Echo> Pending = new();
			public bool Closed;
		}

		private sealed class Echo
		{
			public ICombatEntity Target;
			public float Damage;
		}

		public static bool IsBlinkClone(DummyCharacter clone)
		{
			return clone != null && clone.Variables.Temp.GetBool(CloneFlag);
		}

		public static void Create(Character owner, Skill skill, Position position)
		{
			var clone1Position = new Position(position.X - 40f, position.Y, position.Z);
			var clone2Position = new Position(position.X + 40f, position.Y, position.Z);
			var clone1 = (DummyCharacter)owner.Clone(clone1Position, isSageBlink: true);
			DummyCharacter clone2 = null;
			var registered = false;
			try
			{
				clone2 = (DummyCharacter)owner.Clone(clone2Position, isSageBlink: true);
				var state = new State
				{
					Clone1 = clone1,
					Clone2 = clone2,
					Skill = skill
				};
				States.Add(owner, state);
				registered = true;
				SetupClone(owner, clone1);
				SetupClone(owner, clone2);
			}
			catch
			{
				if (registered)
					Remove(owner);
				else
				{
					CleanupClone(clone1);
					CleanupClone(clone2);
				}
				throw;
			}
		}

		private static void SetupClone(Character owner, DummyCharacter clone)
		{
			clone.MirrorDamageToOwner = false;
			clone.Lock(LockType.GetTargeted);
			clone.Lock(LockType.GetDamaged);
			Send.ZC_PLAY_ANI(clone, "BORN", false);
		}

		// Called AFTER a successful HP decrease, with damage captured at TakeDamage entry.
		// Each living clone reproduces 30% of the original input damage.
		public static void RecordDamage(ICombatEntity attacker, ICombatEntity target, float inputDamage, float hpLost)
		{
			if (SuppressionDepth != 0 || attacker is not Character owner || owner is DummyCharacter || owner.IsDead || hpLost <= 0 || !float.IsFinite(inputDamage) || inputDamage <= 0)
				return;

			if (target == null || target.IsDead || target.Properties.GetFloat(PropertyName.HP) <= 0 || target.Map != owner.Map || !owner.IsEnemy(target))
				return;

			if (!States.TryGetValue(owner, out var state))
				return;

			lock (state)
			{
				if (state.Closed || state.Clock.Elapsed.TotalSeconds >= DurationSeconds)
					return;

				var clone1Active = IsCloneActive(state.Clone1, owner);
				var clone2Active = IsCloneActive(state.Clone2, owner);

				if (!clone1Active && !clone2Active)
					return;

				state.Pending.Enqueue(new Echo
				{
					Target = target,
					Damage = inputDamage * EchoRatio
				});
			}
		}

		// Called by Character.Update; no Task.Delay and no skill cancellation token.
		public static void Update(Character owner)
		{
			if (owner is DummyCharacter || !States.TryGetValue(owner, out var state))
				return;

			try
			{
				lock (state)
				{
					if (state.Closed)
						return;

					if (owner.IsDead || owner.Map == null)
					{
						Remove(owner);
						return;
					}

					var clone1Active = IsCloneActive(state.Clone1, owner);
					var clone2Active = IsCloneActive(state.Clone2, owner);

					if (!clone1Active && !clone2Active)
					{
						Remove(owner);
						return;
					}

					if (state.Clock.Elapsed.TotalSeconds >= DurationSeconds)
					{
						Position? clone1Position = clone1Active ? state.Clone1.Position : null;
						Position? clone2Position = clone2Active ? state.Clone2.Position : null;
						var skill = state.Skill;

						Remove(owner);

						if (clone1Position.HasValue)
							Explode(owner, skill, clone1Position.Value);

						if (clone2Position.HasValue)
							Explode(owner, skill, clone2Position.Value);

						return;
					}

					while (state.Pending.Count > 0)
					{
						if (owner.IsDead || state.Closed)
							break;

						var echo = state.Pending.Dequeue();
						var target = echo.Target;

						if (target == null || target.IsDead || target.Map != owner.Map || !owner.IsEnemy(target))
							continue;

						clone1Active = IsCloneActive(state.Clone1, owner);
						clone2Active = IsCloneActive(state.Clone2, owner);

						if (clone1Active)
							ApplyEcho(owner, state.Clone1, target, echo.Damage);

						if (!target.IsDead && clone2Active)
							ApplyEcho(owner, state.Clone2, target, echo.Damage);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Sage Blink update: {0}", ex);
				Remove(owner);
			}
		}

		private static void ApplyEcho(Character owner, DummyCharacter clone, ICombatEntity target, float damage)
		{
			if (clone == null || target == null || target.IsDead || clone.Map != owner.Map || target.Map != owner.Map)
				return;

			var hpBefore = target.Properties.GetFloat(PropertyName.HP);

			SuppressionDepth++;
			try
			{
				target.TakeDamage(damage, owner);
			}
			finally
			{
				SuppressionDepth--;
			}

			var actualDamage = Math.Max(0f, hpBefore - target.Properties.GetFloat(PropertyName.HP));

			if (actualDamage > 0)
				Send.ZC_HIT_INFO(clone, target, new HitInfo(clone, target, actualDamage, HitResultType.Hit));
		}

		private static void Explode(Character owner, Skill skill, Position center)
		{
			if (owner == null || owner.IsDead || owner.Map == null)
				return;

			var targets = owner.Map.GetAttackableEnemiesIn(owner, new Circle(center, ExplosionRadius))
				.Where(target => !target.IsDead)
				.Distinct()
				.OrderBy(target => target.Position.Get2DDistance(center))
				.Take(15)
				.ToArray();

			SuppressionDepth++;

			try
			{
				foreach (var target in targets)
				{
					if (owner.IsDead || target.IsDead || target.Map != owner.Map)
						continue;

					var result = SCR_SkillHit(owner, target, skill, SkillModifier.Default);

					target.TakeDamage(result.Damage, owner);
					Send.ZC_HIT_INFO(owner, target, new HitInfo(owner, target, result.Damage, result.Result));

					if (!target.IsDead && result.Damage > 0 && result.Result != HitResultType.Dodge)
					{
						target.StartBuff(
							BuffId.Confuse,
							skill.Level,
							0,
							TimeSpan.FromSeconds(ConfusionDurationSeconds),
							owner,
							skill.Id
						);
					}
				}
			}
			finally
			{
				SuppressionDepth--;
			}
		}

		private static bool IsCloneActive(DummyCharacter clone, Character owner)
		{
			return clone != null &&
				!clone.IsDead &&
				clone.Map != null &&
				clone.Map == owner.Map;
		}

		private static void CleanupClone(DummyCharacter clone)
		{
			if (clone == null)
				return;

			if (clone.Map != null)
				clone.Despawn();
		}

		public static void Remove(Character owner)
		{
			if (owner == null || !States.TryGetValue(owner, out var state))
				return;

			lock (state)
			{
				if (state.Closed)
					return;

				state.Closed = true;
				state.Pending.Clear();
				States.Remove(owner);

				CleanupClone(state.Clone1);
				CleanupClone(state.Clone2);
			}
		}

		public static void OnCloneDespawn(DummyCharacter clone)
		{
			if (!IsBlinkClone(clone) || clone.Owner is not Character owner || !States.TryGetValue(owner, out var state))
				return;

			lock (state)
			{
				if (state.Closed)
					return;

				var isClone1 = state.Clone1 == clone;
				var isClone2 = state.Clone2 == clone;

				if (!isClone1 && !isClone2)
					return;

				if (isClone1)
					state.Clone1 = null;

				if (isClone2)
					state.Clone2 = null;

				var clone1Active = IsCloneActive(state.Clone1, owner);
				var clone2Active = IsCloneActive(state.Clone2, owner);

				if (clone1Active || clone2Active)
					return;

				state.Closed = true;
				state.Pending.Clear();
				States.Remove(owner);
			}
		}
	}
}
