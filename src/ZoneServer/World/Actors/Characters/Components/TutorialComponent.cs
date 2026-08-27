using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	public class TutorialComponent : CharacterComponent
	{
		private static readonly TimeSpan ShowCooldown = TimeSpan.FromSeconds(3);

		private const float LowDurabilityRatio = 0.3f;
		private const int RootCrystalMinId = 45110;
		private const int RootCrystalMaxId = 45137;

		private const string VisitedCityVarName = "Melia.Tutorial.VisitedCity";

		private static readonly JobId[] BaseJobIds = [JobId.Swordsman, JobId.Wizard, JobId.Archer, JobId.Cleric, JobId.Scout];

		private static readonly Dictionary<JobId, string> JobTutorials = new()
		{
			[JobId.Sorcerer] = "TUTO_CLASS_SOCERER",
			[JobId.Necromancer] = "TUTO_CLASS_NECROMANCER",
			[JobId.Wugushi] = "TUTO_CLASS_WUGUSHI",
			[JobId.Alchemist] = "TUTO_CLASS_ALCHEMIST",
		};

		/// <summary>
		/// A dictionary with help shown
		/// </summary>
		private readonly Dictionary<int, bool> _help = new();

		private DateTime _lastShowTime = DateTime.MinValue;

		public int Count
		{
			get
			{
				lock (_help)
					return _help.Count;
			}
		}

		public TutorialComponent(Character character) : base(character)
		{
			if (this.Character.Inventory != null)
				this.Character.Inventory.Equipped += this.OnEquipped;
		}

		private void OnEquipped(Character character, Item item)
			=> this.CheckLowDurability(item);

		/// <summary>
		/// Shows the repair tutorial if the item's durability is inside
		/// the client's low durability warning range.
		/// </summary>
		/// <param name="item"></param>
		public void CheckLowDurability(Item item)
		{
			if (item.MaxDurability <= 0 || item.Durability <= 0)
				return;

			if (item.Durability >= item.MaxDurability * LowDurabilityRatio)
				return;

			this.Show("TUTO_NPC_REPAIR");
		}

		/// <summary>
		/// Shows the tutorial matching the killed monster, root crystals
		/// counting separately from regular monsters.
		/// </summary>
		/// <param name="mob"></param>
		public void CheckMonsterKill(Mob mob)
		{
			if (mob.Faction == FactionType.RootCrystal)
			{
				if (mob.Id >= RootCrystalMinId && mob.Id <= RootCrystalMaxId)
					this.Show("TUTO_ROOTCRYSTAL");

				return;
			}

			this.Show("TUTO_C_MONSTER");
		}

		/// <summary>
		/// Shows the job change tutorial once a base class reaches the
		/// level where it can advance.
		/// </summary>
		public void CheckJobChangeAvailable()
		{
			var job = this.Character.Job;

			if (job == null || !BaseJobIds.Contains(job.Id))
				return;

			if (job.Level < ZoneServer.Instance.Conf.World.MaxBaseJobLevel)
				return;

			this.Show("TUTO_CHANGE_JOB");
		}

		/// <summary>
		/// Shows the attack tutorial the first time the character loads
		/// into a map outside a city.
		/// </summary>
		public void CheckLeftCity()
		{
			var map = this.Character.Map;

			if (map == null)
				return;

			if (map.IsCity)
			{
				this.Character.Variables.Perm.SetBool(VisitedCityVarName, true);
				return;
			}

			if (!this.Character.Variables.Perm.GetBool(VisitedCityVarName, false))
				return;

			this.Show("TUTO_ATTACK_KB");
		}

		/// <summary>
		/// Shows the tutorial associated with the picked up item's
		/// group, if there is one.
		/// </summary>
		/// <param name="item"></param>
		public void CheckItemPickup(Item item)
		{
			if (item.NeedsAppraisal)
				this.Show("TUTO_ITEM_APPRAISE");

			switch (item.Data.Group)
			{
				case ItemGroup.Gem:
					this.Show("TUTO_GEM_EQUIP");
					break;

				case ItemGroup.Recipe:
					this.Show("TUTO_CRAFT");
					break;

				case ItemGroup.Card:
					this.Show("TUTO_MONSTER_CARD");
					break;
			}
		}

		/// <summary>
		/// Shows the class tutorial for the job the character advanced
		/// to, if there is one.
		/// </summary>
		/// <param name="jobId"></param>
		public void CheckJobAdvancement(JobId jobId)
		{
			if (!JobTutorials.TryGetValue(jobId, out var className))
				return;

			this.Show(className);
		}

		public void Add(int helpId, bool value)
		{
			lock (_help)
				_help.TryAdd(helpId, value);
		}

		/// <summary>
		/// Removes all tutorials the character has seen, so they show
		/// up again.
		/// </summary>
		public void Reset()
		{
			lock (_help)
				_help.Clear();
		}

		public void Show(string className, bool forceShow = false)
		{
			var help = ZoneServer.Instance.Data.HelpDb.Find(className);

			if (help == null)
			{
				Log.Warning("ShowHelp: Unable to find help by class name {0}.", className);
				return;
			}

			lock (_help)
			{
				if (!forceShow && this._help.TryGetValue(help.Id, out var isHelpShown) && isHelpShown)
					return;

				// Tutorials triggered right after another one are dropped
				// instead of stacking, to be shown on a later trigger.
				if (!forceShow && DateTime.Now < _lastShowTime + ShowCooldown)
					return;

				_lastShowTime = DateTime.Now;
				this._help[help.Id] = true;

				// Custom Tutorials on Help Calls
				switch (className)
				{
					case "TUTO_MOVE_KB":
						this.Character.AddonMessage(AddonMessage.KEYBOARD_TUTORIAL);
						break;
					default:
						Send.ZC_HELP_ADD(this.Character, help.Id, true);
						break;
				}

				if (help.DbSave)
					ZoneServer.Instance.Database.SaveHelp(this.Character.AccountDbId, help.Id, true);
			}
		}
	}
}
