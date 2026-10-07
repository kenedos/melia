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
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillRangePreviewHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Mon
{
	/// <summary>
	/// Handler override for default Zombie's Skill_1.
	/// Grants Dark Force stacks to the Bokor owner when the zombie attacks.
	/// </summary>
	[Package("system")]
	[SkillHandler(SkillId.Mon_summons_zombie_Skill_1)]
	public class Mon_summons_zombie_Skill_1Override : ITargetSkillHandler
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
			var aniTime = 350;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);

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
	[Package("system")]
	[SkillHandler(SkillId.Mon_Graztas_Skill_1)]
	public class Mon_Graztas_Skill_1Override : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(700);

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
			var aniTime = 700;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
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
	[Package("system")]
	[SkillHandler(SkillId.Mon_Zombie_hoplite_Skill_1)]
	public class Mon_Zombie_hoplite_Skill_1Override : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1200);

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
			var aniTime = 1200;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
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
	/// Handler for Sorcerer Familiar bat's attack skill.
	/// </summary>
	[Package("system")]
	[SkillHandler(SkillId.Mon_pcskill_summon_Familiar_Skill_1)]
	public class Mon_pcskill_summon_Familiar_Skill_1 : ParametersOnlySkill
	{
		protected override TimeSpan AniTime { get; } = TimeSpan.Zero;
		protected override TimeSpan HitDelay { get; } = TimeSpan.Zero;
		protected override SplashType SplashType { get; } = SplashType.Circle;
		protected override float Length { get; } = 40f;
		protected override float Width { get; } = 15f;
		protected override float Angle { get; } = 0f;
	}

	/// <summary>
	/// Handler override for Giant Zombie's Skill_2.
	/// Grants Dark Force stacks to the Bokor owner when the zombie attacks.
	/// </summary>
	[Package("system")]
	[SkillHandler(SkillId.Mon_Zombie_hoplite_Skill_2)]
	public class Mon_Zombie_hoplite_Skill_2Override : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1000);

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
			var aniTime = 1000;

			// Attack enemies
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
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

	/// <summary>
	/// Handler override for the Soul Fox Shikigami fox's Skill_1, which
	/// throws an orb that flies out from the fox and back, striking the
	/// enemies in its path each way as its Onmyoji.
	/// </summary>
	[Package("system")]
	[SkillHandler(SkillId.Mon_pcskill_FireFoxShikigami_Skill_1)]
	public class Mon_pcskill_FireFoxShikigami_Skill_1Override : ITargetSkillHandler
	{
		private const float OrbDistance = 150f;
		private const float OrbRadius = 30f;
		private const float OrbSpeed = 200f;
		private const float OrbReturnSpeed = 350f;
		private const int ThrowDelay = 250;

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

			caster.TurnTowards(target);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);

			skill.Run(this.HandleSkill(caster, target, skill, caster.Position));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos)
		{
			var splashArea = new SplashAreas.Square(originPos, caster.Direction, OrbDistance, OrbRadius);
			var distance = Math.Min(OrbDistance, (float)originPos.Get2DDistance(target.Position));
			var outHit = ThrowDelay + (int)(distance / OrbSpeed * 1000);
			var returnHit = ThrowDelay + (int)(OrbDistance / OrbSpeed * 1000) + (int)((OrbDistance - distance) / OrbReturnSpeed * 1000);

			if (caster is Summon summon && summon.Owner != null)
				ShowRangePreview(summon.Owner, skill, splashArea, TimeSpan.FromMilliseconds(returnHit));

			_ = ForceAttackEffect(caster, target, skill);
			_ = this.ThrowOrb(caster, skill);

			await SkillAttack(caster, skill, splashArea, outHit, outHit, modifySkillHitResult: AsOwner);
			await SkillAttack(caster, skill, splashArea, returnHit - outHit, returnHit - outHit, modifySkillHitResult: AsOwner);
		}

		private async Task ThrowOrb(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(ThrowDelay);

			if (caster.IsDead)
				return;

			var start = caster.Position;

			var pad = new Pad(PadName.Onmyoji_CrystalballShikigami_Pad, caster, skill, new SplashAreas.Circle(start, OrbRadius));
			pad.Position = start;
			pad.Direction = caster.Direction;
			pad.Movement.Speed = OrbSpeed;
			caster.Map.AddPad(pad);

			var destination = caster.Map.Ground.GetLastValidPosition(start, start.GetRelative(caster.Direction, OrbDistance));
			await pad.Movement.MoveToAndDestroy(destination);
		}

		/// <summary>
		/// Returns the hit rolled as the fox's Onmyoji using Soul Fox
		/// Shikigami, or the fox's own hit if it has no such owner.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skillHitResult"></param>
		/// <returns></returns>
		private static SkillHitResult AsOwner(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult skillHitResult)
		{
			if (caster is Summon summon && summon.Owner is ICombatEntity owner && owner.TryGetSkill(SkillId.Onmyoji_FireFoxShikigami, out var ownerSkill))
				return SCR_SkillHit(owner, target, ownerSkill);

			return skillHitResult;
		}
	}
}
