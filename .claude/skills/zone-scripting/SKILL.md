---
name: zone-scripting
description: Write or modify zone content scripts — NPCs, dialogs, quests, spawns, warps, dungeons, custom events. Use for files under system/scripts or packages/laima/scripts, or when asked to add an NPC, quest, spawner, or scripted content.
---

# Zone content scripting

Last verified: 2026-06-10
Background: `doc/dev/scripting_system.md`
Golden example (NPC + keyword + quest + dynamic map):
`packages/laima/scripts/zone/content/laima/jobs/cryomancer_quest.cs`

## Procedure

1. Place Laima content under
   `packages/laima/scripts/zone/content/laima/<category>/` (dungeons, items,
   jobs, maps, mobs…). Confirm the folder is covered by a glob in
   `packages/laima/scripts/zone/scripts.txt` / `scripts_content.txt`.
2. Subclass `GeneralScript`, override `Load()`, use the `Shortcuts` API
   (`using static Melia.Zone.Scripting.Shortcuts;`).
3. Wrap every player-visible string in `L("...")`.
4. Map class names: look up in `doc/packages/laima/available_maps.md`.
5. Detailed API reference (don't guess function signatures):
   - NPCs/dialogs: `doc/server/scripting/npc_scripting.md`
   - Quests: `doc/server/scripting/quests.md`
   - Spawns: `doc/server/scripting/spawns.md`
   - All `SCR_*` scriptable hooks: `doc/server/scripting/scriptable_functions.md`

## Minimal NPC template

```csharp
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

public class MyNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddNpc(20137, L("[Title] Name"), "c_Klaipe", -95, -309, 0, MyDialog);
	}

	private async Task MyDialog(Dialog dialog)
	{
		dialog.SetTitle(L("Name"));
		await dialog.Msg(L("Hello."));
	}
}
```

## Checklist

- [ ] File under `packages/laima/scripts/zone/...` (NOT `system/scripts/` or
      the operator's `custom/`/`user/` tiers)
- [ ] Covered by a glob in the laima script list files
- [ ] Strings wrapped in `L()`
- [ ] Ids referenced (monster, item, map) exist in db/data
- [ ] Server boots without script compile errors (scripts compile at startup;
      errors are logged at boot)
- [ ] Do NOT edit `system/scripts/zone/scripts_packages.txt` (auto-generated)

If this procedure no longer matches the code, fix this skill file as part of
your change.
