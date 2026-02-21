//--- Melia Script ----------------------------------------------------------
// Companion Cost
//--- Description -----------------------------------------------------------
// Modifies the client's companion cost function to return the server's
// configured value.
//---------------------------------------------------------------------------

using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;

public class CompanionCostClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadAllScripts();
	}

	[On("PlayerReady")]
	protected void OnPlayerReady(object sender, PlayerEventArgs e)
	{
		// TODO: Decide if using the conf is right approach for this.
		var velhiderPrice = 110000;
		if (ZoneServer.Instance.Data.CompanionDb.TryFindByClassName("Velhider", out var velhiderData))
			velhiderPrice = velhiderData.Price;
		var hoglanPrice = 453600;
		if (ZoneServer.Instance.Data.CompanionDb.TryFindByClassName("hoglan_Pet", out var hoglanData))
			hoglanPrice = hoglanData.Price;

		this.SendRawLuaScript(e.Character, $@"
			Melia.Override(""SCR_GET_VELHIDER_PRICE"", function(original)
				return {velhiderPrice};
			end)
		");

		this.SendRawLuaScript(e.Character, $@"
			Melia.Override(""SCR_GET_HOGLAN_PRICE"", function(original)
				return {hoglanPrice};
			end)
		");
	}
}
