using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	[Package("laima")]
	[SkillHandler(SkillId.Sage_HoleOfDarkness)]
	public class Sage_RuptureOverride : IGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const double BaseCastMilliseconds = 8000;
		private const double PacketGraceMilliseconds = 2000;
		private const float AreaRadius = 120f;
		private const float MaximumCastMovement = 1f;
		private static readonly ConditionalWeakTable<Character, CastState> States = new();

		private sealed class CastState
		{
			public Skill Skill;
			public object Map;
			public Position StartPosition;
			public Position Destination;
			public long Started;
			public long EndedAt;
			public double RequiredMilliseconds;
			public bool Active;
			public bool Ended;
			public bool HasRequest;
		}

		private static double Elapsed(long started) => (Stopwatch.GetTimestamp() - started) * 1000.0 / Stopwatch.Frequency;

		private static double GetCastMilliseconds(Character character)
		{
			var speed = character.Properties.GetFloat("SPEED_BM");
			if (float.IsNaN(speed) || float.IsInfinity(speed)) return BaseCastMilliseconds;
			return Math.Max(0, BaseCastMilliseconds * (100.0 - speed) / 100.0);
		}

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster is not Character character || character.IsDead || character.Map == null) return;
			if (SageRuptureHelper.GetStacks(character) <= 0)
			{
				character.ServerMessage(Localization.Get("Rupture requires at least one stack."));
				Release(character, skill, true);
				return;
			}
			var state = States.GetValue(character, _ => new CastState());
			lock (state)
			{
				state.Skill = skill;
				state.Map = character.Map;
				state.StartPosition = character.Position;
				state.Started = Stopwatch.GetTimestamp();
				state.RequiredMilliseconds = GetCastMilliseconds(character);
				state.Active = true;
				state.Ended = false;
				state.HasRequest = false;
			}
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float castTime)
		{
			if (caster is not Character character || !States.TryGetValue(character, out var state))
				return;

			lock (state)
			{
				if (!state.Active || state.Skill != skill || state.Ended)
					return;

				if (!IsValid(character, state))
				{
					state.Active = false;
					Release(character, skill, true);
					return;
				}

				state.Ended = true;
				state.EndedAt = Stopwatch.GetTimestamp();

				TryComplete(character, skill, state);
			}
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead || character.Map == null) return;
			var targetPosition = GetTargetPosition(skill, farPos);
			if (!States.TryGetValue(character, out var state) || !state.Active)
			{
				// Some clients skip dynamic-cast packets when cast time reaches zero.
				if (GetCastMilliseconds(character) > 0) return;
				StartDynamicCast(skill, character, 0);
				state = States.GetValue(character, _ => new CastState());
				if (!state.Active) return;
				lock (state)
				{
					state.Ended = true;
					state.EndedAt = Stopwatch.GetTimestamp();
				}
			}
			lock (state)
			{
				if (!state.Active || state.Skill != skill || state.HasRequest) return;
				if (!IsValid(character, state) || !character.InSkillUseRange(skill, targetPosition))
				{
					state.Active = false;
					Release(character, skill, true);
					return;
				}
				state.Destination = targetPosition;
				state.HasRequest = true;
				TryComplete(character, skill, state);
			}
		}

		private static Position GetTargetPosition(Skill skill, Position farPos)
		{
			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				return targetPosition;
			return farPos;
		}

		private static bool IsValid(Character character, CastState state)
		{
			return !character.IsDead && character.Map != null && ReferenceEquals(character.Map, state.Map)
				&& character.Position.Get2DDistance(state.StartPosition) <= MaximumCastMovement
				&& Elapsed(state.Started) <= state.RequiredMilliseconds + PacketGraceMilliseconds
				&& (!state.Ended || Elapsed(state.EndedAt) <= PacketGraceMilliseconds);
		}

		private static void TryComplete(Character character, Skill skill, CastState state)
		{
			if (!state.Ended || !state.HasRequest) return;
			// Consume the ticket BEFORE damage callbacks; duplicate/reentrant requests cannot reuse it.
			state.Active = false;
			var completed = false;
			try
			{
				if (!IsValid(character, state) || !character.InSkillUseRange(skill, state.Destination)) return;
				if (SageRuptureHelper.GetStacks(character) <= 0)
				{
					character.ServerMessage(Localization.Get("Rupture requires at least one stack."));
					return;
				}
				var pad = new Pad(PadName.Sage_HoleOfDarkness, character, skill, new Circle(state.Destination, AreaRadius));
				pad.Position = state.Destination;
				pad.Direction = character.Direction;
				if (!SageRuptureHelper.TrySpendAndConsume(character, skill, out var consumed))
				{
					character.ServerMessage(Localization.Get("Not enough SP or Rupture stacks."));
					return;
				}
				Melia.Zone.Pads.HandlersOverride.Wizards.Sage.Sage_HoleOfDarknessOverride.Configure(pad, consumed);
				character.SetAttackState(true);
				character.TurnTowards(state.Destination);
				var origin = character.Position;
				Send.ZC_SKILL_READY(character, skill, origin, state.Destination);
				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, origin, origin.GetDirection(state.Destination), Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, state.Destination);
				character.Map.AddPad(pad);
				skill.IncreaseOverheat();
				completed = true;
			}
			finally { Release(character, skill, !completed); }
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			if (caster is not Character character) return;
			InvalidatePendingCast(character);
			Release(character, skill, true);
		}

		/// <summary>Also call from the common casting/skill-use and cancellation entry points (README).</summary>
		public static void InvalidatePendingCast(Character character)
		{
			if (character == null || !States.TryGetValue(character, out var state)) return;
			lock (state) state.Active = false;
		}

		private static void Release(Character character, Skill skill, bool cancelled)
		{
			if (character.IsCasting(skill)) character.SetCastingState(false, skill);
			if (character.Variables.Temp.TryGet<Skill>("Melia.Cast.Skill", out var current) && current == skill)
				character.Variables.Temp.Remove("Melia.Cast.Skill");
			character.SetAttackState(false);
			if (cancelled) Send.ZC_SKILL_DISABLE(character);
		}
	}
}
