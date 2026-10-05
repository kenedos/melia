using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Database;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.Features.Transcendence
{
public static class TranscendenceMigration
{
private const bool Enabled = true;

		public static void Repair(Character character, Account account)
		{
if (!Enabled || character == null)
return;

var scanned = 0;
var repaired = 0;
var processedItems = new HashSet<long>();

RepairCollection(
character,
character.Inventory.GetItems().Values,
true,
processedItems,
ref scanned,
ref repaired);

RepairCollection(
character,
character.Inventory.GetEquip().Values,
true,
processedItems,
ref scanned,
ref repaired);

RepairCollection(
character,
character.Inventory.GetCards().Values,
true,
processedItems,
ref scanned,
ref repaired);

RepairCollection(
character,
character.PersonalStorage.GetItems().Values,
false,
processedItems,
ref scanned,
ref repaired);

if (account?.TeamStorage != null)
{
RepairCollection(
character,
account.TeamStorage.GetItems().Values,
false,
processedItems,
ref scanned,
ref repaired);
}

if (repaired > 0)
{
character.InvalidateProperties();

character.AddonMessage("INV_ITEM_LIST_GET");
character.AddonMessage("EQUIP_ITEM_LIST_UPDATE");

Log.Info(
"[TranscendenceMigration] Character={0} CharacterId={1} Scanned={2} Repaired={3}",
character.Name,
character.DbId,
scanned,
repaired);
}
}

private static void RepairCollection(
Character character,
IEnumerable<Item> items,
bool synchronizeWithClient,
HashSet<long> processedItems,
ref int scanned,
ref int repaired)
{
if (items == null)
return;

foreach (var item in items.Where(item => item != null))
{
var uniqueId = GetUniqueId(item);

if (!processedItems.Add(uniqueId))
continue;

scanned++;

var transcendStage = item.Properties.GetFloat(
PropertyName.Transcend,
0);

if (transcendStage <= 0)
continue;

// Força uma alteração real na propriedade.
//
// Definir 3 novamente como 3 pode não marcar a propriedade
// como dirty. A sequência 3 -> 0 -> 3 garante que o valor
// final correto seja persistido e enviado ao cliente.
item.Properties.SetFloat(
PropertyName.Transcend,
0);

item.Properties.SetFloat(
PropertyName.Transcend,
transcendStage);

item.Properties.InvalidateAll();

if (synchronizeWithClient && character.Connection != null)
Send.ZC_OBJECT_PROPERTY(character, item);

repaired++;
}
}

private static long GetUniqueId(Item item)
{
if (item.DbId > 0)
return item.DbId;

return item.ObjectId;
}
}
}
