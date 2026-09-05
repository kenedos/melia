//--- Melia Script ----------------------------------------------------------
// Highlander Camp
//--- Description -----------------------------------------------------------
// NPCs found in and around the Highlander Camp.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class CHighlanderNpcScript : GeneralScript
{
	protected override void Load()
	{
		// [Almsgiver] Rasa
		//-------------------------------------------------------------------------
		AddNpc(154052, L("[Almsgiver] Rasa"), "Rasa", "c_highlander", 0, 0, 0, async dialog =>
		{
			dialog.SetTitle(L("Rasa"));

			var character = dialog.Player;

			if (!character.TryGetSkill(SkillId.Pardoner_Oblation, out _))
			{
				await dialog.Msg(L("The church takes offerings through its pardoners. Bring me one and we'll talk."));
				return;
			}

			var cooldown = PardonerSkillHelper.GetChurchDonationCooldown(character);
			if (cooldown > TimeSpan.Zero)
			{
				await dialog.Msg(L("The church needs some time to arrange the relics you've donated us. Please come back in {0} hours, {1} minutes and {2} seconds."), cooldown.Hours, cooldown.Minutes, cooldown.Seconds);
				return;
			}

			await dialog.Msg(L("Offerings are welcome. Show me what the faithful have left in your box."));

			dialog.OpenOblationBox(atChurch: true);
		});
	}
}
