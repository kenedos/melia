using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.QuarrelShooter
{
	/// <summary>
	/// Handler for the QuarrelShooter skill Rapid Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.QuarrelShooter_RapidFire)]
	public class QuarrelShooterRapidFire : IForceSkillHandler, IDynamicCasted
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(300);

		/// <summary>
		/// Start casting.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the Rapid Fire skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if (!caster.InSkillUseRange(skill, farPos))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 0, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			await this.CreateExplosionEffects(skill, caster, targetPos);
			await this.ExecuteRapidFire(skill, caster, targetPos);
		}

		/// <summary>
		/// Executes the rapid fire attacks.
		/// </summary>
		private Task ExecuteRapidFire(Skill skill, ICombatEntity caster, Position targetPos)
		{
			var attackArea = new CircleF(targetPos, 60);
			var attackDelays = new[] { 100f, 180f, 260f, 340f, 420f };
			var damageDelays = new[] { 300f, 380f, 460f, 540f, 620f };

			for (var i = 0; i < 5; i++)
			{
				var hits = new List<SkillHitInfo>();
				_ = SkillAttack(caster, skill, attackArea, attackDelays[i], damageDelays[i], hits);
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Creates explosion effects at the target location.
		/// </summary>
		private async Task CreateExplosionEffects(Skill skill, ICombatEntity caster, Position targetPos)
		{
			var explosionEffects = new[]
			{
				("F_archer_caltrop_hit_explosion", 0.5f),
				("F_archer_caltrop_hit_explosion", 0.7f),
				("F_archer_caltrop_hit_explosion", 0.6f),
				("F_archer_caltrop_hit_explosion", 0.8f),
				("F_archer_caltrop_hit_explosion", 1.2f),
				("F_explosion097", 1f)
			};

			foreach (var (effect, scale) in explosionEffects)
			{
				var position = targetPos.GetRandomInRange2D(1, 20);
				Send.ZC_GROUND_EFFECT(caster, position, effect, scale, 0f, 0f, 0f);
				await skill.Wait(TimeSpan.FromMilliseconds(20));
			}
		}
	}
}
