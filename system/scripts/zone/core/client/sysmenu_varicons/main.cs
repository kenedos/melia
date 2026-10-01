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

			var card = character.Inventory.FindItem(a => a.Id == cardClassId && a.Data.Group == ItemGroup.Card);
			if (card == null)
				continue;

			character.SetEtcProperty(guidProperty, card.ObjectId.ToString());
		}
	}
}
