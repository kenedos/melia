using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Elementalist_Hail)]
	public class Elementalist_HailOverride : ICreatePadHandler, IUpdatePadHandler
	{
		private const int FreezeDurationMs = 4000;
		private const int MaxTargetsPerProjectile = 3;
		private const float HitRange = 40f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			pad.SetRange(50f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(18000);
			pad.Trigger.MaxUseCount = 1;
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			skill.Run(this.HandleMissileFall(pad, creator, skill));
		}

		private async Task HandleMissileFall(Pad pad, ICombatEntity creator, Skill skill)
		{
			var freezeChance = (int)(30 + 3.0f * skill.Level);

			if (creator.TryGetActiveAbilityLevel(AbilityId.Elementalist4, out var abilLevel))
				freezeChance += 8 * abilLevel;

			var rnd = RandomProvider.Get();

			await this.FireProjectile(pad, creator, skill, 40f, 0f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 70f, 0.15f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 95f, 0.3f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 55f, 0.45f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 85f, 0.6f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 70f, 0.75f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 95f, 0.9f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 55f, 1.0f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 85f, 0.5f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 70f, 0.2f, freezeChance, rnd);
			await this.FireProjectile(pad, creator, skill, 85f, 1.1f, freezeChance, rnd);
		}

		private async Task FireProjectile(Pad pad, ICombatEntity creator, Skill skill, float fallRange, float delayTime, int freezeChance, Random rnd)
		{
			if (creator.IsDead || pad.IsDead)
				return;

			var fallPos = pad.Position.GetRandomInRange2D((int)fallRange, rnd);

			creator.MissileFall(skill.Data.ClassName, "I_wizard_hail_force", 0.5f, fallPos, HitRange, delayTime, 0.6f, 200f, 2f, "F_wizard_hail_hit_ice", 0.5f, 0f, "F_wizard_hail_hit_ice", 0.5f);

			var totalDelayTime = 0.6f + delayTime;
			var sleepMs = (int)(totalDelayTime * 1000);
			if (sleepMs > 0)
				await skill.Wait(TimeSpan.FromMilliseconds(sleepMs));

			if (creator.IsDead || pad.IsDead)
				return;

			var targets = creator.Map.GetAttackableEnemiesIn(creator, new CircleF(fallPos, HitRange))
				.Where(t => !t.IsDead)
				.Take(MaxTargetsPerProjectile)
				.ToList();

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(creator, target, skill);
				target.TakeDamage(skillHitResult.Damage, creator);

				var skillHit = new SkillHitInfo(creator, target, skill, skillHitResult);
				Send.ZC_SKILL_HIT_INFO(creator, skillHit);

				if (rnd.Next(100) < freezeChance && !target.IsBuffActive(BuffId.Freeze))
					target.StartBuff(BuffId.Freeze, skill.Level, 0, TimeSpan.FromMilliseconds(FreezeDurationMs), creator);
			}
		}
	}
}

