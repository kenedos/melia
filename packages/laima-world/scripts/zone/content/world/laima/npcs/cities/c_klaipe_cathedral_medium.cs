//--- Melia Script ----------------------------------------------------------
// Laima's Sanctuary
//--- Description -----------------------------------------------------------
// NPCs found in and around Laima's Sanctuary.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Util;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class CKlaipeCathedralMediumNpcScript : GeneralScript
{
	protected override void Load()
	{
		CreateSacramentalShop();

		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue("WARP_C_KLAIPE_CATHEDRAL_MEDIUM", "c_klaipe_cathedral_medium", 262, -153, 0, L("Statue of Goddess Vakarine"));

		// [Almsgiver] Rasa
		//-------------------------------------------------------------------------
		AddNpc(154052, L("[Almsgiver] Rasa"), "Rasa", "c_klaipe_cathedral_medium", 24, -142, 45, async dialog =>
		{
			dialog.SetTitle(L("Rasa"));

			var character = dialog.Player;

			if (!character.TryGetSkill(SkillId.Pardoner_Oblation, out _))
			{
				await dialog.Msg(L("This is where the offerings come to rest. Only a pardoner may hand them over, though. Bring me one and we'll talk."));
				return;
			}

			var cooldown = PardonerSkillHelper.GetChurchDonationCooldown(character);
			if (cooldown > TimeSpan.Zero)
			{
				await dialog.Msg(L("We're still sorting what you brought last time. Everything gets counted, blessed and written down, and that takes a day. Come back in {0} hours, {1} minutes and {2} seconds."), cooldown.Hours, cooldown.Minutes, cooldown.Seconds);
				return;
			}

			await dialog.Msg(L("The Goddess keeps no ledger, but I do. Open your box and let's see what the faithful gave you."));

			dialog.OpenOblationBox(atChurch: true);
		});

		// [Sacristan] Girenas
		//-------------------------------------------------------------------------
		var sacristan = AddNpc(155042, L("[Sacristan] Girenas"), "Girenas", "c_klaipe_cathedral_medium", 343, 108, 0, async dialog =>
		{
			dialog.SetTitle(L("Girenas"));

			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("Water, powder, gyslotis, parchment. Everything a blessing needs and nothing it doesn't. Take what you came for."));
			else
				await dialog.Msg(L("I keep the sacristy stocked so the priests never have to ask twice. You're welcome to the same shelves."));

			await dialog.OpenShop("SanctuarySacramentals");
		});

		sacristan.AssociatedShopName = "SanctuarySacramentals";
		sacristan.ShopType = ShopType.Material;
	}

	/// <summary>
	/// Creates the shop selling the materials cleric skills consume
	/// </summary>
	private void CreateSacramentalShop()
	{
		CreateShop("SanctuarySacramentals", shop =>
		{
			shop.AddItem(640068, amount: 1, price: 50);
			shop.AddItem(640069, amount: 1, price: 20);
			shop.AddItem(640031, amount: 1, price: 35);
			shop.AddItem(645530, amount: 1, price: 1000);
		});
	}
}
