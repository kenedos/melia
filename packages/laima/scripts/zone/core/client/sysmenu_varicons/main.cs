using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

public class SysMenuVarIconsScript : ClientScript
{
	// This seems to be inttermittently crashing the client sometimes.
	// Racing condition on order of packets the client receives, perhaps?
	// We may need to find the correct way to dynamically add those system menus
	// through Melia's sysmenu overlay system - or disable that system and
	// only work directly on the client.
	// protected override void Load()
	// {
	// }
	// 
	// [On("PlayerReady")]
	// protected void OnPlayerReady(object sender, PlayerEventArgs e)
	// {
	// 	var character = e.Character;
	// 
	// 	if (character.Jobs.Has(JobId.Wugushi))
	// 	{
	// 		this.SendRawLuaScript(character, @"
	// 			Melia.Ui.SysMenu.AddButton(""BtnPoisonPot"", ""sysmenu_wugushi"", ""Poison Pot"", ""ui.ToggleFrame('poisonpot')"")
	// 		");
	// 
	// 		var bossCardId = (int)character.Etc.Properties.GetFloat(PropertyName.Wugushi_bosscard);
	// 		if (bossCardId > 0)
	// 			character.SetTempVar(PropertyName.Wugushi_bosscard, bossCardId);
	// 	}
	// 
	// 	if (character.Jobs.Has(JobId.Sorcerer))
	// 	{
	// 		this.SendRawLuaScript(character, @"
	// 			Melia.Ui.SysMenu.AddButton(""BtnGrimoire"", ""sysmenu_neacro"", ""Grimoire"", ""ui.ToggleFrame('grimoire')"")
	// 		");
	// 	}
	// }
}
