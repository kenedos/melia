using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
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
		private readonly List<Mob> _associatedMonsters = new();

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
		/// Gets or sets the pad's first numeric argument, which is
		/// typically used for the pad's "angle".
		/// </summary>
		/// <remarks>
		/// It's currently unknown what exactly this property does. It can
		/// be found in pad-related packets and in the skill bytool data
		/// (element "Pos", argument "Angle"), but it doesn't appear to
		/// have any effect on the pad or its visuals.
		///
		/// Initially we called this property "Angle" due to the bytool
		/// data, but it's been reportedly found to contain different
		/// values in some cases, like Centurion skills.
		/// </remarks>
		public float NumArg1 { get; set; }

		/// <summary>
		/// Gets or sets the pad's second numeric argument, which is
		/// typically used as "distance".
		/// </summary>
		/// <remarks>
		/// It's currently unknown what exactly this property does. It can
		/// be found in pad-related packets and in the skill bytool data
		/// (element "Pos", argument "Dist"), but it doesn't appear to
		/// have any effect on the pad or its visuals.
		///
		/// Initially we called this property "Distance" due to the bytool
		/// data, but it's been reportedly found to contain different
		/// values in some cases, like Centurion skills.
		/// </remarks>
		public float NumArg2 { get; set; }

		/// <summary>
		/// Gets or sets the pad's third numeric argument.
		/// </summary>
		/// <remarks>
		/// It's currently unknown what exactly this property does. It can
		/// be found in pad-related packets, but it doesn't appear to have
		/// any effect on the pad or its visuals.
		/// </remarks>
		public float NumArg3 { get; set; }

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
		/// Returns if the pad has died.
		/// </summary>
		public bool IsDead => this.Trigger.MaxUseCount <= 0;

		/// <summary>
		/// Gets or sets an entity for the pad to follow. When set, the
		/// pad's position is updated to the target's position each tick,
		/// with a forward translation based on movement speed to
		/// compensate for fast-moving characters.
		/// </summary>
		public ICombatEntity FollowTarget { get; private set; }

		/// <summary>
		/// Returns the offset distance applied when following a target.
		/// </summary>
		public float FollowTargetOffset { get; private set; }

		/// <summary>
		/// Returns the offset angle in degrees, relative to the target's
		/// facing direction.
		/// </summary>
		public float FollowTargetOffsetAngle { get; private set; }

		/// <summary>
		/// Sets the pad to follow the given entity.
		/// </summary>
		/// <param name="target"></param>
		public void FollowsTarget(ICombatEntity target)
		{
			this.FollowTarget = target;
		}

		/// <summary>
		/// Sets the pad to follow the given entity with an offset.
		/// The direction is stored as a relative angle from the target's
		/// current facing, so it rotates with them as they turn.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="offset"></param>
		/// <param name="direction"></param>
		public void FollowsTarget(ICombatEntity target, float offset, Direction direction)
		{
			this.FollowTarget = target;
			this.FollowTargetOffset = offset;
			this.FollowTargetOffsetAngle = direction.DegreeAngle - target.Direction.DegreeAngle;
		}

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
		/// <remarks>
		/// We currently allow null names for pads, in which case no
		/// handler will be registered, but this might change in the
		/// future. If at all possible, the official name of the pad
		/// should be used.
		/// </remarks>
		/// <param name="name">The name of the pad, as defined in the client and the PadName enum.</param>
		/// <param name="creator">The actor that created the pad.</param>
		/// <param name="skill">The skill that created the pad.</param>
		/// <param name="triggerArea">The area that defines the pad's trigger zone.</param>
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

		/// <summary>
		/// Creates a new pad.
		/// </summary>
		/// <param name="name">The name of the pad, as defined in the client and the PadName enum.</param>
		/// <param name="creator">The actor that created the pad.</param>
		/// <param name="skill">The skill that created the pad.</param>
		/// <param name="triggerArea">The area that defines the pad's trigger zone.</param>
		/// <param name="options">Options that define additional properties for the pad.</param>
		/// <exception cref="ArgumentException">
		/// Thrown if a handler with the given name does not exist.
		/// </exception>
		public static Pad Create(string name, IActor creator, Skill skill, Position position, IShapeF triggerArea, PadOptions options)
		{
			var pad = new Pad(name, creator, skill, triggerArea);
			pad.Position = position;

			pad.NumArg1 = options.NumArg1;
			pad.NumArg2 = options.NumArg2;
			pad.NumArg3 = options.NumArg3;
			pad.Trigger.LifeTime = options.LifeTime;
			pad.Trigger.UpdateInterval = options.UpdateInterval;
			pad.Trigger.MaxActorCount = options.MaxActorCount;
			pad.Trigger.MaxUseCount = options.MaxUseCount;

			if (name != null)
				pad.RegisterHandler(name);

			pad.Trigger.Subscribe(TriggerType.Destroy, pad.OnDestroyed);

			return pad;
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
			var followTarget = this.FollowTarget;
			if (followTarget != null && !followTarget.IsDead)
			{
				var targetPos = followTarget.Position;

				if (followTarget is Character player && player.Movement.IsMoving)
				{
					var speed = (int)player.Properties.GetFloat(PropertyName.MSPD);
					var translationPerSpeed = speed / 4;
					targetPos = targetPos.GetRelative(followTarget.Direction, translationPerSpeed);
				}

				if (this.FollowTargetOffset != 0)
				{
					var offsetDir = this.FollowTargetOffsetAngle != 0
						? followTarget.Direction.AddDegreeAngle(this.FollowTargetOffsetAngle)
						: followTarget.Direction;
					targetPos = targetPos.GetRelative(offsetDir, this.FollowTargetOffset);
				}

				this.Position = targetPos;
			}

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

		/// <summary>
		/// Called when the pad is destroyed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDestroyed(object sender, TriggerArgs e)
		{
			foreach (var monster in _associatedMonsters)
				monster.Map?.RemoveMonster(monster);
		}

		/// <summary>
		/// Spawns a monster at the pad's position and associates it with
		/// the pad, meaning it will be automatically removed when the pad
		/// is destroyed.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="faction"></param>
		/// <returns></returns>
		public Mob SpawnMonster(int monsterId, FactionType faction)
		{
			var monster = this.CreateMonster(monsterId, faction);
			this.Map.AddMonster(monster);

			return monster;
		}

		/// <summary>
		/// Creates a monster at the pad's position without adding it to
		/// the map and associates it with the pad, meaning it will be
		/// automatically removed when the pad is destroyed.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="faction"></param>
		/// <returns></returns>
		public Mob CreateMonster(int monsterId, FactionType faction)
		{
			var monster = new Mob(monsterId);
			monster.Faction = faction;
			monster.Position = this.Position;
			monster.Direction = this.Direction;
			monster.Components.Add(new MovementComponent(monster));

			this.AssociateMonster(monster);

			return monster;
		}

		/// <summary>
		/// Associates the given monster with the pad, meaning it will be
		/// automatically removed when the pad is destroyed.
		/// </summary>
		/// <param name="monster"></param>
		public void AssociateMonster(Mob monster)
		{
			_associatedMonsters.Add(monster);
		}
	}
}
