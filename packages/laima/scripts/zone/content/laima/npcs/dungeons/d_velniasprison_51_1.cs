//--- Melia Script ----------------------------------------------------------
// Demon Prison District 1
//--- Description -----------------------------------------------------------
// NPCs found in and around Demon Prison District 1.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison511NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(33, 147392, "Lv1 Treasure Chest", "d_velniasprison_51_1", 659.86, 353.03, 1632.73, 0, "TREASUREBOX_LV_D_VELNIASPRISON_51_133", "", "");

		// Sealed Portal - exit back to Aqueduct Bridge Area
		//-------------------------------------------------------------------------
		var sealedPortal = AddNpc(147384, L("Sealed Portal"), "d_velniasprison_51_1", 16, 1730, 180, async dialog =>
		{
			var character = dialog.Player;
			const int SealReleaseCrystal = 650530;

			dialog.SetTitle(L("Sealed Portal"));

			if (character.Inventory.CountItem(SealReleaseCrystal) <= 0)
			{
				await dialog.Msg(L("{#666666}*The portal hums in front of you, the air on its surface thick as river-ice. You push your hand toward it; the binding catches your wrist and holds. Something on the other side is pressing back, smooth and patient and absolute.*{/}"));
				await dialog.Msg(L("{#666666}*The seals are still doing their work. Whatever crosses the threshold from this side stays on this side. There is no way out by force.*{/}"));
				return;
			}

			await dialog.Msg(L("{#666666}*The Seal Release Crystal warms in your pocket as you step toward the portal. The binding parts around it like water around a stone, just enough to let you through.*{/}"));
			await dialog.Msg(L("{#666666}*The crystal cracks in your hand as you cross. One bearer, one passage - that's all the wardmage promised it would carry.*{/}"));

			character.Inventory.Remove(SealReleaseCrystal, 1, InventoryItemRemoveMsg.Destroyed);

			var targetPos = new Position(-1540, 0, -1202);
			if (ZoneServer.Instance.World.TryGetMap("f_farm_47_2", out var targetMap))
				targetPos.Y = targetMap.Ground.GetHeightAt(targetPos);

			character.Warp("f_farm_47_2", targetPos.X, targetPos.Y, targetPos.Z);
		});

		sealedPortal.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
	}
}
