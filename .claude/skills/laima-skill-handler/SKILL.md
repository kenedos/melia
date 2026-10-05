---
name: laima-skill-handler
description: Create or modify a skill handler in the Laima package. Use when adding a new skill, reworking or overriding an existing skill's behavior, or editing files under src/ZoneServer/Packages/Laima/Skills/.
---

# Laima skill handler

Last verified: 2026-06-10
Background: `doc/dev/skill_handlers.md`, `doc/dev/laima_package.md`
Golden example — diff your handler against it before trusting this template:
`src/ZoneServer/Packages/Laima/Skills/Swordsmen/Barbarian/Barbarian_Cleave.cs`

## File placement

`src/ZoneServer/Packages/Laima/Skills/<ClassTree>/<Job>/<Job>_<SkillName>.cs`
(ClassTree plural: Swordsmen/Archers/Clerics/Scouts/Wizards, or Monsters/Common)

## Interface choice

Match the skill's `useType` in the skill db. Common cases:
- `MeleeGround` → `IGroundSkillHandler` (most attack skills)
- `Force` → `IForceSkillHandler` (projectiles)
- `Self` → `ISelfSkillHandler`
- targeted buff → `ITargetSkillHandler`
- passive → `IPassiveSkillHandler`
Full table: `doc/dev/skill_handlers.md`. When unsure, find an existing Laima
handler for a skill with the same useType and copy its packet sequence.

## Template (ground attack skill)

```csharp
namespace Melia.Zone.Skills.Handlers.Swordsman.Barbarian   // singular tree name!
{
	/// <summary>
	/// Handler for the <Job> skill <Name>.
	/// Per the rework, <one-line design intent>.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Job_SkillName)]
	public class Job_SkillNameOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 40, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			skill.Run(this.Attack(skill, caster, splashArea));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				// optional: if (skillHitResult.Damage > 0) target.StartBuff(BuffId.X, skill.Level, 0, duration, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(50), TimeSpan.Zero));
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
```

Usings: copy from the golden example (`static Melia.Zone.Skills.SkillUseFunctions`
provides `SCR_SkillHit`). Style: tabs, Allman braces.

## Checklist

- [ ] `SkillId.<Job>_<Name>` exists in `src/Shared/Game/Const/SkillId.cs`
- [ ] Skill data entry exists: `packages/laima/db/skills.txt` for new skills,
      or `packages/laima/db/skills_overrides.txt` to patch upstream values
      (factor, sp, cooldown, splash, overheat…)
- [ ] Learnable? → `packages/laima/db/skilltree.txt` entry
- [ ] Cooldown group/time set (db entry or `cooldowns.txt`)
- [ ] Paired buff: handler (see `laima-buff-handler` skill) + `buffs.txt`/
      `buffs_overrides.txt` entry
- [ ] Namespace uses **singular** tree (`Swordsman`), class suffixed `Override`
- [ ] Doc comment states the rework intent
- [ ] Build passes: `dotnet build Melia.sln`
- [ ] If you changed how the skill system itself works, update
      `doc/dev/skill_handlers.md`

If this procedure no longer matches the code, fix this skill file as part of
your change.
