using Melia.Shared.Data.Database;

namespace Melia.Shared.Data
{
	/// <summary>
	/// Wrapper for all file databases.
	/// </summary>
	public class MeliaData
	{
		public AbilityDb AbilityDb { get; } = new AbilityDb();
		public AbilityTreeDb AbilityTreeDb { get; } = new AbilityTreeDb();
		public AccountOptionDb AccountOptionDb { get; } = new AccountOptionDb();
		public AchievementDb AchievementDb { get; } = new AchievementDb();
		public AchievementPointDb AchievementPointDb { get; } = new AchievementPointDb();
		public BarrackDb BarrackDb { get; } = new BarrackDb();
		public BuffDb BuffDb { get; } = new BuffDb();

		public CabinetDb CabinetDb { get; } = new CabinetDb();
		public ChatEmoticonDb ChatEmoticonDb { get; } = new ChatEmoticonDb();
		public ChatMacroDb ChatMacroDb { get; } = new ChatMacroDb();
		public CollectionDb CollectionDb { get; }
		public CompanionDb CompanionDb { get; } = new CompanionDb();
		public CompanionShopDb CompanionShopDb { get; } = new CompanionShopDb();
		public CooldownDb CooldownDb { get; } = new CooldownDb();
		public CubeGachaDb CubeGachaDb { get; } = new CubeGachaDb();
		public CustomCommandDb CustomCommandDb { get; } = new CustomCommandDb();
		public DialogDb DialogDb { get; } = new DialogDb();
		public DialogTxDb DialogTxDb { get; } = new DialogTxDb();
		public EquipCardDb EquipCardDb { get; } = new EquipCardDb();
		public EventAttendanceDb EventAttendanceDb { get; } = new EventAttendanceDb();
		public ExpDb ExpDb { get; } = new ExpDb();
		public FactionDb FactionDb { get; } = new FactionDb();
		public FeatureDb FeatureDb { get; } = new FeatureDb();
		public FurnitureDb FurnitureDb { get; } = new FurnitureDb();
		public GlobalDropDb GlobalDropDb { get; }
		public TreasureSpawnPointDb TreasureSpawnPointDb { get; } = new TreasureSpawnPointDb();
		public TreasureDropDb TreasureDropDb { get; }
		public MinigameSpawnPointDb MinigameSpawnPointDb { get; } = new MinigameSpawnPointDb();
		public GroundDb GroundDb { get; } = new GroundDb();
		public HairTypeDb HairTypeDb { get; } = new HairTypeDb();
		public HeadTypeDb HeadTypeDb { get; } = new HeadTypeDb();
		public HelpDb HelpDb { get; } = new HelpDb();
		public InstanceDungeonDb InstanceDungeonDb { get; }
		public InvBaseIdDb InvBaseIdDb { get; } = new InvBaseIdDb();
		public ItemDb ItemDb { get; } = new ItemDb();
		public ItemIconDb ItemIconDb { get; } = new ItemIconDb();
		public ItemExpDb ItemExpDb { get; } = new ItemExpDb();
		public ItemGradeDb ItemGradeDb { get; } = new ItemGradeDb();
		public ItemMonsterDb ItemMonsterDb { get; } = new ItemMonsterDb();
		public ItemSetDb ItemSetDb { get; }
		public ItemSummonBossDb ItemSummonBossDb { get; } = new ItemSummonBossDb();
		public JobDb JobDb { get; } = new JobDb();
		public MapDb MapDb { get; } = new MapDb();
		public MapBonusDropsDb MapBonusDropsDb { get; }
		public MonsterDb MonsterDb { get; } = new MonsterDb();

		public MonsterIconDb MonsterIconDb { get; } = new MonsterIconDb();
		public NavGraphDb NavGraphDb { get; } = new NavGraphDb();
		public NormalTxDb NormalTxDb { get; } = new NormalTxDb();
		public PacketStringDb PacketStringDb { get; } = new PacketStringDb();
		public PropertiesDb PropertiesDb { get; } = new PropertiesDb();
		public QuestDb QuestDb { get; } = new QuestDb();
		public RecipeDb RecipeDb { get; }
		public RedemptionSetDb RedemptionDb { get; }
		public ResurrectionPointDb ResurrectionPointDb { get; } = new ResurrectionPointDb();
		public RewardAttendanceDb RewardAttendanceDb { get; } = new RewardAttendanceDb();
		public SelectItemDb SelectItemDb { get; } = new SelectItemDb();
		public ServerDb ServerDb { get; }
		public SessionObjectDb SessionObjectDb { get; } = new SessionObjectDb();
		public ShopDb ShopDb { get; } = new ShopDb();
		public SimonyDb SimonyDb { get; } = new SimonyDb();
		public SkillDb SkillDb { get; } = new SkillDb();

		public SkillTreeDb SkillTreeDb { get; } = new SkillTreeDb();
		public SkinToneDb SkinToneDb { get; } = new SkinToneDb();
		public SocketPriceDb SocketPriceDb { get; } = new SocketPriceDb();
		public StanceConditionDb StanceConditionDb { get; } = new StanceConditionDb();
		public SystemMessageDb SystemMessageDb { get; } = new SystemMessageDb();
		public TradeShopDb TradeShopDb { get; }
		public WarpDb WarpDb { get; } = new WarpDb();
		public OpDb OpDb { get; set; }

		public MeliaData()
		{
			// Not entirely happy with this design, but I want access to
			// the map list from the server db to determine which maps
			// the zone servers serve.
			this.ServerDb = new ServerDb(this.MapDb);
			this.TradeShopDb = new TradeShopDb(this.ItemDb);
			// Yup, still not entirely happy with this.
			this.CollectionDb = new CollectionDb(this.PropertiesDb, this.ItemDb);
			this.ItemSetDb = new ItemSetDb(this.ItemDb);
			this.RedemptionDb = new RedemptionSetDb(this.ItemDb);
			this.GlobalDropDb = new GlobalDropDb(this.ItemDb);
			this.MapBonusDropsDb = new MapBonusDropsDb(this.ItemDb);
			this.TreasureDropDb = new TreasureDropDb(this.ItemDb);
			this.RecipeDb = new RecipeDb(this.ItemDb);
			this.InstanceDungeonDb = new InstanceDungeonDb(this.MapDb, this.ItemDb, this.MonsterDb);
		}
	}
}
