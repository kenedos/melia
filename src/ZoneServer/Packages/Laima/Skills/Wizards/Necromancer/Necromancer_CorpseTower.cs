using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Spawning;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_CorpseTower)]
	public class Necromancer_CorpseTowerOverride : IGroundSkillHandler
	{
		private const int CorpsePartsCost = 7;
		private const float TowerDurationSeconds = 60f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var corpseParts = (int)character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt);

			if (corpseParts < CorpsePartsCost)
			{
				caster.ServerMessage(Localization.Get("Not enough corpse parts."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			character.SetEtcProperty(PropertyName.Necro_DeadPartsCnt, corpseParts - CorpsePartsCost);
			Send.ZC_OBJECT_PROPERTY(character, character.Etc, PropertyName.Necro_DeadPartsCnt);

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(character, skill, farPos));
		}

		private async Task HandleSkill(Character character, Skill skill, Position spawnPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			if (character.IsDead || character.Map == null)
				return;

			var tower = MonsterSkillCreateMob(skill, character, "pcskill_CorpseTower", spawnPos, 0f, "local name = GetClassString(\"Monster\", \"pcskill_CorpseTower\", \"Name\"); return SofS(name, self.Name);", "PC_Summon_Holding", 0, TowerDurationSeconds, "None", "WlkMSPD#0#RunMSPD#0");

			if (tower == null)
				return;

			var intelligence = Math.Max(0f, character.Properties.GetFloat(PropertyName.INT));
			var spirit = Math.Max(0f, character.Properties.GetFloat(PropertyName.MNA));
			var skillLevel = Math.Max(1, skill.Level);
			var attackBonus = skillLevel + intelligence + spirit;

			var enhanceLevel = Math.Clamp(character.GetAbilityLevel(AbilityId.Necromancer6), 0, 100);
			var enhanceBonus = enhanceLevel * 0.005f + (enhanceLevel >= 100 ? 0.10f : 0f);
			var enhanceMultiplier = 1f + enhanceBonus;

			var currentPhysicalBonus = tower.Properties.GetFloat(PropertyName.PATK_BM);
			var currentMagicBonus = tower.Properties.GetFloat(PropertyName.MATK_BM);
			var basePhysicalAttack = tower.Properties.GetFloat(PropertyName.MINPATK) - currentPhysicalBonus;
			var baseMagicAttack = tower.Properties.GetFloat(PropertyName.MINMATK) - currentMagicBonus;

			var finalPhysicalAttack = (basePhysicalAttack + attackBonus) * enhanceMultiplier;
			var finalMagicAttack = (baseMagicAttack + attackBonus) * enhanceMultiplier;

			tower.Properties.SetFloat(PropertyName.PATK_BM, finalPhysicalAttack - basePhysicalAttack);
			tower.Properties.SetFloat(PropertyName.MATK_BM, finalMagicAttack - baseMagicAttack);
			// HPCount enables the same one-damage mechanic used by Defend the Torch.
			// Preserve the tower's current maximum HP as its hit-point budget.
			tower.Properties.InvalidateAll();
			var maximumHp = Math.Max(1f, tower.Properties.GetFloat(PropertyName.MHP));
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.HPCount, maximumHp);
			tower.ApplyOverrides(propertyOverrides);
			tower.Properties.InvalidateAll();
			tower.HealToFull();

			Console.WriteLine($"[CORPSE_TOWER_ENHANCE] skillLevel={skillLevel} INT={intelligence} SPR={spirit} enhanceLevel={enhanceLevel} enhanceBonus={enhanceBonus:P1} PATK={tower.Properties.GetFloat(PropertyName.MINPATK)} MATK={tower.Properties.GetFloat(PropertyName.MINMATK)}");
		}
	}
}
