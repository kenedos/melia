using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Lycanthropy wolf's Scratch, two swipes at the
	/// enemies in front of it.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_1)]
	public class Druid_WolfScratchOverride : IGroundSkillHandler
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

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 50, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			await SkillAttack(caster, skill, splashArea, 400, 400);
			await SkillAttack(caster, skill, splashArea, 400, 300);
		}
	}

	/// <summary>
	/// Handler for the Lycanthropy wolf's Slash, a line of bursts that
	/// knocks back the enemies in front of it.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_3)]
	public class Druid_WolfSlashOverride : IGroundSkillHandler
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

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			var startingPosition = caster.Position.GetRelative(caster.Direction, 30f);
			var endingPosition = caster.Position.GetRelative(caster.Direction, 150f);

			await skill.Wait(TimeSpan.FromMilliseconds(1190));

			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup008_smoke1", 0.7f),
				Range = 45f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	/// <summary>
	/// Handler for the Lycanthropy wolf's Wild Breath, five hits on the
	/// enemies in front of it that leave them confused.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_4)]
	public class Druid_WolfWildBreathOverride : IGroundSkillHandler
	{
		private const int HitCount = 5;
		private const int ConfuseDuration = 5000;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 40, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hits = new List<SkillHitInfo>();

			await SkillAttack(caster, skill, splashArea, 1100, 1100, hits);

			for (var i = 1; i < HitCount; i++)
				await SkillAttack(caster, skill, splashArea, 1100, 150, hits);

			SkillResultTargetBuff(caster, skill, BuffId.UC_confuse, 1, 0f, ConfuseDuration, 1, 100, -1, hits);
		}
	}

	/// <summary>
	/// Handler for the Lycanthropy wolf's Warcry, a howl that blasts and
	/// stuns the enemies around it.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_5)]
	public class Druid_WolfWarcryOverride : IGroundSkillHandler
	{
		private const int StunDuration = 3000;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			var hits = new List<SkillHitInfo>();

			await skill.Wait(TimeSpan.FromMilliseconds(1400));

			await EffectAndHit(skill, caster, caster.Position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_archer_SiegeBurst_explosion", 1.7f),
				Range = 150f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			}, hits);

			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, StunDuration, 1, 100, -1, hits);
		}
	}
}
