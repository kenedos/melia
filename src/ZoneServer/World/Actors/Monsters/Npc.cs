using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Geometry;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Monsters
{
	/// <summary>
	/// A non-player character that supports dialogues.
	/// </summary>
	public class Npc : MonsterInName, ICombatEntity, ITriggerableArea, IUpdateable
	{
		// TODO: Determine whether NPCs and mobs should actually be
		//   separate classes. NPCs don't typically fight, and many
		//   of them can't even move, but perhaps there are mobs
		//   with triggers? This needs some research.

		private Position _position;

		/// <summary>
		/// Returns the NPC's position on its current map.
		/// Setting this property automatically updates the spatial index.
		/// </summary>
		public override Position Position
		{
			get => _position;
			set
			{
				var oldPosition = _position;
				_position = value;
				this.Map?.UpdateEntitySpatialPosition(this, oldPosition, value);
			}
		}

		/// <summary>
		/// Returns the NPC's armor material type.
		/// </summary>
		public ArmorMaterialType ArmorMaterial => ArmorMaterialType.Cloth;

		/// <summary>
		/// Returns the NPC's movement type.
		/// </summary>
		public MoveType MoveType { get; set; } = MoveType.Normal;

		/// <summary>
		/// Returns the NPC's tendency.
		/// </summary>
		public TendencyType Tendency { get; set; } = TendencyType.Peaceful;

		private string _role;

		/// <summary>
		/// Sets the NPC's role identifier (e.g. "hunter", "villager", "ResourceNode").
		/// </summary>
		public void SetRole(string role) => _role = role;

		/// <summary>
		/// Gets the NPC's role identifier.
		/// </summary>
		public string GetRole() => _role ?? "";

		/// <summary>
		/// Gets or sets whether this NPC can move.
		/// </summary>
		/// <remarks>
		/// Most NPCs are stationary, but some (like interactive objects in minigames)
		/// may need to be able to move.
		/// </remarks>
		public bool AllowMovement { get; set; } = false;

		/// <summary>
		/// Returns the NPC's effective size.
		/// </summary>
		public SizeType EffectiveSize => SizeType.M;

		/// <summary>
		/// Returns the NPC's rank (always Normal for NPCs).
		/// </summary>
		public MonsterRank Rank => MonsterRank.Normal;

		/// <summary>
		/// Holds the order of successive changes in NPC's HP.
		/// </summary>
		public int HpChangeCounter { get; private set; }

		/// <summary>
		/// Returns true if the NPC is dead.
		/// </summary>
		public bool IsDead { get; private set; }

		/// <summary>
		/// Get or set the visual portrait for dialogs.
		/// </summary>
		public string Portrait { get; set; }

		/// <summary>
		/// Returns the function to call when the NPC is triggered via click.
		/// </summary>
		public DialogFunc DialogFunc { get; private set; }

		/// <summary>
		/// Returns the function called when someone steps into the NPC's
		/// trigger area.
		/// </summary>
		public TriggerActorFuncAsync EnterFunc { get; private set; }

		/// <summary>
		/// Returns the function called when someone stays inside the NPC's
		/// trigger area.
		/// </summary>
		public TriggerActorFuncAsync WhileInsideFunc { get; private set; }

		/// <summary>
		/// Returns the function called when someone steps out of the NPC's
		/// trigger area.
		/// </summary>
		public TriggerActorFuncAsync LeaveFunc { get; private set; }

		/// <summary>
		/// Returns the area in which the NPC's enter and leave functions
		/// are triggered.
		/// </summary>
		public IShapeF Area { get; private set; }

		/// <summary>
		/// Returns the NPC's variables.
		/// </summary>
		/// <remarks>
		/// NPC variables are temporary and are not saved across server
		/// restarts.
		/// </remarks>
		public Variables Vars { get; } = new Variables();

		public string AssociatedShopName { get; set; }
		public ShopType ShopType { get; set; }

		/// <summary>
		/// Creates new NPC.
		/// </summary>
		/// <param name="monsterClassId"></param>
		/// <param name="name"></param>
		/// <param name="location"></param>
		/// <param name="direction"></param>
		public Npc(int monsterClassId, string name, Location location, Direction direction, int genType = 0)
			: base(monsterClassId, genType)
		{
			this.Name = name;
			this.Position = location.Position;
			this.Direction = direction;
		}

		/// <summary>
		/// Creates new NPC.
		/// </summary>
		/// <param name="monsterClassId"></param>
		/// <param name="name"></param>
		/// <param name="location"></param>
		/// <param name="direction"></param>
		public Npc(int monsterClassId, string name, Position position, Direction direction, int genType = 0)
			: base(monsterClassId, genType)
		{
			this.Name = name;
			this.Position = position;
			this.Direction = direction;
		}

		/// <summary>
		/// Updates npc and its components.
		/// </summary>
		/// <param name="elapsed"></param>
		public virtual void Update(TimeSpan elapsed)
		{
			this.Components.Update(elapsed);
		}

		/// <summary>
		/// Changes the NPC's state and updates the nearby clients.
		/// </summary>
		/// <param name="state"></param>
		public void SetState(NpcState state)
		{
			this.State = state;
			Send.ZC_SET_NPC_STATE(this);
		}

		/// <summary>
		/// Sets up a function that is called when the NPC is triggered
		/// via click.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="func"></param>
		public void SetClickTrigger(string name, DialogFunc func)
		{
			this.DialogName = name;
			this.DialogFunc = func;
		}

		/// <summary>
		/// Sets up a function that is called when the NPC is triggered
		/// by stepping into the given area.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="func"></param>
		public void SetEnterTrigger(string name, TriggerActorFuncAsync func)
		{
			this.EnterName = name;
			this.EnterFunc = func;
		}

		/// <summary>
		/// Sets up a function that is called when the NPC is triggered
		/// by stepping into the given area.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="func"></param>
		public void SetEnterTrigger(string name, TriggerActorFuncSync func)
		{
			this.SetEnterTrigger(name, async (args) =>
			{
				func(args);
				await Task.CompletedTask;
			});
		}

		/// <summary>
		/// Sets up a function that is called when the NPC is triggered
		/// by stepping into the given area.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="func"></param>
		public void SetWhileInsideTrigger(string name, TriggerActorFuncAsync func)
		{
			this.WhileInsideName = name;
			this.WhileInsideFunc = func;
		}

		/// <summary>
		/// Sets up a function that is called when the NPC is triggered
		/// by stepping into the given area.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="func"></param>
		public void SetLeaveTrigger(string name, TriggerActorFuncAsync func)
		{
			this.LeaveName = name;
			this.LeaveFunc = func;
		}

		/// <summary>
		/// Sets the trigger area for the NPC's enter and leave triggers.
		/// </summary>
		/// <param name="area"></param>
		public void SetTriggerArea(IShapeF area, bool updateArea = false)
		{
			this.Area = area;
			//this.UpdateArea = updateArea;
		}

		/// <summary>
		/// Makes the NPC say the given message, which will display via
		/// chat bubble above their head.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Say(string format, params object[] args)
		{
			var message = args?.Length == 0 ? format : string.Format(format, args);
			Send.ZC_CHAT(this, message);
		}

		/// <summary>
		/// Makes NPC take damage. NPCs typically don't take damage, so this
		/// method does nothing and returns false.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="attacker"></param>
		/// <returns>Always returns false for NPCs.</returns>
		public bool TakeDamage(float damage, ICombatEntity attacker)
		{
			// NPCs don't take damage by default
			return false;
		}

		/// <summary>
		/// Returns true if this NPC can fight. NPCs typically cannot fight.
		/// </summary>
		/// <returns>Always returns false for NPCs.</returns>
		public bool CanFight()
		{
			return false;
		}

		/// <summary>
		/// Returns true if this NPC can attack the given entity. NPCs typically cannot attack.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns>Always returns false for NPCs.</returns>
		public bool CanAttack(ICombatEntity entity)
		{
			return false;
		}

		/// <summary>
		/// Returns true if this NPC can move.
		/// </summary>
		/// <returns>Returns true if AllowMovement is enabled, false otherwise.</returns>
		public bool CanMove()
		{
			return this.AllowMovement;
		}

		/// <summary>
		/// Returns true if this NPC can guard.
		/// </summary>
		/// <returns>Always returns false for NPCs.</returns>
		public bool CanGuard()
		{
			return false;
		}

		/// <summary>
		/// Returns true if this NPC can be staggered.
		/// </summary>
		/// <returns>Always returns false for NPCs.</returns>
		public bool CanStagger()
		{
			return false;
		}

		/// <summary>
		/// Returns true if this NPC can be knocked down.
		/// </summary>
		/// <returns>Always returns false for NPCs.</returns>
		public bool IsKnockdownable()
		{
			return false;
		}

		/// <summary>
		/// Heals the NPC. NPCs don't have HP management, so this does nothing.
		/// </summary>
		/// <param name="hpAmount"></param>
		/// <param name="spAmount"></param>
		public void Heal(float hpAmount, float spAmount)
		{
			// NPCs don't need healing
		}

		/// <summary>
		/// Kills the NPC and plays the death animation.
		/// </summary>
		/// <param name="killer"></param>
		public void Kill(ICombatEntity killer)
		{
			// Mark as dead
			this.IsDead = true;

			// Send death animation to clients
			Send.ZC_SKILL_CAST_CANCEL(this);
			Send.ZC_SKILL_DISABLE(this);
			Send.ZC_DEAD(this);

			this.Properties.SetFloat(PropertyName.HP, 0);
			this.Components.Get<MovementComponent>()?.Stop();
		}
	}

	/// <summary>
	/// An object that defines an area and functions to trigger when
	/// said area is entered or left.
	/// </summary>
	public interface ITriggerableArea : IActor
	{
		/// <summary>
		/// Returns a function to call when someone enters the area.
		/// </summary>
		TriggerActorFuncAsync EnterFunc { get; }

		/// <summary>
		/// Returns a function to call when someone stays inside the area.
		/// </summary>
		TriggerActorFuncAsync WhileInsideFunc { get; }

		/// <summary>
		/// Returns a function to call when someone leaves the area.
		/// </summary>
		TriggerActorFuncAsync LeaveFunc { get; }

		/// <summary>
		/// Area in which the enter and leave functions are triggered.
		/// </summary>
		IShapeF Area { get; }
	}

	/// <summary>
	/// A function that can be used as a synchronous trigger callback.
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public delegate void TriggerFuncSync(TriggerArgs args);

	/// <summary>
	/// A function that can be used as an asynchronous trigger callback.
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public delegate Task TriggerFuncAsync(TriggerArgs args);

	/// <summary>
	/// A function that can be used as a synchronous trigger callback.
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public delegate void TriggerActorFuncSync(TriggerActorArgs args);

	/// <summary>
	/// A function that can be used as an asynchronous trigger callback.
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public delegate Task TriggerActorFuncAsync(TriggerActorArgs args);

	/// <summary>
	/// The event arguments for pad trigger events.
	/// </summary>
	public class TriggerArgs : EventArgs
	{
		/// <summary>
		/// Returns how the trigger was triggered.
		/// </summary>
		public TriggerType Type { get; }

		/// <summary>
		/// Returns the trigger that was triggered.
		/// </summary>
		public IActor Trigger { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="trigger"></param>
		/// <param name="initiator"></param>
		public TriggerArgs(TriggerType type, IActor trigger)
		{
			this.Type = type;
			this.Trigger = trigger;
		}
	}

	/// <summary>
	/// The event arguments for pad trigger events.
	/// </summary>
	public class TriggerActorArgs : EventArgs
	{
		/// <summary>
		/// Returns how the trigger was triggered.
		/// </summary>
		public TriggerType Type { get; }

		/// <summary>
		/// Returns the trigger that was triggered.
		/// </summary>
		public IActor Trigger { get; }

		/// <summary>
		/// Returns the trigger as an Npc.
		/// </summary>
		/// <remarks>
		/// Added a quick hack to fix legacy trigger functions.
		/// Remove when those functions are converted to proper pads.
		/// </remarks>
		public Npc Npc => (Npc)this.Trigger;

		/// <summary>
		/// Returns the triggering actor that triggered the triggerarable trigger
		/// in a most triggerable way. Trigger.
		/// </summary>
		public IActor Initiator { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="trigger"></param>
		/// <param name="initiator"></param>
		public TriggerActorArgs(TriggerType type, IActor trigger, IActor initiator)
		{
			this.Type = type;
			this.Trigger = trigger;
			this.Initiator = initiator;
		}
	}

	/// <summary>
	/// The event arguments for pad trigger events.
	/// </summary>
	public class PadTriggerArgs : EventArgs
	{
		/// <summary>
		/// Returns how the trigger was triggered.
		/// </summary>
		public TriggerType Type { get; }

		/// <summary>
		/// Returns the pad that was triggered.
		/// </summary>
		public Pad Trigger { get; }

		/// <summary>
		/// Returns the actor that created the pad.
		/// </summary>
		public ICombatEntity Creator { get; }

		/// <summary>
		/// Returns the skill used in the creation of the pad.
		/// </summary>
		public Skill Skill { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="trigger"></param>
		/// <param name="creator"></param>
		/// <param name="skill"></param>
		public PadTriggerArgs(TriggerType type, Pad trigger, ICombatEntity creator, Skill skill)
		{
			this.Type = type;
			this.Trigger = trigger;
			this.Creator = creator;
			this.Skill = skill;
		}
	}

	/// <summary>
	/// The event arguments for pad trigger events.
	/// </summary>
	public class PadTriggerActorArgs : EventArgs
	{
		/// <summary>
		/// Returns how the trigger was triggered.
		/// </summary>
		public TriggerType Type { get; }

		/// <summary>
		/// Returns the pad that was triggered.
		/// </summary>
		public Pad Trigger { get; }

		/// <summary>
		/// Returns the triggering actor that triggered the triggerarable trigger
		/// in a most triggerable way. Trigger.
		/// </summary>
		public ICombatEntity Initiator { get; }

		/// <summary>
		/// Returns the actor that created the pad.
		/// </summary>
		public ICombatEntity Creator { get; }

		/// <summary>
		/// Returns the skill used in the creation of the pad.
		/// </summary>
		public Skill Skill { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="trigger"></param>
		/// <param name="initiator"></param>
		/// <param name="creator"></param>
		/// <param name="skill"></param>
		public PadTriggerActorArgs(TriggerType type, Pad trigger, ICombatEntity initiator, ICombatEntity creator, Skill skill)
		{
			this.Type = type;
			this.Trigger = trigger;
			this.Initiator = initiator;
			this.Creator = creator;
			this.Skill = skill;
		}
	}

	/// <summary>
	/// Defines how a trigger was triggered.
	/// </summary>
	public enum TriggerType
	{
		/// <summary>
		/// The trigger was created.
		/// </summary>
		Create,

		/// <summary>
		/// The trigger was destroyed.
		/// </summary>
		Destroy,

		/// <summary>
		/// An actor stepped into the trigger area.
		/// </summary>
		/// <remarks>
		/// Triggers only once per actor, when they initially enter the area.
		/// </remarks>
		Enter,

		/// <summary>
		/// An actor stepped out of the trigger area.
		/// </summary>
		/// <remarks>
		/// Triggers only once per actor, when they leave the area.
		/// </remarks>
		Leave,

		/// <summary>
		/// An update trigger that is raised in regular intervals while the
		/// trigger exists.
		/// </summary>
		/// <remarks>
		/// Triggers only once, regardless of the number of actors inside a
		/// trigger area. Use TriggerComponent.GetActors to retrieve a list
		/// of actors currently inside the area.
		/// </remarks>
		Update,
	}

	public static class NpcExtensions
	{
		/// <summary>
		/// Applies properties from XML propList to an NPC.
		/// </summary>
		public static void ApplyXmlProperties(this Npc npc, Dictionary<string, string> properties)
		{
			foreach (var prop in properties)
			{
				switch (prop.Key)
				{
					case "Name":
						npc.Name = prop.Value;
						break;
					case "Dialog":
						npc.DialogName = prop.Value;
						break;
					case "Enter":
						npc.EnterName = prop.Value;
						break;
					case "Scale":
						if (float.TryParse(prop.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var scale))
							npc.Properties.SetFloat(PropertyName.Scale, scale);
						break;
					case "Level":
						if (int.TryParse(prop.Value, out var level))
							npc.Level = level;
						break;
						// Add more property handlers as needed
				}
			}
		}
	}
}
