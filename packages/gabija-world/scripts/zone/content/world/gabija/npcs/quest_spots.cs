//--- Melia Script ----------------------------------------------------------
// Quest Spots
//--- Description -----------------------------------------------------------
// Interactable points scattered over a map that each player uses up, the way
// the game removes a quest object after it was used.
//---------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using static Melia.Zone.Scripting.Shortcuts;

/// <summary>
/// Describes a set of interactable points a quest sends the player to.
/// </summary>
public class QuestSpotSpec
{
	public string Prefix;
	public int MonsterId;
	public string Name;
	public string Map;
	public (double X, double Z, double Direction)[] Points;

	/// <summary>
	/// The quest the points belong to. Their progress is only given up once
	/// the character no longer has it, so a quest that has been handed in is
	/// not reset by the next visibility check.
	/// </summary>
	public QuestId Quest = QuestId.Zero;

	public Func<Character, bool> IsActive;
	public Func<Character, int, bool> IsAvailable;
	public Func<Character, string> Requirement;
	public string TimedLabel;
	public string TimedAnim = "MAKING";
	public double Seconds = 2;
	public Action<Character, Npc> OnDone;
	public double AggroRadius;
	public string[] GuardClassNames;
	public string GuardMessage;
	public string GuardEffect;
	public float GuardEffectScale = 1;
	public string IdleMessage;
	public int RespawnSeconds;
}

/// <summary>
/// Places quest spots on a map.
/// </summary>
public static class QuestSpots
{
	private const string ConsumedVarPrefix = "Gabija.QuestSpots.";
	private static readonly ConcurrentDictionary<string, List<Mob>> Guards = new();
	private static readonly ConcurrentDictionary<string, Npc> Placed = new();

	/// <summary>
	/// Places every point of the spec as an NPC visible while the spec's
	/// quest is active and the point was not used by the player yet.
	/// </summary>
	/// <param name="spec"></param>
	public static void Add(QuestSpotSpec spec, Action<Npc> setup = null)
	{
		for (var i = 0; i < spec.Points.Length; i++)
		{
			var index = i;
			var point = spec.Points[i];

			var npc = AddConditionalNpc(spec.MonsterId, spec.Name, spec.Prefix + "_" + index, spec.Map, point.X, point.Z, point.Direction,
				character => IsVisible(spec, character, index),
				dialog => Interact(spec, dialog, index));

			Placed[spec.Prefix + "_" + index] = npc;
			setup?.Invoke(npc);
		}
	}

	/// <summary>
	/// Returns the name of the temporary variable holding how often the
	/// character used a point of the given spec.
	/// </summary>
	public static string CountVar(string prefix)
		=> ConsumedVarPrefix + prefix + ".Count";

	/// <summary>
	/// Returns the index of the closest point the character has not used up
	/// within the given radius, or -1.
	/// </summary>
	public static int FindNearest(QuestSpotSpec spec, Character character, double radius)
	{
		var consumed = GetConsumed(spec, character);
		var best = -1;
		var bestDistance = double.MaxValue;

		for (var i = 0; i < spec.Points.Length; i++)
		{
			if (IsConsumed(consumed, i))
				continue;

			var point = new Position((float)spec.Points[i].X, character.Position.Y, (float)spec.Points[i].Z);
			var distance = character.Position.Get2DDistance(point);

			if (distance <= radius && distance < bestDistance)
			{
				best = i;
				bestDistance = distance;
			}
		}

		return best;
	}

	private static Dictionary<int, DateTime> GetConsumed(QuestSpotSpec spec, Character character)
	{
		var name = ConsumedVarPrefix + spec.Prefix;

		if (!character.Variables.Temp.TryGet<Dictionary<int, DateTime>>(name, out var set))
		{
			set = new Dictionary<int, DateTime>();
			character.Variables.Temp.Set(name, set);
		}

		return set;
	}

	private static bool IsConsumed(Dictionary<int, DateTime> consumed, int index)
		=> consumed.TryGetValue(index, out var until) && until > DateTime.Now;

	/// <summary>
	/// Returns whether the character still holds the quest the points belong
	/// to. A spec without a quest keeps its progress for as long as it lives.
	/// </summary>
	/// <param name="spec"></param>
	/// <param name="character"></param>
	/// <returns></returns>
	private static bool IsQuestHeld(QuestSpotSpec spec, Character character)
		=> spec.Quest.Value == 0 || character.Quests.Has(spec.Quest);

	/// <summary>
	/// Gives back everything the character used up, for a quest they no longer
	/// have.
	/// </summary>
	/// <param name="spec"></param>
	/// <param name="character"></param>
	private static void ResetProgress(QuestSpotSpec spec, Character character)
	{
		GetConsumed(spec, character).Clear();
		character.Variables.Temp.SetInt(CountVar(spec.Prefix), 0);
	}

	private static bool IsVisible(QuestSpotSpec spec, Character character, int index)
	{
		if (!IsQuestHeld(spec, character))
		{
			ResetProgress(spec, character);
			return false;
		}

		if (!spec.IsActive(character))
			return false;

		if (spec.IsAvailable != null && !spec.IsAvailable(character, index))
			return false;

		return !IsConsumed(GetConsumed(spec, character), index);
	}

