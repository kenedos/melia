using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer skill Create Shoggoth.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Necromancer_CreateShoggoth)]
	public class Necromancer_CreateShoggothOverride : IGroundSkillHandler
	{
		private const int CorpsePartsCost = 30;
		private const float EnlargeChancePerLevel = 2.5f;
		private const float EnlargedHpRatePerLevel = 0.02f;
		private const float EnlargedScale = 1.5f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!NecromancerSkillHelper.HasNecronomiconCard(caster))
			{
				caster.ServerMessage(Localization.Get("Insert a Beast, Plant or Mutant card in the Necronomicon first."));
				return;
			}

			if (!NecromancerSkillHelper.HasCorpseParts(caster, CorpsePartsCost))
			{
				caster.ServerMessage(Localization.Get("Not enough corpse parts."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			NecromancerSkillHelper.SpendCorpseParts(caster, CorpsePartsCost);

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var spawnPos = originPos.GetRelative(farPos, distance: 80f);
			var shoggoth = MonsterSkillCreateMob(skill, caster, "pcskill_shogogoth", spawnPos, 0f, "", "PC_Summon", 0, 0f, "None", "WlkMSPD#120#RunMSPD#120#$NECRO_MON#1");
			if (shoggoth == null)
				return;

			NecromancerSkillHelper.RemovePreviousSummons(caster, shoggoth);

			NecromancerSkillHelper.ApplySummonTransfer(shoggoth, caster, NecromancerSkillHelper.GetReinforcedRatio(skill, PropertyName.CaptionRatio), skill.Properties.GetFloat(PropertyName.CaptionRatio2), skill.Properties.GetFloat(PropertyName.CaptionRatio3));

			if (caster.TryGetActiveAbilityLevel(AbilityId.Necromancer8, out var enlargeLevel) && GameRandom.Get().NextDouble() * 100 < enlargeLevel * EnlargeChancePerLevel)
			{
				shoggoth.Properties.Modify(PropertyName.MHP_BM, shoggoth.Properties.GetFloat(PropertyName.MHP) * enlargeLevel * EnlargedHpRatePerLevel);
				shoggoth.Properties.SetFloat(PropertyName.HP, shoggoth.Properties.GetFloat(PropertyName.MHP));
				shoggoth.ChangeScale(EnlargedScale, 1f);
			}
		}
	}
}
