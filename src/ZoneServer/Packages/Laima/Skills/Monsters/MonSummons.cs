using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Mon
{
	/// <summary>
	/// Handler override for default Zombie's Skill_1.
	/// Grants Dark Force stacks to the Bokor owner when the zombie attacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Mon_summons_zombie_Skill_1)]
	public class Mon_summons_zombie_Skill_1Override : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(350);

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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 20, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 150;
			var damageDelay = 350;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);

			// Grant Dark Force stacks to the Bokor owner if zombie is a summon and hit enemies
			if (hits.Count > 0 && caster is Summon summon && summon.Owner != null)
			{
				for (var i = 0; i < hits.Count; i++)
					summon.Owner.StartBuff(BuffId.PowerOfDarkness_Buff, TimeSpan.FromSeconds(30), summon.Owner);
			}
		}
	}

	/// <summary>
	/// Handler override for Wheelchair Zombie's Skill_1.
	/// Grants Dark Force stacks to the Bokor owner when the zombie attacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Mon_Graztas_Skill_1)]
	public class Mon_Graztas_Skill_1Override : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(700);

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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 35, width: 30, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			foreach (var hit in hits)
			{
				var bleedDamage = hit.HitInfo.Damage * 0.5f;
				bleedDamage = Math.Max(1, bleedDamage);
				SkillResultTargetBuff(caster, skill, BuffId.HeavyBleeding, skill.Level, bleedDamage, 3000, 1, 100, -1, hit);
			}


			// Grant Dark Force stacks to the Bokor owner if zombie is a summon and hit enemies
			if (hits.Count > 0 && caster is Summon summon && summon.Owner != null)
			{
				for (var i = 0; i < hits.Count; i++)
					summon.Owner.StartBuff(BuffId.PowerOfDarkness_Buff, TimeSpan.FromSeconds(30), summon.Owner);
			}
		}
	}

	/// <summary>
	/// Handler override for Giant Zombie's Skill_1.
	/// Grants Dark Force stacks to the Bokor owner when the zombie attacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Mon_Zombie_hoplite_Skill_1)]
	public class Mon_Zombie_hoplite_Skill_1Override : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1200);

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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 35, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			foreach (var hit in hits)
			{
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.KnockDown, KnockDirection.TowardsTarget, 90, 10f, 0, 0, 2);
			}

			// Grant Dark Force stacks to the Bokor owner if zombie is a summon and hit enemies
			if (hits.Count > 0 && caster is Summon summon && summon.Owner != null)
			{
				for (var i = 0; i < hits.Count; i++)
					summon.Owner.StartBuff(BuffId.PowerOfDarkness_Buff, TimeSpan.FromSeconds(30), summon.Owner);
			}
		}
	}

	/// <summary>
	/// Handler override for Giant Zombie's Skill_2.
	/// Grants Dark Force stacks to the Bokor owner when the zombie attacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Mon_Zombie_hoplite_Skill_2)]
	public class Mon_Zombie_hoplite_Skill_2Override : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 35, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			foreach (var hit in hits)
			{
				SkillResultTargetBuff(caster, skill, BuffId.Stun, 1, 0, 1000, 1, 40, -1, hit);
			}

			// Grant Dark Force stacks to the Bokor owner if zombie is a summon and hit enemies
			if (hits.Count > 0 && caster is Summon summon && summon.Owner != null)
			{
				for (var i = 0; i < hits.Count; i++)
					summon.Owner.StartBuff(BuffId.PowerOfDarkness_Buff, TimeSpan.FromSeconds(30), summon.Owner);
			}
		}
	}
}
