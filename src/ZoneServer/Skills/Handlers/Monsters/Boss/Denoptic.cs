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
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillUseHelper;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Denoptic_Skill_1)]
	public class Mon_boss_Denoptic_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos);
			SkillArcJump(caster, position, 100f, 0.2f, 0.3f, 1f, 1f, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(3500));
			position = caster.Position;
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Denoptic, 0f, 200f, 8, -45f, 150f, 50f, 50f);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.Target, caster, target, rand: 110);
			SkillArcJump(caster, position, 100f, 0.2f, 0.3f, 1f, 1f, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			position = caster.Position;
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Denoptic, 0f, 200f, 8, -45f, 150f, 50f, 50f);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100);
			SkillArcJump(caster, position, 100f, 0.2f, 0.3f, 1f, 1f, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(3300));
			position = caster.Position;
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Denoptic, 0f, 200f, 8, -45f, 150f, 50f, 50f);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Denoptic_Skill_2)]
	public class Mon_boss_Denoptic_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 20);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 20);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 20);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 20);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 20);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Denoptic_Skill_3)]
	public class Mon_boss_Denoptic_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2200;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 120f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Denoptic_Skill_4)]
	public class Mon_boss_Denoptic_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Denoptic_Skill_5)]
	public class Mon_boss_Denoptic_Skill_5 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hits = new List<SkillHitInfo>();
			var position = caster.Position;
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Denoptic, 0f, 200f, 8, -45f, 150f, 50f, 50f);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = caster.Position;
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Denoptic, 1.0471976f, 300f, 8, 45f, 150f, 50f, 50f);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Denoptic_Skill_6)]
	public class Mon_boss_Denoptic_Skill_6 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2116);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hits = new List<SkillHitInfo>();
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = caster.Position;
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Denoptic, 0f, 200f, 8, -45f, 150f, 50f, 50f);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = caster.Position;
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Denoptic, 1.0471976f, 300f, 8, 45f, 150f, 50f, 50f);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}
}
