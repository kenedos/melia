using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;

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
		}

		if (character.Jobs.Has(JobId.Sorcerer))
		{
			this.SendRawLuaScript(character, @"
				Melia.Ui.SysMenu.AddButton(""BtnGrimoire"", ""sysmenu_neacro"", ""Grimoire"", ""ui.ToggleFrame('grimoire')"")
			");
		}
	}
}
