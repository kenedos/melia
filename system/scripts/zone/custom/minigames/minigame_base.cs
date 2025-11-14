using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Spawning;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

/// <summary>
/// Base class for all minigame scripts.
/// </summary>
public abstract class MinigameBase : IMinigameInstance
{
	private static int _nextId = 1;

	public int Id { get; }
	public Map Map { get; }
	public Position SpawnPosition { get; }
	public Direction SpawnDirection { get; }

	protected Random Rnd { get; }
	protected TimeSpan ElapsedTime { get; private set; }
	protected bool IsActive { get; set; }

	public event Action OnEnd;

	protected MinigameBase(Map map, Position position, Direction direction)
	{
		this.Id = _nextId++;
		this.Map = map;
		this.SpawnPosition = position;
		this.SpawnDirection = direction;
		this.Rnd = new Random(RandomProvider.GetSeed());
		this.ElapsedTime = TimeSpan.Zero;
		this.IsActive = false;
	}

	public virtual void Start()
	{
		this.IsActive = true;
		this.OnStart();
	}

	public virtual void Update(TimeSpan elapsed)
	{
		if (!this.IsActive)
			return;

		this.ElapsedTime += elapsed;
		this.OnUpdate(elapsed);
	}

	protected virtual void OnStart()
	{
	}

	protected virtual void OnUpdate(TimeSpan elapsed)
	{
	}

	public void End()
	{
		if (!this.IsActive)
			return;

		this.IsActive = false;
		this.OnEnd?.Invoke();
		this.OnEnded();
	}

	protected virtual void OnEnded()
	{
	}

	/// <summary>
	/// Gets all characters within range of a position.
	/// </summary>
	protected List<Character> GetCharactersInRange(Position position, float range)
	{
		var result = new List<Character>();

		foreach (var character in this.Map.GetCharacters())
		{
			var distance = Math.Sqrt(
				Math.Pow(character.Position.X - position.X, 2) +
				Math.Pow(character.Position.Z - position.Z, 2));

			if (distance <= range)
				result.Add(character);
		}

		return result;
	}

	/// <summary>
	/// Sends a notice message to all characters in range using MGameMessage.
	/// </summary>
	/// <param name="position">The position to check range from.</param>
	/// <param name="range">The range in which to send the message.</param>
	/// <param name="message">The message text to display.</param>
	/// <param name="icon">The icon type (scroll, Clear, !, etc.). Default is "scroll".</param>
	/// <param name="durationSeconds">How long the message stays on screen. Default is 5 seconds.</param>
	protected void SendNoticeInRange(Position position, float range, string message, string icon = "scroll", int durationSeconds = 5)
	{
		var characters = this.GetCharactersInRange(position, range);
		var functionName = $"NOTICE_Dm_{icon}";

		foreach (var character in characters)
		{
			character?.AddonMessage(functionName, message, durationSeconds);
		}
	}
}
