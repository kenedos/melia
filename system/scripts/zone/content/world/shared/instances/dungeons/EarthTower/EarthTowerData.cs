//--- Melia Script ----------------------------------------------------------
// Earth Tower Data
//--- Description -----------------------------------------------------------
// The mgame data model the Earth Tower floors are built with.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using Melia.Shared.World;

/// <summary>
/// A tool script call of an mgame, with its arguments as strings.
/// </summary>
public class MCall
{
	public string Name { get; }
	public string[] Args { get; }

	public MCall(string name, params string[] args)
	{
		this.Name = name;
		this.Args = args;
	}

	public string Str(int index) => index < this.Args.Length ? this.Args[index] : "";

	public int Int(int index) => index < this.Args.Length && float.TryParse(this.Args[index], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var value) ? (int)value : 0;
}

/// <summary>
/// An mgame: a set of stages that start, clear and destroy each other.
/// </summary>
public class MGameData
{
	public string Name { get; }
	public int Floor { get; }
	public List<MStageData> Stages { get; } = new();

	public MGameData(string name, int floor)
	{
		this.Name = name;
		this.Floor = floor;
	}

	public MStageData Stage(string name, bool autoStart)
	{
		var stage = new MStageData(name, autoStart);
		this.Stages.Add(stage);
		return stage;
	}
}

/// <summary>
/// An mgame stage: start calls, objects spawned on start and events.
/// </summary>
public class MStageData
{
	public string Name { get; }
	public bool AutoStart { get; }
	public List<MCall> StartCalls { get; } = new();
	public List<MObjData> Objects { get; } = new();
	public List<MEventData> Events { get; } = new();

	public MStageData(string name, bool autoStart)
	{
		this.Name = name;
		this.AutoStart = autoStart;
	}

	public void Start(string name, params string[] args) => this.StartCalls.Add(new MCall(name, args));

	public MObjData Obj(int key, int monsterId, float x, float y, float z, float angle)
	{
		var obj = new MObjData(key, monsterId, new Position(x, y, z), angle);
		this.Objects.Add(obj);
		return obj;
	}

	public MEventData Event(string name, int execCount, int intervalMs)
	{
		var ev = new MEventData(name, execCount, intervalMs);
		this.Events.Add(ev);
		return ev;
	}
}

/// <summary>
/// An object of a stage, spawned GenCount times within Range of its position.
/// </summary>
public class MObjData
{
	public int Key { get; }
	public int MonsterId { get; }
	public Position Position { get; }
	public float Angle { get; }
	public int GenCount { get; private set; } = 1;
	public int Range { get; private set; }
	public bool IsManual { get; private set; }
	public bool IsNeutral { get; private set; }
	public bool IsPassive { get; private set; }
	public bool IsHidden { get; private set; }
	public string Name { get; private set; }
	public string EnterFunc { get; private set; }
	public float EnterRange { get; private set; }
	public MCall[] DeadCalls { get; private set; } = new MCall[0];

	public MObjData(int key, int monsterId, Position position, float angle)
	{
		this.Key = key;
		this.MonsterId = monsterId;
		this.Position = position;
		this.Angle = angle;
	}

	public MObjData Gen(int count, int range) { this.GenCount = count; this.Range = range; return this; }
	public MObjData Manual() { this.IsManual = true; return this; }
	public MObjData Neutral() { this.IsNeutral = true; return this; }
	public MObjData Passive() { this.IsPassive = true; return this; }
	public MObjData Hidden() { this.IsHidden = true; return this; }
	public MObjData Named(string name) { this.Name = name; return this; }
	public MObjData Enter(string func, float range) { this.EnterFunc = func; this.EnterRange = range; return this; }
	public MObjData Dead(MCall[] calls) { this.DeadCalls = calls; return this; }
}

/// <summary>
/// A stage event: runs its calls once all conditions hold, up to ExecCount
/// times (0 for no limit).
/// </summary>
public class MEventData
{
	public string Name { get; }
	public int ExecCount { get; }
	public int IntervalMs { get; }
	public List<MCall> Conditions { get; } = new();
	public List<MCall> Calls { get; } = new();

	public MEventData(string name, int execCount, int intervalMs)
	{
		this.Name = name;
		this.ExecCount = execCount;
		this.IntervalMs = intervalMs;
	}

	public MEventData If(string name, params string[] args) { this.Conditions.Add(new MCall(name, args)); return this; }
	public MEventData Do(string name, params string[] args) { this.Calls.Add(new MCall(name, args)); return this; }
}
