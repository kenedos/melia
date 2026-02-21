using System;
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

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_eldigo_Skill_1)]
	public class Mon_eldigo_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 10, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_eldigo_Skill_2)]
	public class Mon_eldigo_Skill_2 : ITargetSkillHandler
	{
		private const int CastTimeMs = 1500;

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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var targetPos = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 1.6378922, angle: 0f);

			if (!await MonsterCastTime(skill, caster, "Fire Wall", CastTimeMs, target))
				return;

			await skill.Wait(TimeSpan.FromMilliseconds(800));
			targetPos = originPos.GetRelative(farPos, distance: 20f, angle: 1f);
			SkillCreatePad(caster, skill, targetPos, 0.031604018f, PadName.Mon_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			targetPos = originPos.GetRelative(farPos, distance: 40f, angle: 0f);
			SkillCreatePad(caster, skill, targetPos, 0.011262165f, PadName.Mon_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = originPos.GetRelative(farPos, distance: 60f, angle: 1f);
			SkillCreatePad(caster, skill, targetPos, 0.020105751f, PadName.Mon_firewall);
		}
	}
}
