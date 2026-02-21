using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Yggdrasil.Composition;
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Shared.Network.NormalOp;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.World.Actors
{
	/// <summary>
	/// Describes an entity that can actively participate in combat.
	/// </summary>
	public interface ICombatEntity : IActor, IPropertyHolder
	{
		/// <summary>
		/// Returns the entity's race.
		/// </summary>
		RaceType Race { get; }

		/// <summary>
		/// Returns the entity's element/attribute.
		/// </summary>
		AttributeType Attribute { get; }

		/// <summary>
		/// Returns the entity's armor material.
		/// </summary>
		ArmorMaterialType ArmorMaterial { get; }

		/// <summary>
		/// Returns the entity's mode of movement.
		/// </summary>
		MoveType MoveType { get; set; }

		/// <summary>
		/// Gets or sets the entity's tendency
		/// </summary>
		public TendencyType Tendency { get; set; }

		/// <summary>
		/// Returns the entity's effective size.
		/// </summary>
		/// <remarks>
		/// The effective size is not necessarily the same as the entity's set
		/// size, as some are classified as a certain size for some purposes,
		/// but another size for others. For example, players have their own
		/// "size" property called "PC", but for bonus purposes they are
		/// considered "M" size.
		/// </remarks>
		SizeType EffectiveSize { get; }

		/// <summary>
		/// Returns the entity's monster rank. Returns Normal if entity is
		/// not a mob.
		/// </summary>
		MonsterRank Rank { get; }

		/// <summary>
		/// Returns the entity's radius for pathfinding purposes.
		/// </summary>
		/// <remarks>
		/// Based on shape.ies.
		/// </remarks>
		float AgentRadius => this.EffectiveSize switch
		{
			SizeType.S => 12,
			SizeType.PC => 5,
			SizeType.M => 15,
			SizeType.L => 20,
			SizeType.XL => 40,
			SizeType.XXL => 40,
			_ => 0,
		};

		/// <summary>
		/// Returns the entity's level.
		/// </summary>
		int Level { get; }

		/// <summary>
		/// Returns the entity's current HP.
		/// </summary>
		int Hp { get; }

		/// <summary>
		/// Returns the entity's current HP.
		/// </summary>
		int MaxHp { get; }

		/// <summary>
		/// Holds the order of successive changes in entity's HP.
		/// A higher value indicates the latest HP amount.
		/// </summary>
		int HpChangeCounter { get; }

		/// <summary>
		/// Returns true if the entity is dead.
		/// </summary>
		bool IsDead { get; }

		/// <summary>
		/// Makes entity take damage and kills it if its HP reach 0.
		/// Returns whether the entity is dead.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="attacker"></param>
		/// <returns>Returns true if the entity died from the attack.</returns>
		bool TakeDamage(float damage, ICombatEntity attacker);

		/// <summary>
		/// Returns true if this entity is able to attack others.
		/// </summary>
		/// <returns></returns>
		bool CanFight();

		/// <summary>
		/// Returns true if this entity can attack the given one.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool CanAttack(ICombatEntity entity);

		/// <summary>
		/// Returns true if this entity is able to move.
		/// </summary>
		/// <returns></returns>
		bool CanMove();

		/// <summary>
		/// Returns true if the entity is able to guard.
		/// </summary>
		/// <returns></returns>
		bool CanGuard();

		/// <summary>
		/// Returns true if the entity can be staggered.
		/// </summary>
		/// <returns></returns>
		bool CanStagger();

		/// <summary>
		/// Returns true if the entity is able to be knocked down.
		/// </summary>
		/// <returns></returns>
		bool IsKnockdownable();

		/// <summary>
		/// Heals the entity's HP and SP by the given amounts.
		/// </summary>
		/// <param name="hpAmount"></param>
		/// <param name="spAmount"></param>
		void Heal(float hpAmount, float spAmount);

		/// <summary>
		/// Kills an entity
		/// </summary>
		/// <param name="killer"></param>
		void Kill(ICombatEntity killer);
	}

	public static class IActorExtensions
	{
		/// <summary>
		/// Gets the 3d distance between two actors.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="otherActor"></param>
		/// <returns></returns>
		public static double GetDistance(this IActor actor, IActor otherActor)
		{
			return actor.Position.Get3DDistance(otherActor.Position);
		}

		/// <summary>
		/// Sends an addon message to visible players
		/// </summary>
		/// <param name="function"></param>
		/// <param name="stringParameter"></param>
		/// <param name="intParameter"></param>
		public static void AddonMessage(this IActor actor, string function, string stringParameter = null, int intParameter = 0)
		{
			Send.ZC_ADDON_MSG(actor, function, intParameter, stringParameter);
		}

		/// <summary>
		/// Adds a visual effect to the actor.
		/// </summary>
		public static void AddEffect(this IActor actor, Effect effect)
		{
			if (actor.Components.TryGet<EffectsComponent>(out var effects))
				effects.AddEffect(effect);
		}

		/// <summary>
		/// Adds a named visual effect to the actor, replacing any existing
		/// effect with the same name.
		/// </summary>
		public static void AddEffect(this IActor actor, string name, Effect effect)
		{
			if (actor.Components.TryGet<EffectsComponent>(out var effects))
			{
				effects.RemoveEffect(name);
				effects.AddEffect(name, effect);
			}
		}

		/// <summary>
		/// Removes a named visual effect from the actor.
		/// </summary>
		public static void RemoveEffect(this IActor actor, string name)
		{
			if (actor.Components.TryGet<EffectsComponent>(out var effects))
				effects.RemoveEffect(name);
		}

		/// <summary>
		/// Attaches a visual effect to the actor for a specific connection.
		/// </summary>
		public static void AttachEffect(this IActor actor, IZoneConnection conn, string packetString, float scale, EffectLocation heightOffset)
		{
			Send.ZC_NORMAL.AttachEffect(conn, actor, packetString, scale, heightOffset);
		}

		/// <summary>
		/// Attaches a visual effect to the actor, broadcast to all nearby players.
		/// </summary>
		public static void AttachEffect(this IActor actor, string packetString, EffectLocation heightOffset, byte b1 = 0)
		{
			Send.ZC_NORMAL.AttachEffect(actor, packetString, 1, heightOffset, b1);
		}

		/// <summary>
		/// Attaches a scaled visual effect to the actor, broadcast to all nearby players.
		/// </summary>
		public static void AttachEffect(this IActor actor, string packetString, float scale, EffectLocation heightOffset)
		{
			Send.ZC_NORMAL.AttachEffect(actor, packetString, scale, heightOffset);
		}

		/// <summary>
		/// To attach this actor to another entity.
		/// Handles both the visual attachment (via packet) and the server-side AI state.
		/// </summary>
		/// <param name="actor">The actor that is being attached (self).</param>
		/// <param name="attachToActor">The host/target actor to attach to.</param>
		/// <param name="nodeName">The bone or attachment point name on the 'actor'.</param>
		/// <param name="targetNodeName">The bone or attachment point name on the 'attachToActor'.</param>
		/// <param name="attachSec">Time in seconds it takes to complete the attachment move.</param>
		/// <param name="randomAttachRange">Random variance applied to the attachment distance.</param>
		/// <param name="holdAi">If true, suspends the AI processing of the target actor.</param>
		/// <param name="distance">The fixed offset distance from the target node.</param>
		/// <param name="attachAnim">The animation to play during the attachment process.</param>
		public static void AttachToObject(
			this IActor actor,
			IActor attachToActor,
			string nodeName,
			string targetNodeName,
			float attachSec = 1,
			float randomAttachRange = 0,
			bool holdAi = false,
			float distance = 0,
			string attachAnim = "None")
		{
			// Server-side Logic: Suspend the script processing of the target if needed.
			// Note: If holdAi is intended to stop the 'actor' instead of the 'target', 
			// change attachToActor to actor here.
			if (attachToActor != null && attachToActor.Components.TryGet<AiComponent>(out var aiComponent))
			{
				aiComponent.Script.Suspended = holdAi;
			}

			// Network Layer: Send the synchronization packet to clients.
			// We map holdAi to 1.0f or 0.0f to match the float parameter in the C++ source.
			Send.ZC_ATTACH_TO_OBJ(
				actor,
				attachToActor,
				nodeName,
				targetNodeName,
				attachSec,
				randomAttachRange,
				holdAi ? 1.0f : 0.0f,
				distance: distance,
				attachAnimation: attachAnim
			);
		}

		/// <summary>
		/// Detaches a visual effect from the actor for a specific connection.
		/// </summary>
		public static void DetachEffect(this IActor actor, IZoneConnection conn, string packetString)
		{
			Send.ZC_NORMAL.DetachEffect(conn, actor, packetString);
		}

		/// <summary>
		/// Detaches a visual effect from the actor, broadcast to all nearby players.
		/// </summary>
		public static void DetachEffect(this IActor actor, string packetString)
		{
			Send.ZC_NORMAL.DetachEffect(actor, packetString);
		}

		/// <summary>
		/// Delays the actor's world entry by the specified number of seconds.
		/// </summary>
		public static void DelayEnterWorld(this IActor actor, float delay = 0)
		{
			Send.ZC_NORMAL.DelayEnterWorld(actor, delay);
		}

		/// <summary>
		/// Enter the actor while giving a delay time
		/// </summary>
		/// <param name="actor"></param>
		public static void EnterDelayedActor(this IActor actor)
		{
			Send.ZC_NORMAL.EnterDelayedActor(actor);
		}

		/// <summary>
		/// Change an actor's scale
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="scale"></param>
		/// <param name="duration"></param>
		public static void ChangeScale(this IActor actor, float scale, float duration)
		{
			if (actor is IPropertyHolder holder)
				holder.Properties.SetFloat(PropertyName.Scale, scale);
			Send.ZC_NORMAL.SetScale(actor, 33, scale, duration, 1, 0, 0);
		}

		/// <summary>
		/// Broadcasts a camera shockwave effect to nearby players.
		/// </summary>
		public static void BroadcastShockWave(this IActor actor, int zoomIndex, float range,
			float shakePower, float duration, float shakeAmount, float shakeDirection = 0, float delay = 0)
			=> Send.ZC_CHANGE_CAMERA_ZOOM(actor, zoomIndex, range, shakePower, duration, shakeAmount, shakeDirection, delay);

		/// <summary>
		/// Moves the actor to a position with a jump animation.
		/// </summary>
		public static void JumpToPosition(this IActor actor, float x, float y, float z, float moveTime, float jumpPower)
		{
			actor.Position = new Position(x, y, z);
			Send.ZC_NORMAL.JumpToPosition(actor, 0.3f, 200);
		}

		/// <summary>
		/// Sets the actor's vertical offset from the ground.
		/// </summary>
		public static void SetHeight(this IActor actor, float yOffset)
		{
			Send.ZC_NORMAL.SetHeight(actor, yOffset);
		}

		/// <summary>
		/// Sets the actor's standing (idle) animation.
		/// </summary>
		public static void SetStandAnimation(this IActor actor, FixedAnimation animation)
		{
			Send.ZC_STD_ANIM(actor, animation);
		}

		/// <summary>
		/// Sets the actor's movement animation.
		/// </summary>
		public static void SetMoveAnimation(this IActor actor, FixedAnimation animation, byte b1 = 0)
		{
			Send.ZC_MOVE_ANIM(actor, animation, b1);
		}

		/// <summary>
		/// Displays an emoticon above the actor for the given duration.
		/// </summary>
		public static void ShowEmoticon(this IActor actor, string emoticonName, TimeSpan duration)
		{
			Send.ZC_SHOW_EMOTICON(actor, emoticonName, duration);
		}

		/// <summary>
		/// Spawns a falling projectile effect at the target position.
		/// </summary>
		public static void MissileFall(this IActor actor, string skillClassName,
			string fallingEffectName, float fallingEffectScale, Position fallPos,
			float range, float delayTime, float flyTime, float height, float easing,
			string endEftName, float endScale, float startEasing, string groundEffectName,
			float groundEffectScale)
		{
			Send.ZC_NORMAL.SkillFallingProjectile(actor, skillClassName, fallingEffectName,
				fallingEffectScale, groundEffectName, groundEffectScale, fallPos, range, TimeSpan.FromSeconds(delayTime), flyTime, height, easing, startEasing);
		}

		/// <summary>
		/// Moves the actor to the last walkable position along a path
		/// between two points, with a delay before starting.
		/// </summary>
		public static async Task MoveToLastValidPosition(this IActor actor, Position startPos, Position endPos, float startTime, float endTime)
		{
			if (!actor.Components.TryGet<MovementComponent>(out var movementComponent))
				return;
			var lastEndPos = actor.Map.Ground.GetLastValidPosition(startPos, endPos);
			await movementComponent.MoveToWithDelay(startPos, lastEndPos, TimeSpan.FromMilliseconds(startTime), TimeSpan.FromMilliseconds(endTime - startTime));
		}

		/// <summary>
		/// Generates a new unique sync key for skill synchronization.
		/// </summary>
		public static int GenerateSyncKey(this IActor actor)
		{
			return ZoneServer.Instance.World.CreateSkillHandle();
		}

		/// <summary>
		/// Plays a chain lightning-style effect between multiple actors.
		/// </summary>
		public static void PlayChainEffect(this IActor actor, string effectName, float effectScale, float chainDuration, params (int, int)[] hitKeyActors)
		{
			Send.ZC_NORMAL.ShootChainEffect(actor, effectName, effectScale, chainDuration, hitKeyActors);
		}

		/// <summary>
		/// Plays effect for the actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetString"></param>
		/// <param name="heightOffset"></param>
		/// <param name="b2"></param>
		/// <param name="associatedHandle"></param>
		public static void PlayEffect(this IActor actor, string packetString, EffectLocation heightOffset = EffectLocation.Bottom, float b2 = 0, int associatedHandle = 0)
		{
			Send.ZC_NORMAL.PlayEffect(actor, 1, heightOffset, (byte)b2, 1, packetString, 0, associatedHandle);
		}

		/// <summary>
		/// Plays effect for the actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetString"></param>
		/// <param name="scale"></param>
		/// <param name="b1"></param>
		/// <param name="heightOffset"></param>
		/// <param name="b2"></param>
		/// <param name="associatedHandle"></param>
		public static void PlayEffect(this IActor actor, string packetString, float scale, byte b1 = 1, EffectLocation heightOffset = EffectLocation.Bottom, byte b2 = 0, int associatedHandle = 0)
		{
			Send.ZC_NORMAL.PlayEffect(actor, b1, heightOffset, b2, scale, packetString, 0, associatedHandle);
		}

		/// <summary>
		/// Plays effect for the actor on a specific connection.
		/// </summary>
		/// <param name="packetString"></param>
		public static void PlayEffectLocal(this IActor actor, IZoneConnection conn, string packetString, float scale = 1, EffectLocation heightOffset = EffectLocation.Bottom, byte b1 = 0)
		{
			Send.ZC_NORMAL.PlayEffect(conn, actor, b1, heightOffset, 1, scale, packetString, 0, 0);
		}


		/// <summary>
		/// Plays an effect at a given node.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="effectName"></param>
		/// <param name="duration"></param>
		/// <param name="str1"></param>
		/// <param name="str2"></param>
		public static void PlayEffectNode(this IActor actor, string effectName, float duration, string str1 = "None", string str2 = "None")
		{
			Send.ZC_NORMAL.PlayEffectNode(actor, effectName, duration, str1, str2);
		}

		/// <summary>
		/// Plays an effect with a given position, and returns the effect's handle.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="effectName"></param>
		/// <param name="position"></param>
		/// <param name="scale"></param>
		/// <param name="duration">in millisseconds</param>
		/// <returns></returns>
		public static async Task PlayEffectToGround(this IActor actor, string effectName, Position position, float scale = 1f, float duration = 0f, float delay = 0, float unk1 = 0)
		{
			await Task.Delay(TimeSpan.FromMilliseconds(delay));
			var effectHandle = ZoneServer.Instance.World.CreateEffectHandle();
			Send.ZC_NORMAL.PlayEffectAtPosition(actor, effectName, position, scale, effectHandle, duration);
		}

		/// <summary>
		/// Plays a ground effect at the actor's position.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="effectName"></param>
		/// <param name="scale"></param>
		/// <param name="duration"></param>
		/// <param name="delay"></param>
		/// <param name="angle"></param>
		public static void PlayGroundEffect(this IActor actor, string effectName, float scale = 1f, float duration = 0f, float delay = 0, float angle = 0)
		{
			Send.ZC_GROUND_EFFECT(actor, actor.Position, effectName, scale, duration, delay, angle);
		}

		/// <summary>
		/// Plays an effect on the ground.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="position"></param>
		/// <param name="effectName"></param>
		/// <param name="scale"></param>
		/// <param name="duration"></param>
		/// <param name="delay"></param>
		/// <param name="angle"></param>
		public static void PlayGroundEffect(this IActor actor, Position position, string effectName, float scale = 1f, float duration = 0f, float delay = 0, float angle = 0)
		{
			Send.ZC_GROUND_EFFECT(actor, position, effectName, scale, duration, delay, angle);
		}

		/// <summary>
		/// Plays a named animation on the actor, optionally with a delay.
		/// </summary>
		public static async void PlayAnimation(this IActor actor, string animationName, bool stopOnLastFrame = false, int delay = 0, byte b1 = 0)
		{
			await Task.Delay(delay);
			Send.ZC_PLAY_ANI(actor, animationName, stopOnLastFrame, b1);
		}

		/// <summary>
		/// Plays a sound effect at the actor's location.
		/// </summary>
		public static void PlaySound(this IActor actor, string animationName, bool loop = false)
			=> Send.ZC_PLAY_SOUND(actor, animationName, loop);

		/// <summary>
		/// Plays a gender-appropriate sound effect at the actor's location.
		/// </summary>
		public static void PlaySound(this IActor actor, string femaleAnimationName, string maleAnimationName, bool loop = false)
		{
			var gender = (actor as Character)?.Gender ?? Gender.Female;
			var animationName = gender == Gender.Male ? maleAnimationName : femaleAnimationName;

			PlaySound(actor, animationName, loop);
		}

		/// <summary>
		/// Stops a gender-appropriate sound effect on the actor.
		/// </summary>
		public static void StopSound(this IActor actor, string femaleAnimationName, string maleAnimationName = "")
		{
			var gender = (actor as Character)?.Gender ?? Gender.Female;
			var animationName = gender == Gender.Male ? maleAnimationName : femaleAnimationName;

			Send.ZC_STOP_SOUND(actor, animationName);
		}

		/// <summary>
		/// Stops the currently playing animation on the actor.
		/// </summary>
		public static void StopAnimation(this IActor actor)
			=> Send.ZC_NORMAL.StopAnimation(actor);

		/// <summary>
		/// Displays a floating text effect on the actor.
		/// </summary>
		public static void PlayTextEffect(this IActor actor, string v, string v1)
		{
			Send.ZC_NORMAL.PlayTextEffect(actor, actor, v, 0, v1);
		}

		/// <summary>
		/// Sets the position of the entity and updates client.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="position"></param>
		public static void SetPosition(this IActor actor, Position position)
		{
			actor.Position = position;
			Send.ZC_SET_POS(actor);
		}

		/// <summary>
		/// Shows an effect on the given entity
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="conn"></param>
		public static void ShowEffects(this IActor actor, IZoneConnection conn)
		{
			if (actor.Components.TryGet<EffectsComponent>(out var effectsComponent))
				effectsComponent.ShowEffects(conn);
		}

		/// <summary>
		/// Makes the actor fly upward to the specified height.
		/// </summary>
		public static void Fly(this IActor actor, float flyHeight, float raiseSpeed = 5)
		{
			Send.ZC_FLY(actor, flyHeight, 5);
		}

		/// <summary>
		/// Suspends the target in air
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="maxHeight"></param>
		/// <param name="duration"></param>
		/// <param name="easing"></param>
		/// <param name="playAfterBorn"></param>
		public static void FlyMath(this IActor actor, float maxHeight, float duration, float easing, bool playAfterBorn = true)
		{
			Send.ZC_FLY_MATH(actor, maxHeight, duration, easing, playAfterBorn);
		}

		/// <summary>
		/// Visually makes the actor shake in place
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="duration"></param>
		/// <param name="vibrationStrength"></param>
		/// <param name="frequency"></param>
		/// <param name="delay"></param>
		public static void Vibrate(this IActor actor, float duration, float vibrationStrength, float frequency, float delay)
		{
			Send.ZC_VIBRATE(actor, duration, vibrationStrength, frequency, delay);
		}

		/// <summary>
		/// Returns the actor's region faction identifier (e.g. Klaipeda, Orsha, Fedimian).
		/// Derives from the actor's stored variable or the nearest city.
		/// </summary>
		public static string GetRegionFaction(this IActor actor)
		{
			if (actor is Npc npc)
			{
				if (npc.Vars.TryGetString("Laima.Faction", out var npcFaction) && !string.IsNullOrEmpty(npcFaction))
				{
					return npcFaction;
				}

				if (npc?.Map?.Data != null && !string.IsNullOrEmpty(npc.Map.Data.NearbyCity)
					&& ZoneServer.Instance.Data.MapDb.TryFind(npc.Map.Data.NearbyCity, out var cityMap))
				{
					var city = cityMap.Name;
					if (city.Equals(FactionId.Klaipeda, StringComparison.OrdinalIgnoreCase)) return FactionId.Klaipeda;
					if (city.Equals(FactionId.Orsha, StringComparison.OrdinalIgnoreCase)) return FactionId.Orsha;
					if (city.Equals(FactionId.Fedimian, StringComparison.OrdinalIgnoreCase)) return FactionId.Fedimian;
				}
			}
			else if (actor is Character character)
			{
				if (!character.Variables.Perm.TryGetString("Laima.Faction", out var faction) || string.IsNullOrEmpty(faction))
				{
					var returnLocation = character.GetCityReturnLocation();
					if (!ZoneServer.Instance.Data.MapDb.TryFind(returnLocation.MapId, out var cityMap))
						faction = FactionId.Klaipeda;
					else
					{
						var city = cityMap.Name;
						if (city.Equals(FactionId.Klaipeda, StringComparison.OrdinalIgnoreCase)) faction = FactionId.Klaipeda;
						if (city.Equals(FactionId.Orsha, StringComparison.OrdinalIgnoreCase)) faction = FactionId.Orsha;
						if (city.Equals(FactionId.Fedimian, StringComparison.OrdinalIgnoreCase)) faction = FactionId.Fedimian;
					}
					SetRegionFaction(character, faction);
				}
				return faction;
			}
			return null;
		}

		/// <summary>
		/// Sets the actor's region faction identifier.
		/// </summary>
		public static void SetRegionFaction(this IActor actor, string regionFactionId)
		{
			if (actor is Npc npc)
			{
				npc.Vars.SetString("Laima.Faction", regionFactionId);
			}
			else if (actor is Mob mob)
			{
				mob.Vars.SetString("Laima.Faction", regionFactionId);
			}
			else if (actor is Character character)
			{
				character.Variables.Perm.SetString("Laima.Faction", regionFactionId);
			}
		}
	}

	/// <summary>
	/// Extensions for working with combat entities.
	/// </summary>
	public static class ICombatEntityExtensions
	{

		/// <summary>
		/// Returns a random skill the entity can use
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static bool TryGetRandomSkill(this ICombatEntity caster, out Skill skill)
		{
			skill = null;

			if (caster is not Mob mob)
				return false;

			if (mob.Data.Skills.Count == 0)
				return false;

			var rndSkillId = mob.Data.Skills.Where(a => !caster.IsOnCooldown(a.SkillId)).Select(a => a.SkillId).Random();

			if (caster.Components.TryGet<BaseSkillComponent>(out var skills))
				if (!skills.Has(rndSkillId))
				{
					skill = new Skill(caster, rndSkillId, 1);
					skills.AddSilent(skill);
				}
				else
					skills.TryGet(rndSkillId, out skill);

			return true;
		}

		/// <summary>
		/// Cancels the monster's current skill via AI alert and disables
		/// further skill use temporarily.
		/// </summary>
		public static void CancelMonsterSkill(this ICombatEntity entity)
		{
			if (entity.Components.TryGet<AiComponent>(out var ai))
				ai.Script.QueueEventAlert(new CancelSkillAlert());
			Send.ZC_SKILL_DISABLE(entity);
		}

		/// <summary>
		/// Returns the skill ID of the entity's currently active skill.
		/// </summary>
		public static SkillId GetCurrentSkill(this ICombatEntity caster)
		{
			if (caster is Character character)
				return character.Skills.CurrentSkill;
			else if (caster.Components.TryGet<BaseSkillComponent>(out var component))
				return component.CurrentSkill;

			return SkillId.None;
		}

		/// <summary>
		/// Attaches an actor to another actor at a bone/node attachment point.
		/// </summary>
		public static void AttachToItem(this IActor actor, IActor attachToActor, string nodeName, string targetNodeName, float attachSec = 1, float randomAttachRange = 0, bool holdAi = false, float f1 = 0, string attachAnim = "None")
		{
			if (attachToActor != null && attachToActor.Components.TryGet<AiComponent>(out var aiComponent))
				aiComponent.Script.Suspended = holdAi;
			Send.ZC_ATTACH_TO_OBJ(actor, attachToActor, nodeName, targetNodeName, attachSec, randomAttachRange, f1, attachAnimation: attachAnim);
		}

		/// <summary>
		/// Adds one or more entities to the caster's target list.
		/// </summary>
		public static void AddTarget(this ICombatEntity caster, params ICombatEntity[] targets)
		{
			if (!caster.Components.TryGet<CombatComponent>(out var combat))
				return;
			foreach (var target in targets)
			{
				combat.AddTarget(target);
			}
		}

		/// <summary>
		/// Clears all entities from the caster's target list.
		/// </summary>
		public static void ClearTargets(this ICombatEntity caster)
		{
			caster.Components.Get<CombatComponent>()?.ClearTargets();
		}

		/// <summary>
		/// Returns all entities in the caster's target list.
		/// </summary>
		public static ICombatEntity[] GetTargets(this ICombatEntity caster)
		{
			return caster.Components.Get<CombatComponent>()?.GetTargets() ?? [];
		}

		/// <summary>
		/// Returns a random entity from the caster's target list.
		/// </summary>
		public static ICombatEntity GetRandomTarget(this ICombatEntity caster)
		{
			return caster.Components.Get<CombatComponent>()?.GetRandomTarget() ?? null;
		}

		/// <summary>
		/// Replaces the caster's target list with the given targets.
		/// </summary>
		public static void SetTargets(this ICombatEntity caster, IList<ICombatEntity> targets)
		{
			caster.ClearTargets();
			foreach (var target in targets)
				caster.AddTarget(target);
		}

		/// <summary>
		/// Replaces the caster's target list with the given targets.
		/// </summary>
		public static void SetTarget(this ICombatEntity caster, params ICombatEntity[] targets)
		{
			caster.ClearTargets();
			caster.AddTarget(targets);
		}

		/// <summary>
		/// Adds hate towards a target entity, causing the AI to aggro it.
		/// </summary>
		public static void InsertHate(this ICombatEntity entity, ICombatEntity targetToHate, int hateToAdd = 999)
		{
			entity.Components.Get<AiComponent>()?.Script.QueueEventAlert(new HateIncreaseAlert(targetToHate, hateToAdd));
		}

		/// <summary>
		/// Interrupts the entity's current skill cast.
		/// </summary>
		public static void Interrupt(this ICombatEntity entity)
		{
			Send.ZC_SKILL_CAST_CANCEL(entity);
		}

		/// <summary>
		/// Queues a movement command to the entity's AI to move to the
		/// given position.
		/// </summary>
		public static void MoveTo(this ICombatEntity entity, Position position, float speed, int moveTime = 0, bool ignoreHoldMove = false, bool suspendAI = false)
		{
			entity.Components.Get<AiComponent>()?.Script.QueueEventAlert(new MoveToAlert(position, speed, moveTime, ignoreHoldMove, suspendAI));
		}

		/// <summary>
		/// Immediately teleports the entity to a position and sends
		/// movement packets to the client.
		/// </summary>
		public static void ForceMoveTo(this ICombatEntity entity, Position position, float speed, float moveTime = 0, bool ignoreHoldMove = false, bool suspendAI = false)
		{
			var currentPos = entity.Position;
			entity.Position = position;
			Send.ZC_MOVE_POS(entity, currentPos, position, speed, moveTime);
		}

		/// <summary>
		/// Sets the client-side death script for the actor.
		/// </summary>
		public static void SetClientDeadScript(this IActor actor, string scriptName, string finEft, float finEftScl)
		{
			Send.ZC_NORMAL.Skill_CallLuaFunc(actor, scriptName, 2, 4, 0, 3, 1);
		}

		/// <summary>
		/// Sets up collision-based damage on the entity using the given
		/// skill. Pass SkillId.None to disable collision damage.
		/// </summary>
		public static void SetCollisionDamage(this ICombatEntity entity, SkillId skillId, float damageRate)
		{
			if (entity is Character character)
			{
				if (character.Trigger == null)
					character.Components.Add(character.Trigger = new TriggerComponent(character, new CircleF(character.Position, 25)));
				else if (skillId == SkillId.None)
					character.Components.Remove<TriggerComponent>();
				character.Variables.Temp.SetInt("Melia.CollisionSkillId", (int)skillId);
				character.Variables.Temp.SetFloat("Melia.CollisionDamageRate", damageRate);
			}
			else if (entity is Mob mob)
			{
				if (mob.Trigger == null && skillId != SkillId.None)
					mob.Components.Add(mob.Trigger = new TriggerComponent(mob, new CircleF(mob.Position, mob.Data.BoundingBox.Width)));
				else if (skillId == SkillId.None)
					mob.Components.Remove<TriggerComponent>();
				mob.Vars.SetInt("Melia.CollisionSkillId", (int)skillId);
				mob.Vars.SetFloat("Melia.CollisionDamageRate", damageRate);
			}
		}

		/// <summary>
		/// Selects all entities with the given relation within range
		/// of the caster.
		/// </summary>
		public static ICombatEntity[] SelectObjects(this ICombatEntity caster, float searchRange, RelationType relation = RelationType.Enemy)
			=> SelectObjects(caster, caster.Position, searchRange, relation);

		/// <summary>
		/// Selects all entities with the given relation within range
		/// of a position.
		/// </summary>
		public static ICombatEntity[] SelectObjects(this ICombatEntity caster, Position position, float searchRange, RelationType relation = RelationType.Enemy)
		{
			var circle = new CircleF(position, searchRange);
			return caster.Map.GetActorsIn<ICombatEntity>(circle).Where(a => caster.CheckRelation(a, relation)).ToArray();
		}

		/// <summary>
		/// Selects all entities with the given relation within range
		/// of a target entity.
		/// </summary>
		public static ICombatEntity[] SelectObjectNear(this ICombatEntity caster, ICombatEntity target, float searchRange, RelationType relation = RelationType.Enemy, int i1 = 0, int i2 = 0)
		{
			var circle = new CircleF(target.Position, searchRange);
			return caster.Map.GetActorsIn<ICombatEntity>(circle).Where(a => caster.CheckRelation(a, relation)).ToArray();
		}

		/// <summary>
		/// Selects all entities matching a monster class name within range.
		/// </summary>
		public static ICombatEntity[] SelectObjectsByClassName(this ICombatEntity caster, float searchRange, string className, RelationType relation = RelationType.Enemy)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var monster))
				return [];
			var circle = new CircleF(caster.Position, searchRange);
			return caster.Map.GetActorsIn<ICombatEntity>(circle).Where(a => a is IMonster m && m.Id == monster.Id && caster.CheckRelation(a, relation)).ToArray();
		}

		/// <summary>
		/// Selects all entities within a rectangular area defined by
		/// start and end positions.
		/// </summary>
		public static ICombatEntity[] SelectObjectBySquareCoor(this ICombatEntity caster, RelationType relation, Position start, Position end, float width, int height = 0, bool drawArea = true)
		{
			Square square;
			if (height != 0)
				square = new Square(start, end, height, width);
			else
				square = new Square(start, end, width);
			if (drawArea)
				Debug.ShowShape(caster.Map, square, TimeSpan.FromSeconds(3));
			return caster.Map.GetActorsIn<ICombatEntity>(square).Where(a => caster.CheckRelation(a, relation)).ToArray();
		}

		/// <summary>
		/// Flags the entity to broadcast its owner handle on the next update.
		/// </summary>
		public static void BroadcastOwner(this ICombatEntity entity)
		{
			entity.SetTempVar("BroadcastOwner", 1);
		}

		/// <summary>
		/// Flags the entity to broadcast its relation type on the next update.
		/// </summary>
		public static void BroadcastRelation(this ICombatEntity entity)
		{
			entity.SetTempVar("BroadcastRelation", 1);
		}

		/// <summary>
		/// Sets the entity's owner. If dieWithOwner is true, the entity
		/// will be killed when its owner dies.
		/// </summary>
		public static void SetOwner(this ICombatEntity entity, ICombatEntity owner, bool dieWithOwner = false)
		{
			if (entity is Actor actor)
				actor.OwnerHandle = owner.Handle;
			if (dieWithOwner)
			{
				// Create handlers that auto-unsubscribe after triggering to prevent memory leaks
				if (owner is Character character)
				{
					Action<Character, ICombatEntity> handler = null;
					handler = (_, _) =>
					{
						character.Died -= handler;
						entity.Kill(null);
					};
					character.Died += handler;
				}
				else if (owner is Mob mob)
				{
					Action<Mob, ICombatEntity> handler = null;
					handler = (_, _) =>
					{
						mob.Died -= handler;
						entity.Kill(null);
					};
					mob.Died += handler;
				}
			}
		}

		/// <summary>
		/// Returns true if the entity's temp variable with the given name
		/// equals 1.
		/// </summary>
		public static bool CheckBoolTempVar(this ICombatEntity entity, string varName)
			=> GetTempVar(entity, varName) == 1f;

		/// <summary>
		/// Returns the float value of an entity's temporary variable.
		/// </summary>
		public static float GetTempVar(this ICombatEntity entity, string varName)
		{
			var result = 0f;
			if (entity is Character character)
				character.Variables.Temp.TryGetFloat(varName, out result);
			else if (entity is Mob mob)
				mob.Vars.TryGetFloat(varName, out result);
			return result;
		}

		/// <summary>
		/// Returns the string value of an entity's temporary variable.
		/// </summary>
		public static string GetTempVarStr(this ICombatEntity entity, string varName)
		{
			var result = "";
			if (entity is Character character)
				character.Variables.Temp.TryGetString(varName, out result);
			else if (entity is Mob mob)
				mob.Vars.TryGetString(varName, out result);
			return result;
		}

		/// <summary>
		/// Removes a temporary variable from the entity.
		/// </summary>
		public static void RemoveTempVar(this ICombatEntity entity, string varName)
		{
			if (entity is Character character)
				character.Variables.Temp.Remove(varName);
			else if (entity is Mob mob)
				mob.Vars.Remove(varName);
		}

		/// <summary>
		/// Sets a float temporary variable on the entity.
		/// </summary>
		public static void SetTempVar(this ICombatEntity entity, string varName, float value)
		{
			if (entity is Character character)
				character.Variables.Temp.SetFloat(varName, value);
			else if (entity is Mob mob)
				mob.Vars.SetFloat(varName, value);
		}

		/// <summary>
		/// Sets a boolean temporary variable on the entity.
		/// </summary>
		public static void SetTempVar(this ICombatEntity entity, string varName, bool value)
		{
			if (entity is Character character)
				character.Variables.Temp.SetBool(varName, value);
			else if (entity is Mob mob)
				mob.Vars.SetBool(varName, value);
		}

		/// <summary>
		/// Sets a string temporary variable on the entity.
		/// </summary>
		public static void SetTempVar(this ICombatEntity entity, string varName, string value)
		{
			if (entity is Character character)
				character.Variables.Temp.SetString(varName, value);
			else if (entity is Mob mob)
				mob.Vars.SetString(varName, value);
		}

		/// <summary>
		/// Sets whether the entity is hidden from monster AI targeting.
		/// </summary>
		public static void SetHideFromMon(this ICombatEntity entity, bool value)
		{
			if (entity is Character character)
				character.Variables.Temp.SetBool("Melia.HiddenFromMobs", value);
			else if (entity is Mob mob)
				mob.Vars.SetBool("Melia.HiddenFromMobs", value);
		}

		/// <summary>
		/// Returns the entity's current age in game ticks.
		/// </summary>
		public static int GetAge(this ICombatEntity entity)
		{
			if (entity is Character character)
				return character.Variables.Temp.Get<int>($"Melia.Age");
			else if (entity is Mob mob)
				return mob.Vars.Get<int>($"Melia.Age");
			return 0;
		}

		/// <summary>
		/// Invalidates and re-sends the entity's properties to clients.
		/// </summary>
		public static void InvalidateProperties(this ICombatEntity entity)
		{
			if (entity is Character character)
			{
				if (character.Connection != null)
					character.InvalidateProperties();
				else
					character.Properties.Invalidate();
			}
			else if (entity is Mob mob)
			{
				mob.Properties.Invalidate();
				Send.ZC_MOVE_SPEED(entity);
				Send.ZC_UPDATE_MHP(mob, mob.MaxHp);
			}
		}

		/// <summary>
		/// Returns true if the entity is currently in the air (not grounded).
		/// </summary>
		public static bool IsJumping(this ICombatEntity entity)
			=> !entity.Components.Get<MovementComponent>()?.IsGrounded ?? false;

		// NOTE: This is the main method in Melia for checking enemies, but
		// since it's lacking in checks compared to our relation system in laima,
		// it is temporarily disabled so there's no confusion.
		/// <summary>
		/// Returns true if the other entity is part of a hostile faction.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="otherEntity"></param>
		/// <returns></returns>
		// public static bool IsHostileFaction(this ICombatEntity entity, ICombatEntity otherEntity)
		// {
		// 	var isHostileFaction = ZoneServer.Instance.Data.FactionDb.CheckHostility(entity.Faction, otherEntity.Faction);
		// 	return isHostileFaction;
		// }

		/// <summary>
		/// Returns true if the a faction can be hit by a pad.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool IsHitByPad(this ICombatEntity entity)
		{
			var isHitByPad = ZoneServer.Instance.Data.FactionDb.IsHitByPad(entity.Faction);
			return isHitByPad;
		}

		/// <summary>
		/// Stops the entity's movement. Invokes movement component.
		/// </summary>
		/// <param name="entity"></param>
		public static void StopMove(this ICombatEntity entity)
		{
			if (entity.Components.TryGet<MovementComponent>(out var movement))
				movement.Stop();
		}

		/// <summary>
		/// Makes the entity turn towards the actor if it's not null.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="actor"></param>
		public static void TurnTowards(this IActor entity, IActor actor)
		{
			entity.TurnTowards(actor?.Position ?? Position.Zero);
		}

		/// <summary>
		/// Makes the entity turn towards a specific position.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="position"></param>
		public static void TurnTowards(this IActor entity, Position position)
		{
			if (position == Position.Zero)
				return;

			var dir = entity.Position.GetDirection(position);
			entity.TurnTowards(dir);
		}

		/// <summary>
		/// Makes the entity turn towards the given direction
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="dir"></param>
		public static void TurnTowards(this IActor entity, Direction dir)
		{
			// Don't allow turning if movement is locked (e.g., frozen)
			if (entity is ICombatEntity combatEntity && combatEntity.IsLocked(LockType.Movement))
				return;

			// Don't allow turning for monsters that can't rotate
			if (entity is Mob mob && !mob.Data.CanRotate)
				return;

			entity.Direction = dir;

			if (entity is IMonster)
				Send.ZC_QUICK_ROTATE(entity);
			else
				Send.ZC_ROTATE(entity);
		}

		/// <summary>
		/// Gets equipped item in given slot.
		/// Returns false if no item is found, or entity cannot equip items.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="slot"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool TryGetEquipItem(this ICombatEntity entity, EquipSlot slot, out Item item)
		{
			item = entity.Components.Get<InventoryComponent>()?.GetEquip(slot);
			return item != null;
		}

		/// <summary>
		/// Attempts to reduce the entity's SP by the amount necessary
		/// to use the skill. Returns false if it didn't have enough SP.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static bool TrySpendSp(this ICombatEntity entity, Skill skill)
		{
			var spendSp = skill.Properties.GetFloat(PropertyName.SpendSP);
			return entity.TrySpendSp(spendSp);
		}

		/// <summary>
		/// Attempts to reduce the entity's SP by the given amount.
		/// Returns false if it didn't have enough SP.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public static bool TrySpendSp(this ICombatEntity entity, float amount)
		{
			if (entity is not Character character)
				return true;

			if (amount == 0)
				return true;

			var sp = entity.Properties.GetFloat(PropertyName.SP);
			if (sp < amount)
				return false;

			character.ModifySp(-amount);
			return true;
		}

		/// <summary>
		/// Displays server message for entity if it's able to receive
		/// server messages.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void ServerMessage(this ICombatEntity entity, string format, params object[] args)
		{
			if (entity is Character character)
				character.ServerMessage(format, args);
		}

		/// <summary>
		/// Returns the direction from the actor to the other actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="otherActor"></param>
		/// <returns></returns>
		public static Direction GetDirection(this IActor actor, IActor otherActor)
			=> actor.Position.GetDirection(otherActor.Position);

		/// <summary>
		/// Returns the direction from the actor to the given position.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static Direction GetDirection(this IActor actor, Position pos)
			=> actor.Position.GetDirection(pos);

		/// <summary>
		/// Sets the entity's attack state.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="inAttackState"></param>
		public static void SetAttackState(this ICombatEntity entity, bool inAttackState)
			=> entity.Components.Get<CombatComponent>()?.SetAttackState(inAttackState);

		/// <summary>
		/// Sets the entity's casting state for a specific skill.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="inCastingState"></param>
		public static void SetCastingState(this ICombatEntity entity, bool inCastingState, Skill skill)
			=> entity.Components.Get<CombatComponent>()?.SetCasting(inCastingState, skill);

		/// <summary>
		/// Gets the entity's casting state for any skill.
		/// </summary>
		/// <param name="entity"></param>
		public static bool IsCasting(this ICombatEntity entity)
		{
			if (!entity.Components.TryGet<CombatComponent>(out var combat))
				return false;
			return combat.IsCasting();
		}

		/// <summary>
		/// Gets the entity's casting state for the given skill.
		/// </summary>
		/// <param name="entity"></param>
		public static bool IsCasting(this ICombatEntity entity, Skill skill)
		{
			if (!entity.Components.TryGet<CombatComponent>(out var combat))
				return false;
			return combat.IsCastingSkill(skill);
		}

		/// <summary>
		/// Returns true if the entity is casting a skill that can be
		/// casted while moving.
		/// </summary>
		/// <param name="entity"></param>
		public static bool IsMoveableCasting(this ICombatEntity entity)
			=> entity.Components.Get<CombatComponent>()?.IsMoveableCasting() ?? false;

		/// <summary>
		/// Applies a knockback to target in skill hit context.
		/// Sets knockback info on the SkillHitInfo and applies position/state changes.
		/// The knockback animation is sent embedded in the skill hit packet.
		/// Use this for skill handlers and any code using SkillHitInfo.
		/// </summary>
		/// <param name="target">The target entity to knockback</param>
		/// <param name="caster">The entity causing the knockback</param>
		/// <param name="skill">The skill causing the knockback</param>
		/// <param name="skillHit">The skill hit info (knockback data will be embedded)</param>
		public static void ApplyKnockback(this ICombatEntity target, ICombatEntity caster, Skill skill, SkillHitInfo skillHit)
		{
			if (target.CheckBeforeKnockback(caster))
				return;

			// Stop movement before knockback so the client syncs
			// position via ZC_MOVE_STOP at the correct pre-knockback
			// position, preventing a visual teleport when the entity
			// was moving.
			target.StopMove();

			if (skillHit.KnockBackInfo == null)
			{
				skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, skill);
				skillHit.HitInfo.Type = skill.Data.KnockDownHitType;
			}

			target.Position = skillHit.KnockBackInfo.ToPosition;
			target.AddState(StateType.KnockedBack, skillHit.KnockBackInfo.Time);
		}

		/// <summary>
		/// Applies a knockdown to target in skill hit context.
		/// Sets knockdown info on the SkillHitInfo and applies position/state changes.
		/// The knockdown animation is sent embedded in the skill hit packet.
		/// Use this for skill handlers and any code using SkillHitInfo.
		/// </summary>
		/// <param name="target">The target entity to knockdown</param>
		/// <param name="caster">The entity causing the knockdown</param>
		/// <param name="skill">The skill causing the knockdown</param>
		/// <param name="skillHit">The skill hit info (knockdown data will be embedded)</param>
		public static void ApplyKnockdown(this ICombatEntity target, ICombatEntity caster, Skill skill, SkillHitInfo skillHit)
		{
			if (target.CheckBeforeKnockdown(caster))
				return;

			target.StopMove();

			if (skillHit.KnockBackInfo == null)
			{
				skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, skill);
				skillHit.HitInfo.Type = skill.Data.KnockDownHitType;
			}

			target.Position = skillHit.KnockBackInfo.ToPosition;
			target.AddState(StateType.KnockedDown, skillHit.KnockBackInfo.Time);
		}


		/// <summary>
		/// Checks active buffs for IBuffBeforeKnockbackHandler. Returns
		/// true if any handler prevented the knockback.
		/// </summary>
		private static bool CheckBeforeKnockback(this ICombatEntity target, ICombatEntity attacker)
		{
			var buffs = target.Components.Get<BuffComponent>()?.GetList();
			if (buffs == null)
				return false;

			foreach (var buff in buffs)
			{
				if (buff.Handler is IBuffBeforeKnockbackHandler handler && handler.OnBeforeKnockback(buff, attacker, target) == KnockResult.Prevent)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Checks active buffs for IBuffBeforeKnockdownHandler. Returns
		/// true if any handler prevented the knockdown.
		/// </summary>
		private static bool CheckBeforeKnockdown(this ICombatEntity target, ICombatEntity attacker)
		{
			var buffs = target.Components.Get<BuffComponent>()?.GetList();
			if (buffs == null)
				return false;

			foreach (var buff in buffs)
			{
				if (buff.Handler is IBuffBeforeKnockdownHandler handler && handler.OnBeforeKnockdown(buff, attacker, target) == KnockResult.Prevent)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Gets if the entity's knocked back.
		/// </summary>
		/// <param name="entity"></param>
		public static bool IsKnockedBack(this ICombatEntity entity)
			=> entity.Components.Get<StateLockComponent>()?.IsStateActive(StateType.KnockedBack) ?? false;

		/// <summary>
		/// Gets if the entity's knocked down.
		/// </summary>
		/// <param name="entity"></param>
		public static bool IsKnockedDown(this ICombatEntity entity)
			=> entity.Components.Get<StateLockComponent>()?.IsStateActive(StateType.KnockedDown) ?? false;

		/// <summary>
		/// Gets the entity's guarding state.
		/// </summary>
		/// <param name="entity"></param>
		public static bool IsGuarding(this ICombatEntity entity)
			=> entity.Components.Get<CombatComponent>()?.IsGuarding ?? false;

		/// <summary>
		/// Sets the entity's guard state.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="isGuarding"></param>
		public static void SetGuardState(this ICombatEntity entity, bool isGuarding)
		{
			if (entity.Components.TryGet<CombatComponent>(out var combat))
				combat.IsGuarding = isGuarding;
		}

		/// <summary>
		/// Gets the entity's safe state.
		/// </summary>
		/// <param name="entity"></param>
		public static bool IsSafe(this ICombatEntity entity)
			=> entity.Components.Get<CombatComponent>()?.IsSafe ?? false;

		/// <summary>
		/// Sets the entity's safe state.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="isSafe"></param>
		public static void SetSafeState(this ICombatEntity entity, bool isSafe)
		{
			if (entity.Components.TryGet<CombatComponent>(out var combat))
				combat.IsSafe = isSafe;
		}

		/// <summary>
		/// Gets the entity's stagger state.
		/// </summary>
		public static bool IsStaggered(this ICombatEntity entity)
			=> entity.Components.Get<CombatComponent>()?.IsStaggered ?? false;

		/// <summary>
		/// Applies stagger to the entity.
		/// </summary>
		public static void ApplyStagger(this ICombatEntity entity)
			=> entity.Components.Get<CombatComponent>()?.ApplyStagger();

		/// <summary>
		/// Ends stagger on the entity.
		/// </summary>
		public static void EndStagger(this ICombatEntity entity)
			=> entity.Components.Get<CombatComponent>()?.EndStagger();

		/// <summary>
		/// Checks if an entity is a specific race type
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="race"></param>
		/// <param name="bossCheck"></param>
		/// <returns></returns>
		public static bool IsRaceType(this ICombatEntity entity, RaceType race, bool bossCheck = true)
		{
			if (entity.Race == race)
			{
				if (entity is Mob mob)
				{
					if (mob.Properties.GetString(PropertyName.MonRank) == MonsterRank.Boss.ToString() && bossCheck)
						return true;
					else if (mob.Properties.GetString(PropertyName.MonRank) == MonsterRank.Boss.ToString() && !bossCheck)
						return false;
				}
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns true if the entity has the given ability and it's toggled on.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public static bool IsAbilityActive(this ICombatEntity entity, AbilityId abilityId)
			=> entity.Components.Get<AbilityComponent>()?.IsActive(abilityId) ?? false;

		/// <summary>
		/// Get an ability level.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public static int GetAbilityLevel(this ICombatEntity entity, AbilityId abilityId) =>
			entity.Components.Get<AbilityComponent>()?.GetLevel(abilityId) ?? 0;


		/// <summary>
		/// Get's an entity's owner.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool TryGetOwner(this ICombatEntity entity, out ICombatEntity owner)
		{
			owner = default;
			if (entity is ISubActor subActor && entity.Map.TryGetCombatEntity(subActor.OwnerHandle, out owner))
				return true;
			return false;
		}

		/// <summary>
		/// Checks if an owner's ability is active.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public static bool IsOwnerAbilityActive(this ICombatEntity entity, AbilityId abilityId)
		{
			if (entity is ISubActor subActor && entity.Map.TryGetCombatEntity(subActor.OwnerHandle, out var owner))
				return owner.IsAbilityActive(abilityId);
			else
				return entity.IsAbilityActive(abilityId);
		}

		/// <summary>
		/// Returns the ability with the given id via out if the entity has it.
		/// Returns false if the entity doesn't have the ability.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="abilityId"></param>
		/// <param name="ability"></param>
		/// <returns></returns>
		public static bool TryGetAbility(this ICombatEntity entity, AbilityId abilityId, out Ability? ability)
		{
			ability = default;
			return entity.Components.Get<AbilityComponent>()?.TryGet(abilityId, out ability) ?? false;
		}

		/// <summary>
		/// Returns the ability if it's active with the given id via out and if the entity has it.
		/// Returns false if the entity doesn't have the ability or it's not active.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="abilityId"></param>
		/// <param name="ability"></param>
		/// <returns></returns>
		public static bool TryGetActiveAbility(this ICombatEntity entity, AbilityId abilityId, out Ability? ability)
		{
			ability = default;
			return entity.Components.Get<AbilityComponent>()?.TryGetActive(abilityId, out ability) ?? false;
		}

		/// <summary>
		/// Returns true if the entity has the given ability and it's toggled on.
		/// Returns the ability's level via out if it's active.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="abilityId"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public static bool TryGetActiveAbilityLevel(this ICombatEntity entity, AbilityId abilityId, out int level)
		{
			level = 0;

			if (!entity.Components.TryGet<AbilityComponent>(out var abilities))
				return false;

			if (!abilities.TryGetActive(abilityId, out var ability))
				return false;

			level = ability.Level;
			return ability.Active;
		}

		/// <summary>
		/// Checks if a given skilldown exists.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static bool IsOnCooldown(this ICombatEntity entity, SkillId skillId)
		{
			if (entity.TryGetSkill(skillId, out var skill))
				return entity.Components.Get<CooldownComponent>()?.IsOnCooldown(skill.CooldownData.Id) ?? false;
			return false;
		}

		/// <summary>
		/// Removes the cooldown with a given skill id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static void RemoveCooldown(this ICombatEntity entity, SkillId skillId)
		{
			if (entity.TryGetSkill(skillId, out var skill))
				entity.Components.Get<CooldownComponent>()?.Remove(skill.CooldownData.Id);
		}

		/// <summary>
		/// Checks if a given cooldown exists.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="cooldownId"></param>
		/// <returns></returns>
		public static bool IsOnCooldown(this ICombatEntity entity, CooldownId cooldownId) =>
			entity.Components.Get<CooldownComponent>()?.IsOnCooldown(cooldownId) ?? false;

		/// <summary>
		/// Removes the cooldown with a given id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="cooldownId"></param>
		/// <returns></returns>
		public static bool RemoveCooldown(this ICombatEntity entity, CooldownId cooldownId)
			=> entity.Components.Get<CooldownComponent>()?.Remove(cooldownId) ?? false;

		/// <summary>
		/// Starts the cooldown with a given id and duration.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="cooldownId"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static Cooldown StartCooldown(this ICombatEntity entity, CooldownId cooldownId, TimeSpan duration)
			=> entity.Components.Get<CooldownComponent>()?.Start(cooldownId, duration);

		/// <summary>
		/// Remove buff with the given id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffId"></param>
		public static void RemoveBuff(this ICombatEntity entity, BuffId buffId)
			=> entity.Components.Get<BuffComponent>()?.Remove(buffId);

		/// <summary>
		/// Remove buff with the given id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffId"></param>
		public static void RemoveBuffByCaster(this ICombatEntity entity, ICombatEntity caster, BuffId buffId)
			=> entity.Components.Get<BuffComponent>()?.Remove(buff => buff.Caster == caster && buff.Id == buffId);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff.
		/// </summary>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public static Buff StartBuff(this ICombatEntity entity, BuffId buffId)
			=> entity.Components.Get<BuffComponent>()?.Start(buffId, 0, 0, Buff.DefaultDuration, entity);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffId"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static Buff StartBuff(this ICombatEntity entity, BuffId buffId, TimeSpan duration)
			=> entity.Components.Get<BuffComponent>()?.Start(buffId, 0, 0, duration, entity);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static Buff StartBuff(this ICombatEntity entity, BuffId buffId, TimeSpan duration, IActor caster)
			=> entity.Components.Get<BuffComponent>()?.Start(buffId, 0, 0, duration, caster);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="duration"></param>
		/// <param name="caster"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static Buff StartBuff(this ICombatEntity entity, BuffId buffId, float numArg1, float numArg2, TimeSpan duration, IActor caster, SkillId skillId = SkillId.None)
			=> entity?.Components.Get<BuffComponent>()?.Start(buffId, numArg1, numArg2, duration, caster, skillId);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="duration"></param>
		/// <param name="caster"></param>
		/// <param name="skillId"></param>
		/// <param name="initializer">Optional action to initialize buff variables before activation.</param>
		/// <returns></returns>
		public static Buff StartBuff(this ICombatEntity entity, BuffId buffId, float numArg1, float numArg2, TimeSpan duration, IActor caster, SkillId skillId, Action<Buff> initializer)
			=> entity?.Components.Get<BuffComponent>()?.Start(buffId, numArg1, numArg2, duration, caster, skillId, initializer);

		/// <summary>
		/// Stops the buff with the given id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffIds"></param>
		public static void StopBuff(this ICombatEntity entity, params BuffId[] buffIds)
			=> entity.Components.Get<BuffComponent>()?.Stop(buffIds);

		/// <summary>
		/// Stops all buff with the given tags.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffTags"></param>
		public static void StopBuffByTag(this ICombatEntity entity, params string[] buffTags)
			=> entity.Components.Get<BuffComponent>()?.RemoveBuff(buffTags);

		/// <summary>
		/// Starts the buff if it's not active, or removes it if it is.
		/// </summary>
		public static void ToggleBuff(this ICombatEntity target, BuffId buffId, float numArg1, float numArg2, TimeSpan duration, IActor caster, SkillId skillId = SkillId.Normal_Attack)
		{
			if (target.IsBuffActive(buffId))
				target.RemoveBuff(buffId);
			else
				target.StartBuff(buffId, numArg1, numArg2, TimeSpan.Zero, caster);
		}

		/// <summary>
		/// Returns true if the buff with the given id is active.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffIds"></param>
		/// <returns></returns>
		public static bool IsAnyBuffActive(this ICombatEntity entity, params BuffId[] buffIds)
			=> entity.Components.Get<BuffComponent>()?.HasAny(buffIds) ?? false;

		/// <summary>
		/// Returns true if the buff with the given id is active.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public static bool IsBuffActive(this ICombatEntity entity, BuffId buffId)
			=> entity.Components.Get<BuffComponent>()?.Has(buffId) ?? false;

		/// <summary>
		/// Returns true if entity has any buffs active.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool HasBuffs(this ICombatEntity entity)
			=> entity.Components.Get<BuffComponent>()?.Count > 0;

		/// <summary>
		/// Returns true if the buff with the given id is found.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public static bool TryGetBuff(this ICombatEntity entity, BuffId buffId, out Buff buff)
		{
			buff = default;
			return entity.Components.Get<BuffComponent>()?.TryGet(buffId, out buff) ?? false;
		}

		/// <summary>
		/// Returns true if the buff with the given id is found.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public static bool TryGetBuffByKeyword(this ICombatEntity entity, string keyword, out Buff buff)
		{
			buff = entity.Components.Get<BuffComponent>()?.GetAll(a => a.Data.Tags.Has(keyword)).FirstOrDefault() ?? default;
			return buff != null;
		}



		/// <summary>
		/// Returns true if the buff with the given id is found.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool IsBuffActiveByKeyword(this ICombatEntity entity, params string[] keyword)
		{
			return entity.Components.Get<BuffComponent>()?.Exists(b => b.Data.Tags.HasAny(keyword)) ?? false;
		}

		/// <summary>
		/// Gets a float property value from the entity via out.
		/// Returns false and the default value if not found.
		/// </summary>
		public static bool TryGetProp(this IPropertyHolder entity, string propertyName, out float value, float defaultValue = default)
		{
			if (!entity.Properties.TryGetFloat(propertyName, out value))
			{
				value = defaultValue;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Gets a string property value from the entity via out.
		/// Returns false and the default value if not found.
		/// </summary>
		public static bool TryGetProp(this IPropertyHolder entity, string propertyName, out string value, string defaultValue = default)
		{
			if (!entity.Properties.TryGet(propertyName, out value))
			{
				value = defaultValue;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns true if the skill with the given id is found.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static bool TryGetSkill(this ICombatEntity entity, SkillId skillId, out Skill skill)
		{
			skill = null;
			if (entity is Character character)
				character.Skills.TryGet(skillId, out skill);
			else
				entity.Components.Get<BaseSkillComponent>()?.TryGet(skillId, out skill);
			return skill != null;
		}

		/// <summary>
		/// Returns true if the skill level is greater than 0 with the given id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static bool TryGetSkillLevel(this ICombatEntity entity, SkillId skillId, out int level)
		{
			Skill? skill = null;
			if (entity is Character character)
				character.Skills.TryGet(skillId, out skill);
			else
				entity.Components.Get<BaseSkillComponent>()?.TryGet(skillId, out skill);
			level = skill?.Level ?? 0;
			return level > 0;
		}

		/// <summary>
		/// Returns true if the distance between the caster and the target
		/// doesn't exceed the skill's max range.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool InSkillUseRange(this ICombatEntity caster, Skill skill, ICombatEntity target)
			=> InSkillUseRange(caster, skill, target.Position);

		/// <summary>
		/// Returns true if the distance between the caster and the position
		/// doesn't exceed the skill's max range.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static bool InSkillUseRange(this ICombatEntity caster, Skill skill, Position pos)
		{
			var maxRange = skill.Properties.GetFloat(PropertyName.MaxR);

			// There are somewhat frequent situations where the client is
			// convinced it's in range, but the server disagrees. It's good
			// that we have these checks, but we also want the experience
			// to be smooth, so we'll allow a little extra range.
			maxRange *= 1.25f;

			return caster.Position.InRange2D(pos, maxRange);
		}

		/// <summary>
		/// Returns true if the entity has the skill at at least the given level.
		/// </summary>
		/// <remarks>
		/// Currently only works for characters, as monsters don't have a skill
		/// component yet. It will always return false for monsters.
		/// </remarks>
		/// <param name="entity"></param>
		/// <param name="skillId"></param>
		/// <param name="minLevel"></param>
		/// <returns></returns>
		public static bool HasSkill(this ICombatEntity entity, SkillId skillId, int minLevel = 1)
			=> entity.Components.Get<SkillComponent>()?.GetLevel(skillId) >= minLevel;

		/// <summary>
		/// Returns the entity from the collection that is closest to the
		/// given position.
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static ICombatEntity GetClosest(this IEnumerable<ICombatEntity> entities, Position pos)
		{
			var closest = (ICombatEntity)null;
			var closestDist = float.MaxValue;

			foreach (var entity in entities)
			{
				var dist = (float)entity.Position.Get2DDistance(pos);
				if (dist < closestDist)
				{
					closest = entity;
					closestDist = dist;
				}
			}

			return closest;
		}

		/// <summary>
		/// Returns the entity from the collection that is closest to the
		/// given position and matches the predicate.
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="pos"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static ICombatEntity GetClosest(this IEnumerable<ICombatEntity> entities, Position pos, Func<ICombatEntity, bool> predicate)
		{
			var closest = (ICombatEntity)null;
			var closestDist = float.MaxValue;

			foreach (var entity in entities)
			{
				if (!predicate(entity))
					continue;

				var dist = (float)entity.Position.Get2DDistance(pos);
				if (dist < closestDist)
				{
					closest = entity;
					closestDist = dist;
				}
			}

			return closest;
		}

		/// <summary>
		/// Applies damage to the specified combat entity as a result of a skill hit.
		/// </summary>
		/// <remarks>This method calculates and applies damage to the target entity and sends hit information to the
		/// relevant systems. If the <paramref name="caster"/> is not a combat entity, the method logs a debug message and
		/// exits without applying damage.</remarks>
		/// <param name="entity">The combat entity receiving the damage.</param>
		/// <param name="damage">The amount of damage to apply to the entity.</param>
		/// <param name="caster">The actor responsible for casting the skill. Must implement <see cref="ICombatEntity"/>.</param>
		/// <param name="skillId">The identifier of the skill causing the hit.</param>
		/// <param name="hitType">The type of hit (e.g., normal, critical). Defaults to <see cref="HitType.Normal"/>.</param>
		public static void TakeSimpleHit(this ICombatEntity entity, float damage, IActor caster, SkillId skillId = SkillId.Normal_Attack, HitType hitType = HitType.Normal)
		{
			if (caster is not ICombatEntity attacker)
			{
				Log.Debug("TakeDamage: Tried to attack while being a non-combat entity.");
				return;
			}

			entity.TakeDamage(damage, attacker);

			var hit = new HitInfo(attacker, entity, skillId, damage, HitResultType.Hit);
			hit.Type = hitType;
			Send.ZC_HIT_INFO(attacker, entity, hit);
		}

		/// <summary>
		/// Applies a skill hit to the target, making it take damage as if hit
		/// by the skill.
		/// </summary>
		/// <remarks>
		/// Simulates a basic skill hit, without any additional effects.
		/// </remarks>
		/// <param name="entity"></param>
		/// <param name="attacker"></param>
		/// <param name="skill"></param>
		public static void TakeSkillHit(this ICombatEntity entity, ICombatEntity attacker, Skill skill, HitType hitType = HitType.Normal)
		{
			var caster = attacker;
			var target = entity;

			var skillHitResult = SCR_SkillHit(caster, target, skill);

			target.TakeDamage(skillHitResult.Damage, caster);
			var hit = new HitInfo(caster, target, skill, skillHitResult);
			hit.Type = hitType;
			Send.ZC_HIT_INFO(caster, target, hit);
		}

		/// <summary>
		/// Removes a random buff from the entity with the given chance in percent.
		/// </summary>
		/// <remarks>
		/// If chance is 100 or above, a random buff will always be removed,
		/// assuming there is one to remove. Only buffs that are removable
		/// by skills are considered removable.
		/// </remarks>
		/// <param name="entity"></param>
		/// <param name="chance"></param>
		public static void RemoveRandomBuff(this ICombatEntity entity, float chance = 100)
		{
			var rnd = RandomProvider.Get();

			if (rnd.Next(100) < chance && entity.Components.TryGet<BuffComponent>(out var buffs))
				buffs.RemoveRandomBuff();
		}

		/// <summary>
		/// Removes a random debuff from the entity with the given chance in percent.
		/// </summary>
		/// <remarks>
		/// If chance is 100 or above, a random debuff will always be removed,
		/// assuming there is one to remove. Only debuffs that are removable
		/// by skills are considered removable.
		/// </remarks>
		/// <param name="entity"></param>
		/// <param name="chance"></param>
		public static void RemoveRandomDebuff(this ICombatEntity entity, float chance = 100)
		{
			var rnd = RandomProvider.Get();

			if (rnd.Next(100) < chance && entity.Components.TryGet<BuffComponent>(out var buffs))
				buffs.RemoveRandomDebuff();
		}

		/// <summary>
		/// Returns true if the entity is behind the target.
		/// </summary>
		/// <remarks>
		/// Uses the target's current direction and the given max angle to
		/// determine if the entity is behind it.
		/// </remarks>
		/// <param name="entity"></param>
		/// <param name="target"></param>
		/// <param name="maxAngle"></param>
		/// <returns></returns>
		public static bool IsBehind(this ICombatEntity entity, ICombatEntity target, float maxAngle = 90)
		{
			var casterAngle = entity.Direction.DegreeAngle;
			var targetAngle = target.Direction.DegreeAngle;

			return Math.Abs(casterAngle - targetAngle) < maxAngle || Math.Abs(casterAngle + 360f - targetAngle) < maxAngle || Math.Abs(casterAngle - targetAngle + 360f) < maxAngle;
		}

		/// <summary>
		/// Returns true if a lock of the given type is active, indicating
		/// that they should not be able to take the action.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="lockType"></param>
		/// <returns></returns>
		public static bool IsLocked(this ICombatEntity entity, string lockType)
			=> entity.Components.Get<StateLockComponent>()?.IsLocked(lockType) ?? false;

		/// <summary>
		/// Adds a lock for the given type.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="lockType"></param>
		public static void Lock(this ICombatEntity entity, string lockType)
			=> entity.Components.Get<StateLockComponent>()?.Lock(lockType);

		/// <summary>
		/// Adds a lock for the given type.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="lockType"></param>
		/// <param name="duration"></param>
		public static void Lock(this ICombatEntity entity, string lockType, TimeSpan duration)
			=> entity.Components.Get<StateLockComponent>()?.Lock(lockType, duration);

		/// <summary>
		/// Remove a lock for the given type.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="lockType"></param>
		public static void Unlock(this ICombatEntity entity, string lockType)
			=> entity.Components.Get<StateLockComponent>()?.Unlock(lockType);

		/// <summary>
		/// Locks the actions assoctiated with the given state.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="stateType"></param>
		public static void AddState(this ICombatEntity entity, string stateType)
			=> entity.Components.Get<StateLockComponent>()?.AddState(stateType);

		/// <summary>
		/// Locks the actions assoctiated with the given state for the given
		/// duration.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="stateType"></param>
		/// <param name="duration"></param>
		public static void AddState(this ICombatEntity entity, string stateType, TimeSpan duration)
			=> entity.Components.Get<StateLockComponent>()?.AddState(stateType, duration);

		/// <summary>
		/// Removes one set of the locks associated with the given state.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="stateType"></param>
		public static void RemoveState(this ICombatEntity entity, string stateType)
			=> entity.Components.Get<StateLockComponent>()?.RemoveState(stateType);

		/// <summary>
		/// Returns true if the given state is active.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="stateType"></param>
		/// <returns></returns>
		public static bool IsStateActive(this ICombatEntity entity, string stateType)
			=> entity.Components.Get<StateLockComponent>()?.IsStateActive(stateType) ?? false;

		/// <summary>
		/// Resizes the entity to the given scale after a brief delay. If
		/// the entity has a "Kill" temp var set, it will be killed after
		/// resizing.
		/// </summary>
		public static async void Resize(this ICombatEntity entity, float scale)
		{
			await Task.Delay(500);
			if (entity is Character)
				return;

			if (entity.CheckBoolTempVar("Kill"))
			{
				entity.ChangeScale(scale, 0.2f);
				entity.Kill(null);
				entity.PlayEffect("F_buff_explosion_burst");
			}
			else
				entity.ChangeScale(scale, 0.2f);
		}

		/// <summary>
		/// Returns the entity's currently active companion via out.
		/// Returns false if no companion is active.
		/// </summary>
		public static bool TryGetActiveCompanion(this ICombatEntity entity, out Companion companion)
		{
			companion = entity.Components.Get<CompanionComponent>()?.ActiveCompanion ?? null;
			return companion != null;
		}

		/// <summary>
		/// Returns true if the entity is currently riding its companion.
		/// </summary>
		public static bool IsRiding(this ICombatEntity entity)
			=> entity.Components.Get<CompanionComponent>()?.ActiveCompanion?.IsRiding ?? false;

		/// <summary>
		/// Sends a localized system message to the entity if it is a character.
		/// </summary>
		public static void SendSysMsg(this ICombatEntity entity, string msg, params MsgParameter[] args)
		{
			if (entity is Character character)
				character.SystemMessage(msg, args);
		}

		/// <summary>
		/// Returns true if the target is dead or gone.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool TargetGone(this ICombatEntity entity, ICombatEntity target)
		{
			if (target == null)
				return true;

			if (target.IsDead)
				return true;

			if (entity.Map.GetCombatEntity(target.Handle) == null)
				return true;

			return false;
		}

		/// <summary>
		/// Returns true if the entity is in range of the given target.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public static bool InRangeOfTarget(this ICombatEntity entity, ICombatEntity target, float range)
		{
			return entity.Position.InRange2D(target.Position, (int)range);
		}

		/// <summary>
		/// Makes the entity spin and throw the target, playing a spinning
		/// throw animation.
		/// </summary>
		public static void SpinThrow(this ICombatEntity entity, float rotDelay, float accelTime, int rotCount, float rotSec, ICombatEntity target, Position throwPos, string throwKey, string kdKey, string monName, string nodeName, string dragEft, float dragEftScale, float SR)
		{
			// throwKey might be Send.ZC_SYNC_EXEC or Send.ZC_SYNC_EXEC_BY_SKILL_TIME
			Send.ZC_NORMAL.SpinThrow(entity, rotDelay, accelTime, rotCount, rotSec, target, throwPos, throwKey, monName, nodeName, dragEft, dragEftScale, SR);
		}

		/// <summary>
		/// Makes the entity spin in place.
		/// </summary>
		public static void SpinObject(this ICombatEntity entity, float spinDelay = 0, int spinCount = 0, float rotationPerSecond = 0, float velocityChangeTerm = 0)
		{
			Send.ZC_NORMAL.SpinObject(entity, spinDelay, spinCount, rotationPerSecond, velocityChangeTerm);
		}

		/// <summary>
		/// Throws a monster that was attached to this entity.
		/// </summary>
		public static void ThrowAttachedMonster(this ICombatEntity entity, ICombatEntity caster, string monName, Position throwPos, float throwSpd, float hideTime)
		{
			Send.ZC_NORMAL.ThrowAttachedMonster(entity, caster, monName, throwPos, throwSpd, hideTime);
		}

		/// <summary>
		/// Enables or disables invincibility on the entity via buff.
		/// </summary>
		public static void SetInvincible(this ICombatEntity entity, bool isOn)
		{
			if (isOn)
				entity.StartBuff(BuffId.Invincible);
			else
				entity.RemoveBuff(BuffId.Invincible);
		}
	}
}