	private static async Task Interact(QuestSpotSpec spec, Dialog dialog, int index)
	{
		var character = dialog.Player;

		dialog.SetTitle(spec.Name);

		if (!spec.IsActive(character))
		{
			if (spec.IdleMessage != null)
				await dialog.Msg(spec.IdleMessage);
			return;
		}

		Placed.TryGetValue(spec.Prefix + "_" + index, out var npc);

		var missing = spec.Requirement?.Invoke(character);
		if (missing != null)
		{
			character.ServerMessage(missing);
			return;
		}

		if (spec.GuardClassNames != null && npc != null)
		{
			var key = spec.Prefix + "_" + character.ObjectId + "_" + index;

			if (Guards.TryGetValue(key, out var guards))
			{
				guards.RemoveAll(g => g.Map == null || g.IsDead);
				if (guards.Count > 0)
				{
					MarkGuards(spec, guards);
					if (spec.GuardMessage != null)
						character.ServerMessage(spec.GuardMessage);
					return;
				}
			}
			else
			{
				SpawnGuards(spec, character, npc, key);
				if (Guards.TryGetValue(key, out var spawned))
					MarkGuards(spec, spawned);
				if (spec.GuardMessage != null)
					character.ServerMessage(spec.GuardMessage);
				return;
			}
		}

		var result = await character.TimeActions.StartAsync(spec.TimedLabel, L("Cancel"), spec.TimedAnim, TimeSpan.FromSeconds(spec.Seconds));

		if (result != TimeActionResult.Completed)
			return;

		if (!spec.IsActive(character) || !IsVisible(spec, character, index))
			return;

		var consumed = GetConsumed(spec, character);
		consumed[index] = spec.RespawnSeconds > 0 ? DateTime.Now.AddSeconds(spec.RespawnSeconds) : DateTime.MaxValue;
		character.Variables.Temp.SetInt(CountVar(spec.Prefix), character.Variables.Temp.GetInt(CountVar(spec.Prefix), 0) + 1);

		if (spec.AggroRadius > 0 && npc != null)
		{
			foreach (var enemy in character.Map.GetAttackableEnemiesInPosition(character, npc.Position, (float)spec.AggroRadius))
				enemy.InsertHate(character, 1);
		}

		Guards.TryRemove(spec.Prefix + "_" + character.ObjectId + "_" + index, out _);
		spec.OnDone?.Invoke(character, npc);
		character.LookAround();

		if (spec.RespawnSeconds > 0)
			_ = RefreshLater(character, spec.RespawnSeconds);
	}

	private static async Task RefreshLater(Character character, int seconds)
	{
		await Task.Delay(TimeSpan.FromSeconds(seconds + 1));

		if (character.Map != null)
			character.LookAround();
	}

	/// <summary>
	/// Runs a temporary pad that reports every enemy inside it on each update.
	/// </summary>
	public static async Task RunChargePad(Character character, Position center, float range, int lifeMs, int updateMs, int lifeCostMs, Func<bool> isActive, Action<ICombatEntity> onHit)
	{
		var life = lifeMs;

		while (life > 0 && character.Map != null && isActive())
		{
			foreach (var enemy in character.Map.GetAttackableEnemiesInPosition(character, center, range))
			{
				if (enemy.IsDead)
					continue;

				onHit(enemy);
				life -= lifeCostMs;
			}

			life -= updateMs;
			await Task.Delay(updateMs);
		}
	}

	/// <summary>
	/// Spawns hostile monsters at a position that go after the character.
	/// </summary>
	public static void SpawnHostiles(Character character, Position center, string className, int count, TimeSpan life, int level = 0)
	{
		if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var data))
			return;

		for (var i = 0; i < count; i++)
		{
			var mob = new Mob(data.Id, RelationType.Enemy);
			mob.Position = center.GetRandomInRange2D(10, 30);
			mob.SpawnPosition = mob.Position;

			if (level > 0)
				mob.Level = level;

			mob.Components.Add(new LifeTimeComponent(mob, life));
			mob.Components.Add(new MovementComponent(mob));
			mob.Components.Add(new AiComponent(mob, "BasicMonster"));

			character.Map.AddMonster(mob);
			mob.InsertHate(character);
		}
	}

	/// <summary>
	/// Attaches the spec's guard effect to every guard.
	/// </summary>
	private static void MarkGuards(QuestSpotSpec spec, List<Mob> guards)
	{
		if (spec.GuardEffect == null)
			return;

		foreach (var guard in guards)
			guard.AttachEffect(spec.GuardEffect, spec.GuardEffectScale, EffectLocation.Top);
	}

	private static void SpawnGuards(QuestSpotSpec spec, Character character, Npc npc, string key)
	{
		var list = new List<Mob>();

		foreach (var className in spec.GuardClassNames)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var data))
				continue;

			var mob = new Mob(data.Id, RelationType.Enemy);
			mob.Position = npc.Position.GetRandomInRange2D(20, 40);
			mob.SpawnPosition = mob.Position;
			mob.Components.Add(new LifeTimeComponent(mob, TimeSpan.FromMinutes(5)));
			mob.Components.Add(new MovementComponent(mob));
			mob.Components.Add(new AiComponent(mob, "BasicMonster"));

			character.Map.AddMonster(mob);
			mob.InsertHate(character);
			list.Add(mob);
		}

		Guards[key] = list;
	}
}

/// <summary>
/// Attaches the effects the game's simple AI puts on a placed NPC.
/// </summary>
public static class NpcEffectExtensions
{
	/// <summary>
	/// Attaches a permanent effect to the NPC and returns the NPC.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="effectName"></param>
	/// <param name="scale"></param>
	/// <param name="location"></param>
	/// <returns></returns>
	public static Npc WithEffect(this Npc npc, string effectName, float scale, EffectLocation location = EffectLocation.Bottom)
	{
		npc?.AddEffect(new AttachEffect(effectName, scale, location));
		return npc;
	}
}
