// ===================================================================
// CharacterUtilities.cs - Miscellaneous helper methods and properties
// ===================================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		#region Property Management
		/// <summary>
		/// Sets a character property and sends it to the client.
		/// </summary>
		public void SetProperty(string propertyName, float value)
		{
			this.Properties.SetFloat(propertyName, value);
			Send.ZC_OBJECT_PROPERTY(this, propertyName);
			if (propertyName == PropertyName.CastingSpeed)
				this.StatChanged?.Invoke(this);
		}

		/// <summary>
		/// Sets a property on any IPropertyObject and updates the client.
		/// </summary>
		public void SetProperty(IPropertyObject propertyObject, string propName, float propValue)
		{
			propertyObject.Properties.SetFloat(propName, propValue);
			Send.ZC_OBJECT_PROPERTY(this, propertyObject, propName);
		}

		/// <summary>
		/// Modifies a character property and sends it to the client.
		/// </summary>
		public void ModifyProperty(string propertyName, float value)
		{
			this.Properties.Modify(propertyName, value);
			Send.ZC_OBJECT_PROPERTY(this, propertyName);
			if (propertyName == PropertyName.CastingSpeed)
				this.StatChanged?.Invoke(this);
		}

		/// <summary>
		/// Sets an account property and updates the client.
		/// </summary>
		public void SetAccountProperty(string propertyName, int value)
		{
			var properties = this.Connection.Account.Properties;
			properties.SetFloat(propertyName, value);
			Send.ZC_NORMAL.AccountProperties(this, propertyName);
		}

		/// <summary>
		/// Modifies an account property and updates the client.
		/// </summary>
		public bool ModifyAccountProperty(string propertyName, float modifier)
		{
			var properties = this.Connection.Account.Properties;
			if (!PropertyTable.Exists(properties.Namespace, propertyName))
				return false;

			switch (propertyName)
			{
				case PropertyName.PremiumMedal: this.Connection.Account.PremiumMedals += (int)modifier; break;
				case PropertyName.GiftMedal: this.Connection.Account.GiftMedals += (int)modifier; break;
				case PropertyName.Medal: this.Connection.Account.Medals += (int)modifier; break;
				default: properties.Modify(propertyName, modifier); break;
			}

			Send.ZC_OBJECT_PROPERTY(this.Connection, this.Connection.Account, propertyName);
			Send.ZC_NORMAL.AccountProperties(this, propertyName);
			Send.ZC_PC_PROP_UPDATE(this, (short)PropertyTable.GetId("Account", propertyName), 1);
			return true;
		}

		/// <summary>
		/// Sets an ETC property and updates the client.
		/// </summary>
		public void SetEtcProperty(string propertyName, int value)
		{
			if (!PropertyTable.Exists(this.Etc.Properties.Namespace, propertyName))
				return;

			this.Etc.Properties.SetFloat(propertyName, value);
			Send.ZC_OBJECT_PROPERTY(this, this.SocialUserId, this.Etc.Properties.GetSelect(propertyName));
		}

		/// <summary>
		/// Sets an ETC property and updates the client.
		/// </summary>
		public void SetEtcProperty(string propertyName, string value)
		{
			if (!PropertyTable.Exists(this.Etc.Properties.Namespace, propertyName))
				return;

			this.Etc.Properties.SetString(propertyName, value);
			Send.ZC_OBJECT_PROPERTY(this, this.SocialUserId, this.Etc.Properties.GetSelect(propertyName));
		}

		/// <summary>
		/// Modifies an ETC property and updates the client.
		/// </summary>
		public void ModifyEtcProperty(string propertyName, int amount)
		{
			if (!PropertyTable.Exists(this.Etc.Properties.Namespace, propertyName))
				return;

			this.Etc.Properties.Modify(propertyName, amount);
			Send.ZC_OBJECT_PROPERTY(this, this.SocialUserId, this.Etc.Properties.GetSelect(propertyName));
		}

		/// <summary>
		/// Modifies a session property and updates the client.
		/// </summary>
		public bool ModifySessionProperty(string propertyName, float modifier)
		{
			var properties = this.SessionObjects.Main.Properties;
			if (!PropertyTable.Exists("Session", propertyName))
				return false;

			properties.Modify(propertyName, modifier);
			Send.ZC_OBJECT_PROPERTY(this.Connection, this.SessionObjects.Main, propertyName);
			Send.ZC_PC_PROP_UPDATE(this, (short)PropertyTable.GetId("Session", propertyName), 1);
			return true;
		}

		/// <summary>
		/// Invalidates specified properties and updates the client.
		/// </summary>
		public void InvalidateProperties(params string[] propertyNames)
		{
			this.Properties.Invalidate(propertyNames);
			Send.ZC_OBJECT_PROPERTY(this, propertyNames);

			if (propertyNames.Contains(PropertyName.MSPD))
				Send.ZC_MOVE_SPEED(this);
		}

		/// <summary>
		/// Sends client updates after invalidating all properties.
		/// </summary>
		public void InvalidateProperties()
		{
			this.Properties.InvalidateAll();
			Send.ZC_OBJECT_PROPERTY(this);
			Send.ZC_MOVE_SPEED(this);
			Send.ZC_CASTING_SPEED(this);
			Send.ZC_UPDATE_ALL_STATUS(this, this.HpChangeCounter);
			Send.ZC_UPDATE_SKL_SPDRATE_LIST(this, this.Skills.GetList());
		}

		/// <summary>
		/// Invalidates properties for a given object and updates the client.
		/// </summary>
		public void InvalidateProperties(IPropertyObject obj, params string[] propertyNames)
		{
			if (obj is Item item && item.HasSockets)
				Send.ZC_EQUIP_GEM_INFO(this, item);

			if (propertyNames == null || propertyNames.Length == 0)
			{
				obj.Properties.InvalidateAll();
				Send.ZC_OBJECT_PROPERTY(this, obj);
			}
			else
			{
				obj.Properties.Invalidate(propertyNames);
				Send.ZC_OBJECT_PROPERTY(this, obj, propertyNames);
			}
		}
		#endregion

		#region Session Object Management
		/// <summary>
		/// Adds a Session Object and updates the client.
		/// </summary>
		public SessionObject AddSessionObject(string sessionObjectId)
		{
			var sessionObject = this.SessionObjects.GetOrCreate(sessionObjectId);
			Send.ZC_SESSION_OBJ_ADD(this, sessionObject);
			return sessionObject;
		}

		/// <summary>
		/// Adds a Session Object and updates the client.
		/// </summary>
		public SessionObject AddSessionObject(int sessionObjectId)
		{
			var sessionObject = this.SessionObjects.GetOrCreate(sessionObjectId);
			Send.ZC_SESSION_OBJ_ADD(this, sessionObject);
			return sessionObject;
		}

		/// <summary>
		/// Sets a session object with a given value.
		/// </summary>
		public SessionObject SetSessionObject(string sessionObjectId, int value = 1)
		{
			var sessionObject = this.SessionObjects.GetOrCreate(sessionObjectId);
			sessionObject.Properties.Modify(PropertyName.QuestInfoValue1, value);
			Send.ZC_SESSION_OBJ_ADD(this, sessionObject);
			return sessionObject;
		}

		/// <summary>
		/// Removes a Session Object and updates the client.
		/// </summary>
		public bool RemoveSessionObject(int sessionObjectId)
		{
			if (this.SessionObjects.Remove(sessionObjectId))
			{
				Send.ZC_SESSION_OBJ_REMOVE(this, sessionObjectId);
				return true;
			}
			return false;
		}
		#endregion

		#region Misc Helpers
		/// <summary>
		/// Adds points to the given achievement for this character.
		/// </summary>
		public void AddAchievementPoint(string achievementName, int value)
		{
			this.Achievements?.AddAchievementPoints(achievementName, value);
		}

		/// <summary>
		/// Equips an achievement title. Validates that the character has unlocked
		/// the achievement before equipping. Use -1 to unequip.
		/// </summary>
		/// <param name="achievementId">The achievement id to equip, or -1 to unequip.</param>
		/// <returns>True if the title was equipped successfully, false if validation failed.</returns>
		public bool EquipTitle(int achievementId)
		{
			// Unequip title
			if (achievementId == -1)
			{
				this.EquippedTitleId = -1;
				return true;
			}

			// Validate that character has unlocked the achievement
			if (this.Achievements != null && this.Achievements.HasAchievement(achievementId))
			{
				this.EquippedTitleId = achievementId;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used to setup a "new" instance of a map in the client.
		/// </summary>
		/// <param name="silent">If true, does not send the layer change packet to the client.</param>
		public int StartLayer(bool silent = false)
		{
			this.Layer = this.Map.GetNewLayer();
			if (!silent)
				Send.ZC_SET_LAYER(this, this.Layer, true);
			this.LookAround();
			return this.Layer;
		}

		/// <summary>
		/// Used to remove a "new" instance of a map in the client.
		/// </summary>
		public void StopLayer()
		{
			this.Layer = 0;
			Send.ZC_SET_LAYER(this, this.Layer, false);
		}

		/// <summary>
		/// Changes the character's hair and updates nearby clients.
		/// </summary>
		public void ChangeHair(int hairTypeIndex)
		{
			this.Hair = hairTypeIndex;
			Send.ZC_UPDATED_PCAPPEARANCE(this);
		}

		/// <summary>
		/// Executes a client-side script.
		/// </summary>
		public void ExecuteClientScript(string script, params object[] args)
		{
			Send.ZC_EXEC_CLIENT_SCP(this.Connection, args.Length > 0 ? string.Format(script, args) : script);
		}

		/// <summary>
		/// Shows a tutorial/help window.
		/// </summary>
		public void ShowHelp(string className, bool forceShow = false)
		{
			this.Tutorials.Show(className, forceShow);
		}

		/// <summary>
		/// Displays an item balloon notification.
		/// </summary>
		public void ShowItemBalloon(string style, string systemMsg, string v3, Item item, float duration, float delaySec, string type)
		{
			Task.Delay(TimeSpan.FromSeconds(delaySec)).ContinueWith(_ =>
			{
				Send.ZC_NORMAL.ShowItemBalloon(this, item, type, style, systemMsg, duration);
			});
		}

		/// <summary>
		/// Retrieves a stored companion property string for this character.
		/// Returns false and the default value if not set.
		/// </summary>
		public bool TryGetCompanionProperty(Companion companion, string propertyName, out string propertyValue, string defaultValue = null)
		{
			if (!this.Variables.Perm.TryGetString($"{companion.DbId}_{propertyName}", out propertyValue))
				propertyValue = defaultValue;
			return propertyValue != null;
		}

		/// <summary>
		/// Stores a string companion property for this character.
		/// </summary>
		public void SetCompanionProperty(Companion companion, string propertyName, string propertyValue)
		{
			this.Variables.Perm.SetString($"{companion.DbId}_{propertyName}", propertyValue);
		}

		/// <summary>
		/// Stores a float companion property as a string for this character.
		/// </summary>
		public void SetCompanionProperty(Companion companion, string propertyName, float propertyValue)
		{
			this.Variables.Perm.SetString($"{companion.DbId}_{propertyName}", propertyValue.ToString());
		}

		/// <summary>
		/// Returns the last position where the character opened a UI,
		/// including map name, world position, and frame name. Returns
		/// null if no position has been saved.
		/// </summary>
		public Tuple<string?, Position, string?> GetLastUIOpenPosition()
		{
			var lastUIOpenPos = this.Etc.Properties.GetString(PropertyName.LastUIOpenPos, "None");

			if (lastUIOpenPos == "None")
				return null;

			var splitUIPos = lastUIOpenPos.Split('/');
			string mapname = splitUIPos.Length > 0 ? splitUIPos[0] : null;
			float.TryParse(splitUIPos.Length > 1 ? splitUIPos[1] : "0", out float x);
			float.TryParse(splitUIPos.Length > 2 ? splitUIPos[2] : "0", out float y);
			float.TryParse(splitUIPos.Length > 3 ? splitUIPos[3] : "0", out float z);
			string uiname = splitUIPos.Length > 4 ? splitUIPos[4] : null;

			return Tuple.Create(mapname, new Position(x, y, z), uiname);
		}

		/// <summary>
		/// Saves the character's current position and the given UI frame
		/// name as the last UI open position.
		/// </summary>
		public void SaveLastUIOpenPosition(string frameName)
		{
			this.SetEtcProperty(PropertyName.LastUIOpenPos, $"{this.Map?.Data?.ClassName ?? "Unknown"}/{this.Position.X}/{this.Position.Y}/{this.Position.Z}/{frameName}");
		}
		#endregion
	}
}
