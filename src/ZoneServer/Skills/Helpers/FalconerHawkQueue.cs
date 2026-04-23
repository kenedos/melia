using System;
using System.Threading;
using System.Threading.Tasks;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Thin façade for interacting with the per-hawk skill queue, which
	/// lives in the hawk's AI script (PC_Pet_Hawk). Handlers enqueue
	/// requests through here without needing a direct reference to the
	/// scripted AI type.
	/// </summary>
	public static class FalconerHawkQueue
	{
		/// <summary>
		/// Enqueues a hawk-skill request on the given hawk's AI queue.
		/// Requests execute one at a time; a second cast of any hawk skill
		/// while one is in-flight will queue behind it.
		/// </summary>
		public static bool Enqueue(Companion hawk, HawkSkillRequest request)
		{
			if (TryGetQueue(hawk, out var queue))
			{
				queue.EnqueueHawkSkill(request);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Cancels the hawk's currently in-flight request (if any) without
		/// touching pending queued requests. Used for external interrupts
		/// such as stun, freeze, or state-lock where queued follow-ups
		/// should still run after the interrupt lifts.
		/// </summary>
		public static void CancelInFlight(Companion hawk)
		{
			if (TryGetQueue(hawk, out var queue))
				queue.CancelInFlightSkill();
		}

		/// <summary>
		/// Cancels the in-flight request for the caster's hawk (if they
		/// have one active). Convenience wrapper for StateLockComponent
		/// and death hooks that only have the caster reference.
		/// </summary>
		public static void CancelInFlightForCaster(ICombatEntity caster)
		{
			if (caster == null || !FalconerHawkHelper.TryGetHawk(caster, out var hawk))
				return;

			CancelInFlight(hawk);
		}

		/// <summary>
		/// Clears pending requests and cancels the in-flight request.
		/// Used on hawk death/reset.
		/// </summary>
		public static void Clear(Companion hawk)
		{
			if (TryGetQueue(hawk, out var queue))
				queue.ClearSkillQueue();
		}

		/// <summary>
		/// Returns true if the hawk's queue has recently started a skill
		/// and the 2.5s inter-skill global cooldown has not yet elapsed.
		/// </summary>
		public static bool IsOnGlobalCooldown(Companion hawk)
		{
			return TryGetQueue(hawk, out var queue) && queue.IsOnGlobalCooldown();
		}

		private static bool TryGetQueue(Companion hawk, out IHawkSkillQueue queue)
		{
			queue = null;
			if (hawk == null)
				return false;

			if (!hawk.Components.TryGet<AiComponent>(out var ai) || ai.Script == null)
				return false;

			queue = ai.Script as IHawkSkillQueue;
			return queue != null;
		}
	}

	/// <summary>
	/// Implemented by the hawk's AI script. Lets server-side handler code
	/// enqueue hawk-skill requests and signal cancellation without
	/// hard-referencing the scripted AI class.
	/// </summary>
	public interface IHawkSkillQueue
	{
		void EnqueueHawkSkill(HawkSkillRequest request);
		void CancelInFlightSkill();
		void ClearSkillQueue();
		bool IsOnGlobalCooldown();
	}

	/// <summary>
	/// A queued hawk-skill request. Execute contains only the hawk-side
	/// work (aerial animation, damage, effects); the AI-owned processor
	/// owns the busy flag, movement lock, and inter-skill GCD.
	/// </summary>
	public readonly record struct HawkSkillRequest(
		Skill Skill,
		ICombatEntity Caster,
		Func<HawkSkillContext, Task> Execute);

	/// <summary>
	/// Execution context passed to a hawk request's Execute callback.
	/// The Token fires when the request is externally cancelled (stun,
	/// freeze, death, state-lock). It is NOT tied to the caster-skill's
	/// shared _cts, so self-repeated casts do not abort the in-flight
	/// hawk work.
	/// </summary>
	public sealed class HawkSkillContext
	{
		public Skill Skill { get; }
		public ICombatEntity Caster { get; }
		public Companion Hawk { get; }
		public CancellationToken Token { get; }

		public HawkSkillContext(Skill skill, ICombatEntity caster, Companion hawk, CancellationToken token)
		{
			this.Skill = skill;
			this.Caster = caster;
			this.Hawk = hawk;
			this.Token = token;
		}

		public Task Delay(int milliseconds) => Task.Delay(milliseconds, this.Token);
		public Task Delay(TimeSpan time) => Task.Delay(time, this.Token);
	}
}
