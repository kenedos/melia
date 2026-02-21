using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	public class TutorialComponent : CharacterComponent
	{
		/// <summary>
		/// A dictionary with help shown
		/// </summary>
		private readonly Dictionary<int, bool> _help = new();

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
		}

		public void Add(int helpId, bool value)
		{
			lock (_help)
				_help.TryAdd(helpId, value);
		}

		public void Show(string className, bool forceShow = false)
		{
			var help = ZoneServer.Instance.Data.HelpDb.Find(className);

			if (help == null)
			{
				Log.Warning("ShowHelp: Unable to find help by class name {0}.", className);
				return;
			}

			if (this._help.TryGetValue(help.Id, out var isHelpShown) && isHelpShown)
				return;

			lock (_help)
			{
				if (this._help.TryAdd(help.Id, true) || forceShow)
				{
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
						ZoneServer.Instance.Database.SaveHelp(this.Character.DbId, help.Id, true);
				}
			}
		}
	}
}
