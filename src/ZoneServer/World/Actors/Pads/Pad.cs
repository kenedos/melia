using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads.Components;
using Yggdrasil.Composition;
using Yggdrasil.Geometry;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Pads
{
	/// <summary>
	/// Represents a pad, which are zones on the map that trigger events.
	/// </summary>
	/// <remarks>
	/// The exact classification of pads is currently still in the air.
	/// We're considering them their own type of actor that are typically
	/// created by skills, though they might just as well be a type of
	/// monster.
	/// </remarks>
	public class Pad : Actor, IUpdateable
	{
		/// <summary>
		/// Gets or sets the pad's name.
		/// </summary>
		public override string Name { get; set; }

		/// <summary>
		/// Returns the actor that created the pad.
		/// </summary>
		public IActor Creator { get; }

		/// <summary>
		/// Returns the skill the pad was created by.
		/// </summary>
		public Skill Skill { get; }

		/// <summary>
		/// Returns the pad's area, defining the trigger zone.
		/// </summary>
		public IShapeF Area { get; set; }

		/// <summary>
		/// Returns the pad's movement component.
		/// </summary>
		public PadMovementComponent Movement { get; }

		/// <summary>
		/// Returns the pad's observer tracking component.
		/// </summary>
		public PadObserverComponent Observers { get; }

		/// <summary>
		/// Returns the pad's variables.
		/// </summary>
		/// <remarks>
		/// This is a collection of variables that can be used to store temporary
		/// information about the pad. They are not saved and will be lost after
		/// the pad was destroyed.
		/// </remarks>
		public Variables Variables { get; } = new();

		/// <summary>
		/// Returns the first argument the pad was started with.
		/// </summary>
		public float NumArg1 { get; set; }

		/// <summary>
		/// Returns the second argument the pad was started with.
		/// </summary>
		public float NumArg2 { get; set; }

		/// <summary>
		/// Returns the third argument the pad was started with.
		/// </summary>
		public float NumArg3 { get; set; }

		/// <summary>
		/// Returns if the pad has died.
		/// </summary>
		public bool IsDead => this.Trigger.MaxUseCount <= 0;

		public Mob? Monster
		{
			get
			{
				return this.Variables.Get<Mob>("Melia.Pad.Monster");
			}
			set
			{
				if (value == null)
					this.Variables.Remove("Melia.Pad.Monster");
				else
					this.Variables.Set("Melia.Pad.Monster", value);
			}
		}

		/// <summary>
		/// Creates a new pad.
		/// </summary>
		/// <param name="creator"></param>
		/// <param name="skill"></param>
		/// <param name="triggerArea"></param>
		public Pad(IActor creator, Skill skill, IShapeF triggerArea)
			: this(null, creator, skill, triggerArea)
		{
		}

		/// <summary>
		/// Creates a new pad.
		/// </summary>
		/// <param name="name">
		/// If not null, a pad handler with the given name will be looked up.
		/// And its methods will be registered as trigger events. The given
		/// handler must exist. Use null if no handler is needed.
		/// </param>
		/// <param name="creator"></param>
		/// <param name="skill"></param>
		/// <param name="triggerArea"></param>
		/// <exception cref="ArgumentException">
		/// Thrown if a handler with the given name does not exist.
		/// </exception>
		public Pad(string name, IActor creator, Skill skill, IShapeF triggerArea)
		{
			this.Name = name;
			this.Creator = creator;
			this.Skill = skill;
			this.Area = triggerArea;

			this.Position = creator.Position;
			this.Direction = creator.Direction;
			this.Layer = creator.Layer;

			this.Components.Add(this.Movement = new PadMovementComponent(this));
			this.Components.Add(this.Trigger = new TriggerComponent(this, triggerArea));
			this.Components.Add(this.Observers = new PadObserverComponent(this));

			if (name != null)
				this.RegisterHandler(name);
		}

		public Pad(ICombatEntity creator, Skill skill, string name, Position position, Direction direction, float range = 0)
		{
			this.Name = name;
			this.Creator = creator;
			this.Skill = skill;

			this.Position = position;
			this.Direction = direction;
			this.Layer = creator.Layer;

			if (range == 0)
				range = 10;
			this.Area = new Circle(this.Position, range);
			this.Components.Add(this.Trigger = new TriggerComponent(this, this.Area));
			this.Components.Add(this.Movement = new PadMovementComponent(this));
			this.Components.Add(this.Observers = new PadObserverComponent(this));

			if (name != null)
				this.RegisterHandler(name);
		}

		public Pad(ICombatEntity creator, Skill skill, string name, Position position, Direction direction, int bladeCount, int bladeLength, int bladeWidth)
		{
			this.Name = name;
			this.Creator = creator;
			this.Skill = skill;

			this.Position = position;
			this.Direction = direction;
			this.Layer = creator.Layer;

			this.Area = new BladedFan(this.Position, bladeCount, bladeLength, bladeWidth);
			this.Components.Add(this.Trigger = new TriggerComponent(this, this.Area));
			this.Components.Add(this.Movement = new PadMovementComponent(this));
			this.Components.Add(this.Observers = new PadObserverComponent(this));

			if (name != null)
				this.RegisterHandler(name);
		}

		/// <summary>
		/// Looks up the handler for the given name and registers its methods
		/// as trigger events.
		/// </summary>
		/// <param name="name"></param>
		private void RegisterHandler(string name)
		{
			if (!ZoneServer.Instance.PadHandlers.TryGetHandler(name, out var handler))
			{
				// Thanks exec, I'll just log this and continue with my life instead of crashing the server.
				//throw new ArgumentException($"No handler found for pad '{name}'.");
				Log.Debug($"No handler found for pad '{name}'.");
				return;
			}

			if (handler is ICreatePadHandler create) this.Trigger.Subscribe(TriggerType.Create, create.Created);
			if (handler is IDestroyPadHandler destroy) this.Trigger.Subscribe(TriggerType.Destroy, destroy.Destroyed);
			if (handler is IEnterPadHandler enter) this.Trigger.Subscribe(TriggerType.Enter, enter.Entered);
			if (handler is ILeavePadHandler leave) this.Trigger.Subscribe(TriggerType.Leave, leave.Left);
			if (handler is IUpdatePadHandler update) this.Trigger.Subscribe(TriggerType.Update, update.Updated);
		}

		/// <summary>
		/// Called in regular intervals to update the pad and potentially raise
		/// its events.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.Components.Update(elapsed);
		}

		/// <summary>
		/// Destroys the pad, triggering its Destroyed event and removing it from
		/// its current map.
		/// </summary>
		public void Destroy()
		{
			if (this.Monster != null)
			{
				this.Map?.RemoveMonster(this.Monster);
				this.Monster = null;
			}
			this.Map?.RemovePad(this);
		}

		/// <summary>
		/// Activates the pad, adding it to the map.
		/// </summary>
		public void Activate()
		{
			this.Creator.Map?.AddPad(this);
			if (this.Monster != null)
				this.Map?.AddMonster(this.Monster);
			this.Skill.Vars.SetInt($"Melia.{this.Skill.Id}.PadHandle", this.Handle);
		}

		public void SetBladedFanRange(int bladeCount, int bladeLength, int bladeWidth)
		{
			this.Area = new BladedFan(this.Position, bladeCount, bladeLength, bladeWidth);
			if (this.Trigger == null)
				this.Components.Add(this.Trigger = new TriggerComponent(this, this.Area));
			else
				this.Trigger.Area = this.Area;
		}

		public void SetRange(float range)
		{
			this.Area = new Circle(this.Position, range);
			if (this.Trigger == null)
				this.Components.Add(this.Trigger = new TriggerComponent(this, this.Area));
			else
				this.Trigger.Area = this.Area;
		}

		public void SetRectangleRange(Direction direction, float width, float distance)
		{
			this.Area = new Square(this.Position, direction, distance, width);
			if (this.Trigger == null)
				this.Components.Add(this.Trigger = new TriggerComponent(this, this.Area));
			else
				this.Trigger.Area = this.Area;
		}

		/// <summary>
		/// Sets update interval, in ms
		/// </summary>
		/// <param name="interval"></param>
		public void SetUpdateInterval(float interval)
		{
			this.Trigger.UpdateInterval = TimeSpan.FromMilliseconds(interval);
		}

		public void LinkEffect(string effectName, int monsterId, float size)
		{
			var startPos = this.Position.GetRelative(this.Direction, -size / 2);
			var endPos = this.Position.GetRelative(this.Direction, size / 2);
			Send.ZC_NORMAL.PadLinkEffect(this, startPos, endPos, effectName, monsterId);
		}

		public void ChangeGroundEffect(string effectName, float size)
		{
			Send.ZC_NORMAL.ChangeGroundEffect(this, effectName, size);
		}
	}
}
