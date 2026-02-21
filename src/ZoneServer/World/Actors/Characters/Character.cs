using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Const.Web;
using Melia.Shared.Game.Properties;
using Melia.Shared.L10N;
using Melia.Shared.Network.Helpers;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Scripting;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Buffs.Handlers.Scout.Assassin;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Storages;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.World.Actors.Characters
{
	/// <summary>
	/// Represents a player character.
	/// </summary>
	public partial class Character : Actor, ICombatEntity, ICommander, IPropertyObject, IUpdateable
	{
		#region Private Fields
		private const int MaxMonsterAppearPerTick = 8;

		private int _destinationChannelId;
		private readonly object _warpLock = new();
		private readonly object _lookAroundLock = new();
		private readonly object _hpLock = new();
		private IMonster[] _visibleMonsters = [];
		private Character[] _visibleCharacters = [];
		private Pad[] _visiblePads = [];
		private readonly HashSet<Pad> _observedPads = [];
		private readonly static TimeSpan ResurrectDialogDelay = TimeSpan.FromSeconds(2);
		private TimeSpan _resurrectDialogTimer = ResurrectDialogDelay;
		private Localizer _localizer;
		private Companion _companionToReactivate;
		private DateTime _pendingCompanionActivation = DateTime.MinValue;
		private readonly static TimeSpan CompanionActivationDelay = TimeSpan.FromSeconds(2);
		#endregion

		#region Core Properties
		private Position _position;

		/// <summary>
		/// Returns the character's position on its current map.
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
		/// Returns when the character was last saved.
		/// </summary>
		public DateTime LastSaved { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Returns whether the character is travelling between maps.
		/// </summary>
		public bool IsWarping { get; set; } = true;

		/// <summary>
		/// Returns true if the character was just saved before a warp.
		/// </summary>
		public bool SavedForWarp { get; internal set; }

		/// <summary>
		/// Connection this character uses.
		/// </summary>
		public virtual IZoneConnection Connection { get; set; }

		/// <summary>
		/// Returns the name of the character's account.
		/// </summary>
		public string Username => this.Connection.Account.Name;

		/// <summary>
		/// Gets or sets the character's unique database id.
		/// </summary>
		public long DbId { get; set; }

		/// <summary>
		/// Returns the character's globally unique id.
		/// </summary>
		public long ObjectId => ObjectIdRanges.Characters + this.DbId;

		/// <summary>
		/// Returns the character's globally unique id on the social server.
		/// </summary>
		public long SocialUserId => ObjectIdRanges.SocialUser + this.DbId;

		/// <summary>
		/// Id of the character's account.
		/// </summary>
		public long AccountDbId { get; set; }

		/// <summary>
		/// Id of the character's account.
		/// </summary>
		public long AccountObjectId => ObjectIdRanges.Accounts + this.AccountDbId;

		/// <summary>
		/// Character's name.
		/// </summary>
		public override string Name { get; set; }

		/// <summary>
		/// Character's team name.
		/// </summary>
		public string TeamName { get; set; }
		#endregion

		#region Character Appearance & State
		/// <summary>
		/// Character's gender.
		/// </summary>
		public Gender Gender { get; set; }

		/// <summary>
		/// Character's hair style.
		/// </summary>
		public int Hair { get; set; }

		/// <summary>
		/// Gets or sets the character's skin color.
		/// </summary>
		public uint SkinColor { get; set; }

		/// <summary>
		/// Character's pose.
		/// </summary>
		public byte Pose { get; set; }

		/// <summary>
		/// Returns stance, based on job and other factors.
		/// </summary>
		public int Stance { get; protected set; }

		/// <summary>
		/// Character's head's direction.
		/// </summary>
		public Direction HeadDirection { get; set; }

		/// <summary>
		/// Gets or sets whether the character is sitting.
		/// </summary>
		public bool IsSitting { get; set; }

		/// <summary>
		/// Specifies which hats are visible on the character.
		/// </summary>
		public VisibleEquip VisibleEquip { get; set; } = VisibleEquip.All;

		/// <summary>
		/// Gets or set the character's greeting message.
		/// </summary>
		public string GreetingMessage { get; set; }

		/// <summary>
		/// Gets or sets the character's chat balloon.
		/// </summary>
		public int ChatBalloon { get; set; } = 1;

		/// <summary>
		/// Get or set the character's chat balloon expiration
		/// </summary>
		public DateTime ChatBalloonExpiration { get; set; } = new DateTime(2999, 12, 31, 23, 59, 59, DateTimeKind.Local);

		/// <summary>
		/// Character's Balloon Id
		/// </summary>
		public int BalloonId { get; set; }

		/// <summary>
		/// Animation Pairing
		/// </summary>
		public bool IsPaired { get; set; }

		/// <summary>
		/// Character is riding a "vehicle" (Companion)
		/// </summary>
		public bool IsRiding { get; set; }

		/// <summary>
		/// Character's online status.
		/// </summary>
		public bool IsOnline { get; set; } = false;

		/// <summary>
		/// Returns true if the character is autotrading (offline vending).
		/// When autotrading, the character remains in the world with their
		/// shop open while the player is disconnected.
		/// </summary>
		public bool IsAutoTrading { get; set; } = false;
		#endregion

		#region Game System Properties
		/// <summary>
		/// Returns the character's race.
		/// </summary>
		public RaceType Race => RaceType.None;

		/// <summary>
		/// Returns the character's element/attribute.
		/// </summary>
		public AttributeType Attribute => (AttributeType)(int)this.Properties.GetFloat(PropertyName.Attribute, (int)AttributeType.None);

		/// <summary>
		/// Returns the character's armor material.
		/// </summary>
		public ArmorMaterialType ArmorMaterial => (ArmorMaterialType)(int)this.Properties.GetFloat(PropertyName.ArmorMaterial, (int)ArmorMaterialType.None);

		/// <summary>
		/// Returns the character's mode of movement.
		/// </summary>
		public MoveType MoveType { get; set; } = MoveType.Normal;

		/// <summary>
		/// Gets or sets the character's tendency
		/// </summary>
		public TendencyType Tendency { get; set; } = TendencyType.Peaceful;

		/// <summary>
		/// The map the character is in.
		/// </summary>
		public int MapId { get; set; }

		/// <summary>
		/// Returns the character's game permission level, based on the account's authority.
		/// </summary>
		public PermissionLevel PermissionLevel => this.Connection?.Account?.PermissionLevel ?? PermissionLevel.User;

		/// <summary>
		/// Returns the character's party id.
		/// </summary>
		public long PartyId { get; set; }

		/// <summary>
		/// Returns the character's guild id.
		/// </summary>
		public long GuildId { get; set; }

		/// <summary>
		/// Character's class change reset points
		/// </summary>
		public int ResetPoints { get; set; }
		#endregion

		#region Components & Managers
		/// <summary>
		/// Returns a reference to the character's job list.
		/// </summary>
		public JobComponent Jobs { get; protected set; }

		/// <summary>
		/// The character's inventory.
		/// </summary>
		public InventoryComponent Inventory { get; protected set; }

		/// <summary>
		/// Character's skills.
		/// </summary>
		public SkillComponent Skills { get; protected set; }

		/// <summary>
		/// Character's abilities.
		/// </summary>
		public AbilityComponent Abilities { get; protected set; }

		/// <summary>
		/// Character's buffs.
		/// </summary>
		public BuffComponent Buffs { get; protected set; }

		/// <summary>
		/// Returns the character's quests manager.
		/// </summary>
		public QuestComponent Quests { get; protected set; }

		/// <summary>
		/// Returns the character's collection manager.
		/// </summary>
		public CollectionComponent Collections { get; protected set; }

		/// <summary>
		/// Returns the character's item set manager.
		/// </summary>
		public ItemSetComponent ItemSets { get; protected set; }

		/// <summary>
		/// Returns the character's time actions.
		/// </summary>
		public TimeActionComponent TimeActions { get; protected set; }

		/// <summary>
		/// Character's timed events.
		/// </summary>
		public TimedEventComponent TimedEvents { get; protected set; }

		/// <summary>
		/// Character's track manager.
		/// </summary>
		public TrackComponent Tracks { get; protected set; }

		/// <summary>
		/// Character's achievement manager.
		/// </summary>
		public AchievementComponent Achievements { get; protected set; }

		/// <summary>
		/// The character's equipped achievement title id, or -1 if none.
		/// </summary>
		public int EquippedTitleId { get; set; } = -1;

		/// <summary>
		/// Character's adventure book manager.
		/// </summary>
		public AdventureBookComponent AdventureBook { get; protected set; }

		/// <summary>
		/// Character's attendance events/rewards manager.
		/// </summary>
		public AttendanceComponent Attendance { get; protected set; }

		/// <summary>
		/// Returns the character's companion component.
		/// </summary>
		public CompanionComponent Companions { get; protected set; }

		/// <summary>
		/// Character's dungeon manager.
		/// </summary>
		public DungeonComponent Dungeon { get; protected set; }

		/// <summary>
		/// Gets the component that manages the effects applied to the entity.
		/// </summary>
		/// <remarks>The <see cref="EffectsComponent"/> provides functionality for managing and querying the active
		/// effects on the entity. This property is read-only outside the class and can only be modified within the class or
		/// its derived types.</remarks>
		public EffectsComponent Effects { get; protected set; }

		/// <summary>
		/// Returns the character's movement component.
		/// </summary>
		public MovementComponent Movement { get; protected set; }

		/// <summary>
		/// Returns the character's summoned monsters.
		/// </summary>
		public SummonComponent Summons { get; protected set; }

		/// <summary>
		/// Returns the character's tutorials viewed.
		/// </summary>
		public TutorialComponent Tutorials { get; protected set; }

		/// <summary>
		/// Character's session objects.
		/// </summary>
		public SessionObjects SessionObjects { get; } = new SessionObjects();

		/// <summary>
		/// Character's scripting variables.
		/// </summary>
		public VariablesContainer Variables { get; } = new VariablesContainer();
		#endregion

		#region Storage Properties
		/// <summary>
		/// Returns the character's personal storage.
		/// </summary>
		public PersonalStorage PersonalStorage { get; }

		/// <summary>
		/// Returns the character's team storage.
		/// </summary>
		public TeamStorage TeamStorage => this.Connection.Account.TeamStorage;

		/// <summary>
		/// Returns a reference to the character's current storage.
		/// </summary>
		public Storages.Storage CurrentStorage
		{
			get => this.Variables.Temp.Get<Storages.Storage>("Melia.Storage") ?? this.PersonalStorage;
			set => this.Variables.Temp.Set("Melia.Storage", value);
		}
		#endregion

		#region Properties Objects
		/// <summary>
		/// Character's properties.
		/// </summary>
		public CharacterProperties Properties { get; protected set; }

		/// <summary>
		/// GuildMember Properties
		/// </summary>
		public Properties GuildMemberProperties { get; } = new Properties("GuildMember");

		/// <summary>
		/// Returns a reference to the character's properties.
		/// </summary>
		Properties IPropertyHolder.Properties => this.Properties;

		/// <summary>
		/// Returns the character's PCEtc properties.
		/// </summary>
		public PCEtc Etc { get; protected set; }

		/// <summary>
		/// Gets or sets the player's localizer.
		/// </summary>
		public Localizer Localizer
		{
			get => _localizer ?? ZoneServer.Instance.MultiLocalization.GetDefault();
			private set => _localizer = value;
		}
		#endregion

		#region Events
		/// <summary>
		/// Raised when the characters sits down or stands up.
		/// </summary>
		public event Action<Character> SitStatusChanged;

		/// <summary>
		/// Raised when the characters stats change.
		/// </summary>
		public event Action<Character> StatChanged;

		/// <summary>
		/// Raised when the character dies.
		/// </summary>
		public Action<Character, ICombatEntity> Died { get; set; }

		/// <summary>
		/// Raised when the character takes damage.
		/// </summary>
		public Action<Character, float, ICombatEntity> Damaged { get; set; }

		/// <summary>
		/// Manages the character's HP/SP recovery ticks.
		/// </summary>
		public RecoveryComponent Recovery { get; protected set; }

		/// <summary>
		/// Manages the character's combat state (targeting, attack state, etc.).
		/// </summary>
		public CombatComponent Combat { get; protected set; }

		/// <summary>
		/// Manages the character's skill cooldowns.
		/// </summary>
		public CooldownComponent Cooldowns { get; protected set; }

		/// <summary>
		/// Manages the character's state locks (frozen, knocked down, etc.).
		/// </summary>
		public StateLockComponent States { get; protected set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Creates new character.
		/// </summary>
		public Character() : base()
		{
			this.Faction = FactionType.Law;
			this.InitializeComponents();
			this.Properties = new CharacterProperties(this);
			this.Etc = new PCEtc(this);
			this.PersonalStorage = new PersonalStorage(this);
			this.AddSessionObjects();
		}

		/// <summary>
		/// Used to create a dummy character (Dummy PC).
		/// </summary>
		/// <param name="copyFrom"></param>
		public Character(Character copyFrom)
		{
			this.CopyFromCharacter(copyFrom);
			this.Components.Add(new CombatComponent(this));
		}

		private void InitializeComponents()
		{
			this.Components.Add(this.Inventory = new InventoryComponent(this));
			this.Components.Add(this.Jobs = new JobComponent(this));
			this.Components.Add(this.Skills = new SkillComponent(this));
			this.Components.Add(this.Abilities = new AbilityComponent(this));
			this.Components.Add(this.Buffs = new BuffComponent(this));
			this.Components.Add(this.Recovery = new RecoveryComponent(this));
			this.Components.Add(this.Combat = new CombatComponent(this));
			this.Components.Add(this.Cooldowns = new CooldownComponent(this));
			this.Components.Add(this.TimeActions = new TimeActionComponent(this));
			this.Components.Add(this.States = new StateLockComponent(this));
			this.Components.Add(this.Quests = new QuestComponent(this));
			this.Components.Add(this.Collections = new CollectionComponent(this));
			this.Components.Add(this.ItemSets = new ItemSetComponent(this));
			this.Components.Add(this.Movement = new MovementComponent(this));
			this.Components.Add(new AttachmentComponent(this));
			this.Components.Add(this.TimedEvents = new TimedEventComponent(this));
			this.Components.Add(this.Tracks = new TrackComponent(this));
			this.Components.Add(this.Achievements = new AchievementComponent(this));
			this.Components.Add(this.AdventureBook = new AdventureBookComponent(this));
			this.Components.Add(this.Attendance = new AttendanceComponent(this));
			this.Components.Add(this.Companions = new CompanionComponent(this));
			this.Components.Add(this.Summons = new SummonComponent(this));
			this.Components.Add(this.Tutorials = new TutorialComponent(this));
			this.Components.Add(this.Dungeon = new DungeonComponent(this));
			this.Components.Add(this.Effects = new EffectsComponent(this));
		}

		private void CopyFromCharacter(Character copyFrom)
		{
			this.DbId = copyFrom.DbId;
			this.AccountDbId = copyFrom.AccountDbId;
			this.Name = copyFrom.Name;
			this.TeamName = copyFrom.TeamName;
			this.Stance = copyFrom.Stance;
			this.Gender = copyFrom.Gender;
			this.Exp = copyFrom.Exp;
			this.JobId = copyFrom.JobId;
			this.Hair = copyFrom.Hair;
			this.SkinColor = copyFrom.SkinColor;
			this.SetFaction(copyFrom.Faction);
			this.Properties = copyFrom.Properties;
			this.Jobs = copyFrom.Jobs;
			this.Inventory = copyFrom.Inventory;
			this.VisibleEquip = copyFrom.VisibleEquip;

			SetPosition(copyFrom.Position);
			SetDirection(copyFrom.Direction);
			this.Pose = copyFrom.Pose;
		}
		#endregion

		#region Core Update Method
		/// <summary>
		/// Updates character and its components.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.Components.Update(elapsed);
			this.UpdateResurrection(elapsed);
			this.UpdatePendingCompanion();
		}

		/// <summary>
		/// Activates pending companion after scripts have had time to set layer.
		/// </summary>
		private void UpdatePendingCompanion()
		{
			if (_pendingCompanionActivation == DateTime.MinValue)
				return;

			if (DateTime.Now < _pendingCompanionActivation)
				return;

			_pendingCompanionActivation = DateTime.MinValue;

			if (!this.HasCompanions)
				return;

			foreach (var companion in this.Companions.GetList())
			{
				if (companion.IsActivated && companion.Map != this.Map)
					companion.SetCompanionState(true);
			}
		}

		/// <summary>
		/// Schedules companion activation after a delay.
		/// </summary>
		public void ScheduleCompanionActivation()
		{
			_pendingCompanionActivation = DateTime.Now + CompanionActivationDelay;
		}

		/// <summary>
		/// Sends the resurrection dialog as necessary.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateResurrection(TimeSpan elapsed)
		{
			if ((this.IsDead) && ((!this.Map.IsPVP) || Feature.IsEnabled("AllowPVPResurrection")))
			{
				_resurrectDialogTimer -= elapsed;
				if (_resurrectDialogTimer <= TimeSpan.Zero)
				{
					ResurrectOptions options = 0;

					if (ZoneServer.Instance.Conf.World.ResurrectCityOption)
					{
						options |= ResurrectOptions.NearestCity;
					}
					if (ZoneServer.Instance.Conf.World.ResurrectRevivalPointOption)
					{
						options |= ResurrectOptions.NearestRevivalPoint;
					}
					if (ZoneServer.Instance.Conf.World.ResurrectRevivalPointOption)
					{
						if (this.HasItem("RestartCristal"))
						{
							options |= ResurrectOptions.SoulCrystal;
						}
					}

					Send.ZC_RESURRECT_DIALOG(this, options);
					_resurrectDialogTimer = ResurrectDialogDelay;
				}
			}
		}
		#endregion

		#region Session Objects Management
		/// <summary>
		/// Adds default session objects.
		/// </summary>
		private void AddSessionObjects()
		{
			this.SessionObjects.Add(new SessionObject(SessionObjectId.Drop));
			this.SessionObjects.Add(new SessionObject(SessionObjectId.MapEventReward));
			this.SessionObjects.Add(new SessionObject(SessionObjectId.SmartGen));
			this.SessionObjects.Add(new SessionObject(SessionObjectId.Raid));
			this.SessionObjects.Add(new SessionObject(SessionObjectId.NpcDialogCount));
			this.SessionObjects.Add(new SessionObject(SessionObjectId.Request));
			this.SessionObjects.Add(new SessionObject(SessionObjectId.Shop));
			this.SessionObjects.Add(new SessionObject(SessionObjectId.ExpCardUse));

			var sessionObject = new SessionObject(SessionObjectId.Main);
			sessionObject.Properties.SetString(PropertyName.QSTARTZONETYPE, "StartLine1");
			this.SessionObjects.Add(sessionObject);
		}

		/// <summary>
		/// Sends a message to the character and closes the connection
		/// after a short delay.
		/// </summary>
		public void Disconnect(string message)
		{
			this.MsgBox(message);
			this.Connection?.Close(ZoneServer.Instance.Conf.World.ConnectionCloseDelay);
		}

		/// <summary>
		/// Clones the character within it's same appearance and
		/// spawns it on the current map at a given position.
		/// </summary>
		/// <param name="position"></param>
		public Character Clone(Position position)
		{
			var dummyCharacter = new DummyCharacter();

			dummyCharacter.Owner = this;
			dummyCharacter.Name = this.Name;
			dummyCharacter.TeamName = this.TeamName;
			dummyCharacter.JobId = this.JobId;
			dummyCharacter.Gender = this.Gender;
			dummyCharacter.Hair = this.Hair;
			dummyCharacter.SkinColor = this.SkinColor;
			dummyCharacter.MapId = this.MapId;

			dummyCharacter.Position = position;
			dummyCharacter.Direction = this.Direction;

			foreach (var item in this.Inventory.GetEquip())
			{
				var newItem = new Item(item.Value.Id, item.Value.Amount);
				dummyCharacter.Inventory.SetEquipSilent(item.Key, newItem);
			}

			foreach (var job in this.Jobs.GetList())
			{
				dummyCharacter.Jobs.AddSilent(new Job(dummyCharacter, job.Id));
			}

			foreach (var skill in this.Skills.GetList())
			{
				var newSkill = new Skill(dummyCharacter, skill.Id, skill.Level);
				dummyCharacter.Skills.AddSilent(newSkill);
			}

			dummyCharacter.InitProperties();
			dummyCharacter.Properties.Stamina = (int)this.Properties.GetFloat(PropertyName.MaxSta);
			dummyCharacter.UpdateStance();
			dummyCharacter.ModifyHpSafe(this.MaxHp, out var hp, out var priority);

			this.Map.AddCharacter(dummyCharacter);

			Send.ZC_ENTER_PC(this.Connection, dummyCharacter);
			Send.ZC_OWNER(this, dummyCharacter);
			Send.ZC_UPDATED_PCAPPEARANCE(dummyCharacter);

			Send.ZC_NORMAL.HeadgearVisibilityUpdate(dummyCharacter);

			return dummyCharacter;
		}

		private static readonly BuffId[] OobeBuffIds = new[]
		{
			BuffId.OOBE_Soulmaster_Buff,
			BuffId.OOBE_Prakriti_Buff,
			BuffId.OOBE_Anila_Buff,
			BuffId.OOBE_Possession_Buff,
			BuffId.OOBE_Patati_Buff,
			BuffId.OOBE_Moksha_Buff,
			BuffId.OOBE_Tanoti_Buff,
			BuffId.OOBE_Strong_Buff,
			BuffId.OOBE_Stack_Buff
		};

		/// <summary>
		/// Return true in case of the character has used Out Of Body Skill
		/// </summary>
		/// <returns></returns>
		public bool IsOutOfBody()
		{
			foreach (var buffId in OobeBuffIds)
			{
				if (this.IsBuffActive(buffId))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Cancels any active Out Of Body Experience buffs, forcing
		/// the character to return to their body and cleaning up the
		/// dummy character.
		/// </summary>
		public void CancelOutOfBody()
		{
			foreach (var buffId in OobeBuffIds)
				this.StopBuff(buffId);
		}
		#endregion

		// Additional partial class files will contain:
		// - CharacterStats.cs (level, exp, stats management)
		// - CharacterCombat.cs (combat, damage, healing)
		// - CharacterMovement.cs (position, warping, visibility)
		// - CharacterItems.cs (inventory management)
		// - CharacterSocial.cs (party, guild, messaging)
		// - CharacterJobSkills.cs (job and skill management)
		// - CharacterDialog.cs (NPC interactions)
		// - CharacterUtilities.cs (helper methods)
	}
}
