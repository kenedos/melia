using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Summon Salamion.
	/// Summons a fire spirit that heals the sorcerer and their summons.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_SummonSalamion)]
	public class Sorcerer_SummonSalamionOverride : IGroundSkillHandler
	{
		private const int SummonLifetimeSeconds = 300;
		private const string SalamionVar = "SORCERER_SUMMONSALOON";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(character, skill, originPos, farPos));
		}

		private async Task HandleSkill(Character character, Skill skill, Position originPos, Position farPos)
		{
			KillExistingSalamions(character);

			await skill.Wait(TimeSpan.FromMilliseconds(1300));

			var spawnPos = originPos.GetRelative(farPos, distance: 20f);
			var summon = new Summon(character, (int)MonsterId.Salamion, RelationType.Friendly);

			summon.Name = $"!@#${{Auto_1}}_of_{{Auto_2}}$*$Auto_1$*${character.Name}$*$Auto_2$*$@dicID_^*$ETC_20150317_000235$*^#@!";
			summon.Position = spawnPos;
			summon.Direction = character.Direction;
			summon.Map = character.Map;
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;
			summon.Level = character.Level;
			summon.Vars.SetInt(SalamionVar, 1);

			CalculateSummonStats(summon, character, skill);

			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.FIXMSPD_BM, 80f);
			summon.InvalidateProperties();

			summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromSeconds(SummonLifetimeSeconds)));
			summon.SetState(true, canMove: true, hasAi: false);
			summon.Components.Add(new AiComponent(summon, "PC_Summon_Necromancer", character));

			var maxHp = summon.Properties.GetFloat(PropertyName.MHP);
			summon.Heal(maxHp, 0);

			character.Summons.AddSummon(summon);
			character.Variables.Temp.SetInt(SalamionVar, summon.Handle);
			summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);

			if (character.IsAbilityActive(AbilityId.Sorcerer17))
				summon.StartBuff(BuffId.SummonSalamion_Buff, skill.Level, 0, TimeSpan.FromSeconds(SummonLifetimeSeconds), character);

			Send.ZC_NORMAL.SummonPlayAnimation(character, SalamionVar, 1);
			Send.ZC_MSPD(character, summon, 0, summon.Properties.GetFloat(PropertyName.MSPD));
		}

		private void CalculateSummonStats(Summon summon, Character caster, Skill skill)
		{
			var intelligence = Math.Max(0f, caster.Properties.GetFloat(PropertyName.INT));
			var constitution = Math.Max(0f, caster.Properties.GetFloat(PropertyName.CON));
			var spirit = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MNA));
			var skillLevel = Math.Clamp(skill.Level, 1, 10);
			var skillScale = Math.Max(1f, skill.Level);
			var inheritanceRate = 0.05f + skillLevel * 0.015f;

			var hpFactor = 1f + intelligence / 50f + constitution / 10f;
			var offensiveFactor = 1f + intelligence / 250f + skill.Level * 0.05f;
			var defensiveFactor = 1f + spirit / 50f + skill.Level * 0.25f;
			var inheritedMHP = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MHP)) * inheritanceRate;
			var inheritedDEF = Math.Max(0f, caster.Properties.GetFloat(PropertyName.DEF)) * inheritanceRate;
			var inheritedMDEF = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MDEF)) * inheritanceRate;
			var inheritedCRTDR = Math.Max(0f, caster.Properties.GetFloat(PropertyName.CRTDR)) * inheritanceRate;
			var casterMinimumMagicAttack = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MINMATK));
			var casterMaximumMagicAttack = Math.Max(casterMinimumMagicAttack, caster.Properties.GetFloat(PropertyName.MAXMATK));
			var inheritedMagicAttack = ((casterMinimumMagicAttack + casterMaximumMagicAttack) / 2f) * inheritanceRate;
			var inheritedAccuracy = Math.Max(0f, caster.Properties.GetFloat(PropertyName.HR)) * inheritanceRate;

			var currentMHPBonus = summon.Properties.GetFloat(PropertyName.MHP_BM);
			var currentPATKBonus = summon.Properties.GetFloat(PropertyName.PATK_BM);
			var currentMATKBonus = summon.Properties.GetFloat(PropertyName.MATK_BM);
			var currentDEFBonus = summon.Properties.GetFloat(PropertyName.DEF_BM);
			var currentMDEFBonus = summon.Properties.GetFloat(PropertyName.MDEF_BM);
			var currentCRTDRBonus = summon.Properties.GetFloat(PropertyName.CRTDR_BM);
			var currentAccuracyBonus = summon.Properties.GetFloat(PropertyName.HR_BM);

			var baseMHP = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MHP) - currentMHPBonus);
			var basePATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MINPATK) - currentPATKBonus);
			var baseMATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MINMATK) - currentMATKBonus);
			var baseDEF = Math.Max(0f, summon.Properties.GetFloat(PropertyName.DEF) - currentDEFBonus);
			var baseMDEF = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MDEF) - currentMDEFBonus);

			var finalMHP = baseMHP * skillScale * hpFactor;
			var finalPATK = basePATK * skillScale * offensiveFactor;
			var finalMATK = baseMATK * skillScale * offensiveFactor;
			var finalDEF = baseDEF * skillScale * defensiveFactor;
			var finalMDEF = baseMDEF * skillScale * defensiveFactor;

			summon.Properties.SetFloat(PropertyName.MHP_BM, currentMHPBonus + finalMHP - baseMHP + inheritedMHP);
			summon.Properties.SetFloat(PropertyName.PATK_BM, currentPATKBonus + finalPATK - basePATK + inheritedMagicAttack);
			summon.Properties.SetFloat(PropertyName.MATK_BM, currentMATKBonus + finalMATK - baseMATK + inheritedMagicAttack);
			summon.Properties.SetFloat(PropertyName.DEF_BM, currentDEFBonus + finalDEF - baseDEF + inheritedDEF);
			summon.Properties.SetFloat(PropertyName.MDEF_BM, currentMDEFBonus + finalMDEF - baseMDEF + inheritedMDEF);
			summon.Properties.SetFloat(PropertyName.CRTDR_BM, currentCRTDRBonus + inheritedCRTDR);
			summon.Properties.SetFloat(PropertyName.HR_BM, currentAccuracyBonus + inheritedAccuracy);
		}

		private void KillExistingSalamions(Character character)
		{
			var existingSalamions = character.Summons.GetSummons(s => s.Vars.TryGetInt(SalamionVar, out var value) && value == 1);

			foreach (var summon in existingSalamions)
			{
				if (!summon.IsDead)
					summon.Kill(character);
			}

			character.Variables.Temp.Remove(SalamionVar);
		}
	}
}
