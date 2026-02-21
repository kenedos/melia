namespace Melia.Shared.Game.Const
{
	public class SessionObjectId
	{
		/// <summary>
		/// Session object used to keep track of help messages.
		/// </summary>
		/// <remarks>
		/// 770001 = Only entry in sessionobject_jansori.ies
		/// There's very little data in the table, but apparently
		/// "Jansori is scolding, nagging, and grumbling" in
		/// Korean? I guess that's a fitting name for the Navi-like
		/// feature that keeps telling me how to do stuff.
		/// </remarks>
		public const int Jansori = 770001;

		/// <summary>
		/// Session object for NPC dialogue counting (NPC 대사 카운팅)
		/// </summary>
		public const int NpcDialogCount = 3;

		/// <summary>
		/// Session object for missions, no longer used?
		/// ssn_mission
		/// </summary>
		public const int Mission = 10017;

		/// <summary>
		/// Session object for storing objects?
		/// </summary>
		public const int ObjectStorage = 15005;

		/// <summary>
		/// Session object for exp card usage?
		/// </summary>
		public const int ExpCardUse = 15008;

		/// <summary>
		/// Session object for set store sales count
		/// ssn_shop
		/// </summary>
		public const int Shop = 60000;

		/// <summary>
		/// Unknown?
		/// ssn_timesave
		/// </summary>
		public const int TimeSave = 60001;

		/// <summary>
		/// Unused?
		/// ssn_mapintro
		/// </summary>
		public const int MapDirection = 60002;

		/// <summary>
		/// Session object for Drop Management (드랍 관리)
		/// ssn_drop
		/// </summary>
		public const int Drop = 80000;

		/// <summary>
		/// Session object for SmartGen
		/// ssn_smartgen
		/// </summary>
		public const int SmartGen = 80001;

		/// <summary>
		/// Session object for SmartGen Monsters? Unused?
		/// ssn_smartgen_mon
		/// </summary>
		public const int SmartGenMon = 80004;

		/// <summary>
		/// Session object for SmartGen Hide Monsters? Unused?
		/// ssn_smartgen_hidemon
		/// </summary>
		public const int SmartGenHideMon = 80005;


		/// <summary>
		/// Session object for Klaipeda Manager (클라페다매니저)
		/// Used to track Quests/Warps/Tracks/Tutorials/Events, practically everything
		/// ssn_klapeda
		/// </summary>
		public const int Main = 90000;

		/// <summary>
		/// Session object for Zone Event Reward Properties (존 이벤트 보상 프로퍼티)
		/// SSN_MAPEVENTREWARD
		/// </summary>
		public const int MapEventReward = 95000;

		/// <summary>
		/// Session object for "commissioned retailer", might be bounties (의뢰소매니저)
		/// </summary>
		public const int Request = 96000;

		/// <summary>
		/// Session object for Hourly Missions (1시간별 미션)
		/// </summary>
		public const int Raid = 100000;

		/// <summary>
		/// Session object for timed actions
		/// </summary>
		public const int TemporaryStopEvent = 15509;
	}
}
