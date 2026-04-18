using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Vibora_Spiritarcher_Skill_1)]
	public class Mon_Vibora_Spiritarcher_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var originPos = caster.Position;
			var hitDelay = 800 + (int)(caster.Position.Get2DDistance(target.Position) * 3.2);
			var leadPos = GetLeadPosition(target, hitDelay);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aniTime = hitDelay + 200;
			_ = ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritarcher_Skill_2)]
	public class Mon_Vibora_Spiritarcher_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(420);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 140, width: 13, angle: 25f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 220;
			var aniTime = 420;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 140, width: 13, angle: 25f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 430;
			aniTime = 10;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 140, width: 13, angle: 25f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 440;
			aniTime = 10;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritelitesoldier_Skill_1)]
	public class Mon_Vibora_Spiritelitesoldier_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(330);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 31.25f, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 330;
			var aniTime = 330;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 31.25f, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 330;
			aniTime = 0;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritelitesoldier_Skill_2)]
	public class Mon_Vibora_Spiritelitesoldier_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(420);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 90, angle: 50f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 220;
			var aniTime = 420;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			// TODO: No Implementation S_R_ABIL_KNOCK_TARGET

		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritelitesoldier_Skill_3)]
	public class Mon_Vibora_Spiritelitesoldier_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var targetPos = GetRelativePosition(PosType.TargetRandom, caster, target);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 20f, (int)skill.Properties.GetFloat(PropertyName.SkillSR)));
			var targets = caster.GetTargets();
			SkillTargetDamage(skill, caster, targets, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritsoldier_Skill_1)]
	public class Mon_Vibora_Spiritsoldier_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(350);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 150;
			var aniTime = 350;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			var position = originPos.GetRelative(farPos, distance: 0);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash", 0.2f, 0f, 0f, 270f);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritsoldier_Skill_2)]
	public class Mon_Vibora_Spiritsoldier_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(375);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 175;
			var aniTime = 375;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			await skill.Wait(TimeSpan.FromMilliseconds(375));
			var position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "E_warrior_pierce", 1f, 0f, 0f, 90f);
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_dash021_twirl", 1f, 0f, 0f, 270f);
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "F_smoke037", 0.30000001f, 0f, 0f, 270f);
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "F_smoke174", 2f, 0f, 0f, 270f);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritsoldier_Skill_3)]
	public class Mon_Vibora_Spiritsoldier_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(250);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 50;
			var aniTime = 250;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 500;
			aniTime = 250;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 646;
			aniTime = 146;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 792;
			aniTime = 146;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 938;
			aniTime = 146;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1084;
			aniTime = 146;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1230;
			aniTime = 146;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1376;
			aniTime = 146;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1422;
			aniTime = 46;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1668;
			aniTime = 246;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			var position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
			await skill.Wait(TimeSpan.FromMilliseconds(146));
			position = originPos.GetRelative(farPos, distance: 0, angle: 90f);
			Send.ZC_GROUND_EFFECT(caster, position, "I_warrior_florysh_shot_dash_re", 0.60000002f, 0f, 0f, 270f);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritwizard_Skill_1)]
	public class Mon_Vibora_Spiritwizard_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var originPos = caster.Position;
			var hitDelay = 400 + (int)(caster.Position.Get2DDistance(target.Position) * 4.6);
			var leadPos = GetLeadPosition(target, hitDelay);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aniTime = hitDelay + 200;
			_ = ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritwizard_Skill_2)]
	public class Mon_Vibora_Spiritwizard_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var originPos = caster.Position;
			var hitDelay = 800 + (int)(caster.Position.Get2DDistance(target.Position) * 3.2);
			var leadPos = GetLeadPosition(target, hitDelay);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aniTime = hitDelay + 200;
			_ = ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritwizard_Skill_3)]
	public class Mon_Vibora_Spiritwizard_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			var targetPos = GetRelativePosition(PosType.Target, caster, target);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Shadowmancer_ShadowCondensationBuffPad);
		}
	}

	[SkillHandler(SkillId.Mon_Vibora_Spiritwizard_Skill_4)]
	public class Mon_Vibora_Spiritwizard_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(400));

		}
	}
}
