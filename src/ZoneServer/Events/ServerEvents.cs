using Melia.Zone.Events.Arguments;
using Yggdrasil.Events;

namespace Melia.Zone.Events
{
	/// <summary>
	/// Manager for events occurring on the server, such as players logging
	/// in or killing monsters.
	/// </summary>
	public class ServerEvents
	{
		/// <summary>
		/// Manager for dynamic events that can be added and removed at runtime.
		/// </summary>
		public readonly DynamicEventManager Dynamic = new();

		// Time Events
		//-------------------------------------------------------------------

		/// <summary>
		/// Raised on every heartbeat tick.
		/// </summary>
		public Event<TimeEventArgs> WorldTick = new();

		/// <summary>
		/// Raised every full real-life second.
		/// </summary>
		public Event<TimeEventArgs> SecondTick = new();

		/// <summary>
		/// Raised every full real-life minute.
		/// </summary>
		public Event<TimeEventArgs> MinuteTick = new();

		/// <summary>
		/// Raised every full five real-life minutes.
		/// </summary>
		public Event<TimeEventArgs> FiveMinutesTick = new();

		/// <summary>
		/// Raised every full fifteen real-life minutes.
		/// </summary>
		public Event<TimeEventArgs> FifteenMinutesTick = new();

		/// <summary>
		/// Raised every full twenty real-life minutes.
		/// </summary>
		public Event<TimeEventArgs> TwentyMinutesTick = new();

		/// <summary>
		/// Raised every full thirty real-life minutes.
		/// </summary>
		public Event<TimeEventArgs> ThirtyMinutesTick = new();

		/// <summary>
		/// Raised every full real-life hour.
		/// </summary>
		public Event<TimeEventArgs> HourTick = new();

		// Player Events
		//-------------------------------------------------------------------

		/// <summary>
		/// Raised when a player logged in.
		/// </summary>
		/// <remarks>
		/// This event is raised right after the login was confirmed and
		/// before any more packets are sent to the client or they are
		/// added to the world. This makes it a good time to make 
		/// modifications to the character, but packets sent to the
		/// client might not get handled as intended yet.
		/// </remarks>
		public Event<PlayerEventArgs> PlayerLoggedIn = new();

		/// <summary>
		/// Raised after the player's client loaded the map and is ready
		/// to start the game.
		/// </summary>
		/// <remarks>
		/// This event is similar to PlayerReady and fires from the same
		/// packet handler, but instead of waiting until the character
		/// and all its data is fully initialized, it fires immediately,
		/// which allows for redirects, such as rerouting the character
		/// to a different map.
		/// 
		/// If the packet handling should be stopped after executing this
		/// event, set CancelHandling on the arguments to true.
		/// </remarks>
		public Event<PlayerGameReadyEventArgs> PlayerGameReady = new();

		/// <summary>
		/// Raised when a player logged in completely.
		/// </summary>
		/// <remarks>
		/// This event is raised right after we start sending the client
		/// updates about the world, such as monsters and players moving
		/// around. At this point, the client is expected to be fully
		/// loaded and ready to receive whatever you throw at it.
		/// </remarks>
		public Event<PlayerEventArgs> PlayerReady = new();

		/// <summary>
		/// Raised when a player gains a new job or circle.
		/// </summary>
		public Event<PlayerEventArgs> PlayerAdvancedJob = new();

		/// <summary>
		/// Raised when a player says something in public chat.
		/// </summary>
		public Event<PlayerChatEventArgs> PlayerChat = new();

		/// <summary>
		/// Raised when the level of a player's skill changed.
		/// </summary>
		public Event<PlayerSkillLevelChangedEventArgs> PlayerSkillLevelChanged = new();

		/// <summary>
		/// Raised when a player gains new items, such as by picking them up
		/// or buying them.
		/// </summary>
		public Event<PlayerItemEventArgs> PlayerAddedItem = new();

		/// <summary>
		/// Raised when a player loses items, such as by destroying or selling them.
		/// </summary>
		public Event<PlayerItemEventArgs> PlayerRemovedItem = new();

		/// <summary>
		/// Raised when a dialog message is sent to a player.
		/// </summary>
		public Event<PlayerDialogEventArgs> PlayerDialog = new();

		/// <summary>
		/// Raised when a player leaves a party.
		/// </summary>
		public Event<PlayerEventArgs> PlayerLeftParty = new();

		/// <summary>
		/// Raised when a player enters a map.
		/// </summary>
		public Event<PlayerEventArgs> PlayerEnteredMap = new();

		/// <summary>
		/// Raised when a player's client has finished loading the map.
		/// </summary>
		/// <remarks>
		/// This event fires from the CZ_LOAD_COMPLETE packet handler,
		/// after the client has fully loaded the map and is ready to
		/// receive gameplay actions such as dungeon stage starts,
		/// companion spawns, and other post-load operations.
		/// </remarks>
		public Event<PlayerEventArgs> PlayerLoadComplete = new();

		/// <summary>
		/// Raised when a player leaves a map.
		/// </summary>
		public Event<PlayerEventArgs> PlayerLeftMap = new();

		// Combat Events
		//-------------------------------------------------------------------

		/// <summary>
		/// Raised when one entity kills another.
		/// </summary>
		public Event<CombatEventArgs> EntityKilled = new();

		// Monster Events
		//-------------------------------------------------------------------

		/// <summary>
		/// Raised when a player uses an item.
		/// </summary>
		public Event<PlayerUsedItemEventArgs> PlayerUsedItem = new();

		/// <summary>
		/// Raised when a player completes a quest.
		/// </summary>
		public Event<PlayerCompletedQuestEventArgs> PlayerCompletedQuest = new();

		/// <summary>
		/// Raised when a player starts a quest.
		/// </summary>
		public Event<PlayerStartedQuestEventArgs> PlayerStartedQuest = new();

		/// <summary>
		/// Raised when a player abandons a quest.
		/// </summary>
		public Event<PlayerAbandonedQuestEventArgs> PlayerAbandonedQuest = new();

		/// <summary>
		/// Raised when a quest's objectives have all been completed.
		/// </summary>
		public Event<PlayerQuestObjectivesCompletedEventArgs> PlayerQuestObjectivesCompleted = new();

		/// <summary>
		/// Raised when a player's reputation with a faction changes.
		/// </summary>
		public Event<ReputationEventArgs> PlayerReputationChanged = new();

		/// <summary>
		/// Raised when a monster is about to disappear.
		/// </summary>
		/// <remarks>
		/// Raised regardless of why the monster disappears, be it
		/// because it died, its summoning time ran out, or it got
		/// bored of our antics.
		/// </remarks>
		public Event<MonsterEventArgs> MonsterDisappears = new();
	}
}
