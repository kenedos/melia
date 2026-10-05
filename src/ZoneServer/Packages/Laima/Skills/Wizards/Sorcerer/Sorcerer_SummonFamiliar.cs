using System;
using System.Linq;
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
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Summon Familiar.
	/// Summons 5 bat familiars that attack enemies in a kamikaze fashion.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_SummonFamiliar)]
	public class Sorcerer_SummonFamiliarOverride : IGroundSkillHandler
	{
		private const int BatLifetimeSeconds = 60;
		private const string FamiliarVar = "SORCERER_FAMILIAR";

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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SYNC_START(caster, skillHandle, 1);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, skill.Data.DefaultHitDelay);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			RemoveExistingFamiliars(character);
			CreateFamiliarBats(character, skill);
			character.StartBuff(BuffId.sorcerer_bat, skill.Level, 0, TimeSpan.FromSeconds(BatLifetimeSeconds), character);
		}

		private void RemoveExistingFamiliars(Character character)
		{
			if (character.TryGetBuff(BuffId.sorcerer_bat, out _))
				character.StopBuff(BuffId.sorcerer_bat);

			var familiars = character.Summons.GetSummons(s =>
				s.Id == (int)MonsterId.Familiar ||
				s.Vars.TryGetInt(FamiliarVar, out var value) && value == 1);

			foreach (var familiar in familiars)
			{
				if (!familiar.IsDead)
					familiar.Kill(character);
			}
		}

		private void CreateFamiliarBats(Character character, Skill skill)
		{
			var random = RandomProvider.Get();

			var batCount = Math.Max(1, skill.Level);

			for (var i = 0; i < batCount; i++)
			{
				var summon = new Summon(character, (int)MonsterId.Familiar, RelationType.Friendly);
				var spawnPos = character.Position.GetRandomInRange2D(20, random);

				summon.Position = spawnPos;
				summon.Direction = character.Direction;
				summon.Map = character.Map;
				summon.OwnerHandle = character.Handle;
				summon.Faction = FactionType.Law;
				summon.Level = character.Level;

				summon.Vars.SetInt(FamiliarVar, 1);
				summon.Vars.SetInt("SORCERER_BAT_STOP", 0);
				summon.Vars.SetInt("SORCERER_FAMILIAR_SKILL_LEVEL", skill.Level);
				summon.Vars.SetBool("EnableAIOutOfPC", true);

				CalculateFamiliarStats(summon, character, skill);

				summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromSeconds(BatLifetimeSeconds)));

				summon.SetState(true);

				var maxHp = summon.Properties.GetFloat(PropertyName.MHP);
				summon.Heal(maxHp, 0);

				HoldSummonMovement(skill, summon, 800);
				character.Summons.AddSummon(summon);

				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

				Send.ZC_SYNC_START(character, skillHandle, 1);
				Send.ZC_MSPD(character, summon, 0, summon.Properties.GetFloat(PropertyName.MSPD));

				summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);

				Send.ZC_SYNC_END(character, skillHandle, 0);
				Send.ZC_SYNC_EXEC_BY_SKILL_TIME(character, skillHandle, skill.Data.DefaultHitDelay);
			}
		}

		private void CalculateFamiliarStats(Summon summon, Character caster, Skill skill)
		{
			var intelligence = Math.Max(0f, caster.Properties.GetFloat(PropertyName.INT));
			var constitution = Math.Max(0f, caster.Properties.GetFloat(PropertyName.CON));
			var spirit = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MNA));
			var skillLevel = Math.Clamp(skill.Level, 1, 10);
			var skillScale = Math.Max(1f, skill.Level);
			var inheritanceRate = 0.05f + skillLevel * 0.015f;

			var hpFactor = 1f + intelligence / 50f + constitution / 10f;
			var offensiveFactor = 1f + intelligence / 250f + skill.Level * 0.15f;
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
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);
		}

		private void HoldSummonMovement(Skill skill, Summon summon, int durationMs)
		{
			summon.Vars.SetBool("HoldMovement", true);
			_ = RemoveHoldAfterDelay(skill, summon, durationMs);
		}

		private async System.Threading.Tasks.Task RemoveHoldAfterDelay(
			Skill skill,
			Summon summon,
			int durationMs
		)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(durationMs));

			if (!summon.IsDead)
				summon.Vars.SetBool("HoldMovement", false);
		}
	}
}
