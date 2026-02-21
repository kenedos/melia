using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Scripting.AI
{
	/// <summary>
	/// An alert about an event that happened to or around an AI's entity.
	/// </summary>
	public interface IAiEventAlert
	{
	}

	/// <summary>
	/// Information for the AI that the entity was hit.
	/// </summary>
	public class HitEventAlert : IAiEventAlert
	{
		/// <summary>
		/// Returns the target.
		/// </summary>
		public ICombatEntity Target { get; }

		/// <summary>
		/// Returns the attacker.
		/// </summary>
		public ICombatEntity Attacker { get; }

		/// <summary>
		/// Returns the damage dealt.
		/// </summary>
		public float Damage { get; }

		/// <summary>
		/// Creates new event.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="attacker"></param>
		/// <param name="damage"></param>
		public HitEventAlert(ICombatEntity target, ICombatEntity attacker, float damage)
		{
			this.Target = target;
			this.Attacker = attacker;
			this.Damage = damage;
		}
	}

	public class HateResetAlert : IAiEventAlert
	{
		/// <summary>
		/// Returns the entity for which the hate was reset.
		/// </summary>
		public ICombatEntity Target { get; }

		/// <summary>
		/// Creates new event.
		/// </summary>
		/// <param name="target"></param>
		public HateResetAlert(ICombatEntity target)
		{
			this.Target = target;
		}

		/// <summary>
		/// Creates new event.
		/// </summary>
		public HateResetAlert()
		{
			this.Target = null;
		}
	}

	public class HateIncreaseAlert : IAiEventAlert
	{
		/// <summary>
		/// Returns the entity for which the hate should increase
		/// </summary>
		public ICombatEntity Target { get; }

		/// <summary>
		/// Returns the amount of hate to gain.
		/// </summary>
		public float Amount { get; }

		/// <summary>
		/// Creates new event.
		/// </summary>
		/// <param name="target"></param>
		public HateIncreaseAlert(ICombatEntity target, float amount)
		{
			this.Target = target;
			this.Amount = amount;
		}
	}

	public class MoveToAlert : IAiEventAlert
	{
		public Position Position { get; }
		public float Speed { get; }
		public TimeSpan MoveTime { get; }
		public bool IgnoreHoldMove { get; }
		public bool SuspendAI { get; }

		/// <summary>
		/// Creates new event.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="speed"></param>
		public MoveToAlert(Position position, float speed)
		{
			this.Position = position;
			this.Speed = speed;
		}


		/// <summary>
		/// Creates new event.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="speed"></param>
		/// <param name="moveTime"></param>
		/// <param name="ignoreHoldMove"></param>
		/// <param name="monAiHold"></param>
		public MoveToAlert(Position position, float speed, float moveTime, bool ignoreHoldMove = false, bool monAiHold = false)
		{
			this.Position = position;
			this.Speed = speed;
			this.MoveTime = TimeSpan.FromMilliseconds(moveTime);
			this.IgnoreHoldMove = ignoreHoldMove;
			this.SuspendAI = monAiHold;
		}
	}

	public class CancelSkillAlert : IAiEventAlert
	{
	}

	/// <summary>
	/// Alert to suspend AI processing (hold AI).
	/// </summary>
	public class SuspendAiAlert : IAiEventAlert
	{
	}

	/// <summary>
	/// Alert to resume AI processing (unhold AI).
	/// </summary>
	public class ResumeAiAlert : IAiEventAlert
	{
	}

	/// <summary>
	/// Alert to knockdown the entity.
	/// </summary>
	public class KnockdownAlert : IAiEventAlert
	{
		/// <summary>
		/// Returns the direction of the knockdown.
		/// </summary>
		public Direction Direction { get; }

		/// <summary>
		/// Returns the position to knockdown to (optional).
		/// </summary>
		public Position? Position { get; }

		/// <summary>
		/// Creates new knockdown event with direction.
		/// </summary>
		/// <param name="direction"></param>
		public KnockdownAlert(Direction direction)
		{
			this.Direction = direction;
			this.Position = null;
		}

		/// <summary>
		/// Creates new knockdown event with target position.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="direction"></param>
		public KnockdownAlert(Position position, Direction direction)
		{
			this.Position = position;
			this.Direction = direction;
		}
	}

	public class ChangeTendencyEventAlert : IAiEventAlert
	{
		/// <summary>
		/// New tendency
		/// </summary>
		public TendencyType Tendency { get; }

		/// <summary>
		/// Creates new event.
		/// </summary>
		/// <param name="target"></param>
		public ChangeTendencyEventAlert(TendencyType tendency)
		{
			this.Tendency = tendency;
		}
	}
}
