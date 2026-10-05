using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

public class SysMenuVarIconsScript : ClientScript
{
	protected override void Load()
	{
	}

	[On("PlayerReady")]
	protected void OnPlayerReady(object sender, PlayerEventArgs e)
	{
		var character = e.Character;

		if (character.Jobs.Has(JobId.Wugushi))
		{
			this.SendRawLuaScript(character, @"
				Melia.Ui.SysMenu.AddButton(""BtnPoisonPot"", ""sysmenu_wugushi"", ""Poison Pot"", ""ui.ToggleFrame('poisonpot')"")
			");

			var bossCardId = (int)character.Etc.Properties.GetFloat(PropertyName.Wugushi_bosscard);

			if (bossCardId > 0)
				character.SetTempVar(PropertyName.Wugushi_bosscard, bossCardId);
		}

		if (character.Jobs.Has(JobId.Sorcerer))
		{
			this.SendRawLuaScript(character, @"
				Melia.Ui.SysMenu.AddButton(""BtnGrimoire"", ""sysmenu_neacro"", ""Grimoire"", ""ui.ToggleFrame('grimoire')"")
			");

			RefreshGrimoireGuids(character);
		}

		if (character.Jobs.Has(JobId.Necromancer))
		{
			this.SendRawLuaScript(character, @"
				Melia.Ui.SysMenu.AddButton(""BtnNecronomicon"", ""sysmenu_neacro"", ""Necronomicon"", ""ui.ToggleFrame('necronomicon')"")
			");

			RefreshNecronomiconGuids(character);
			Send.ZC_OBJECT_PROPERTY(character, character.SocialUserId, character.Etc.Properties.GetSelect(PropertyName.Necro_DeadPartsCnt));
			character.AddonMessage(AddonMessage.UPDATE_NECRONOMICON_UI);
		}
	}

	private static void RefreshGrimoireGuids(Character character)
	{
		var etc = character.Etc.Properties;

		for (var slot = 1; slot <= 2; slot++)
		{
			var cardProperty = slot == 1 ? PropertyName.Sorcerer_bosscard1 : PropertyName.Sorcerer_bosscard2;
			var guidProperty = slot == 1 ? PropertyName.Sorcerer_bosscardGUID1 : PropertyName.Sorcerer_bosscardGUID2;

			var cardClassId = (int)etc.GetFloat(cardProperty);

			if (cardClassId <= 0)
				continue;

			var card = character.Inventory.FindItem(item => item.Id == cardClassId && item.Data.Group == ItemGroup.Card);

			if (card == null)
				continue;

			character.SetEtcProperty(guidProperty, card.ObjectId.ToString());
		}
	}

	private static void RefreshNecronomiconGuids(Character character)
	{
		var etc = character.Etc.Properties;

		var cardProperties = new[]
		{
			PropertyName.Necro_bosscard1,
			PropertyName.Necro_bosscard2,
			PropertyName.Necro_bosscard3,
			PropertyName.Necro_bosscard4
		};

		var guidProperties = new[]
		{
			PropertyName.Necro_bosscardGUID1,
			PropertyName.Necro_bosscardGUID2,
			PropertyName.Necro_bosscardGUID3,
			PropertyName.Necro_bosscardGUID4
		};

		for (var index = 0; index < cardProperties.Length; index++)
		{
			var cardClassId = (int)etc.GetFloat(cardProperties[index]);

			if (cardClassId <= 0)
				continue;

			var card = character.Inventory.FindItem(item => item.Id == cardClassId && item.Data.Group == ItemGroup.Card);

			if (card == null)
				continue;

			character.SetEtcProperty(guidProperties[index], card.ObjectId.ToString());
		}
	}
}
