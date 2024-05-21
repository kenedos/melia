//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around West Siauliai Woods.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class SQSiauliaiWestNpcScript : GeneralScript
{
	public override void Load()
	{
		// Archimedes
		//-------------------------------------------------------------------------

		// AddAiNpc(20111, "[General] Archimedes", "f_siauliai_west", -598, -671, 90.0);

		AddAiNpc(155096, "Lycomedes", "f_siauliai_west", -598, -671, 90.0, "Lycomedes");

		AddNpc(20111, "[Soldier] ", "Lycomedes", "f_siauliai_west", -734, -419, 90.0, async dialog =>
		{
			dialog.SetTitle("Lycomedes");
			await dialog.Msg("Be careful, some monsters have recently became aggressive around this area.");
			var response = await dialog.Select("I can help you with some equipment if you can get a few items for me.",
				Option("Okay.", "yes"),
				Option("No thanks.", "no")
			);

			if (response == "yes")
			{
				dialog.Close();
			}
			else if (response == "no")
			{
				dialog.Close();
			}
		});
	}
}
