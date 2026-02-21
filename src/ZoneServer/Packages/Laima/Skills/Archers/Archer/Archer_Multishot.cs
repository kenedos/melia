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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archer
{
	/// <summary>
	/// Handler for the Archer skill Multishot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Archer_Multishot)]
	public class Archer_MultishotOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float SplashRadius = 30;
		private const int TotalHits = 10;
		private readonly static TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(120);

		/// <summary>
		/// Called when the user starts casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_archer_multishot_cast", "voice_archer_m_multishot_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_archer_multishot_cast", "voice_archer_m_multishot_cast");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
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

			var splashArea = new Circle(targetPos, SplashRadius);
			this.Attack(skill, caster, splashArea);
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async void Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			try
			{
				var hits = new List<SkillHitInfo>();

				for (var i = 0; i < TotalHits; ++i)
				{
					// TODO: Try to optimize this to not get all targets
					//   every time, though we might want/need that?
					var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
					var targetPos = splashArea.OriginPos;

					if (targets.Count != 0)
					{
						var target = targets.Random();
						if (!caster.CanAttack(target))
							continue;

						var skillHitResult = SCR_SkillHit(caster, target, skill);

						target.TakeDamage(skillHitResult.Damage, caster);
						targetPos = target.Position;

						var hit = new HitInfo(caster, target, skill, skillHitResult.Damage, skillHitResult.Result);
						Send.ZC_HIT_INFO(caster, target, hit);
					}

					// It seems like the game uses ZC_SYNC_* packets
					// to control how and when packets such as this
					// are actually handled by the client instead of
					// sending them on precise timers, though timers
					// also seem to get employed... Needs more research.
					Send.ZC_NORMAL.SkillProjectile(caster, targetPos, "I_arrow013_mash_yellow#Dummy_Force", 0.6f, "F_explosion092_hit", 0.6f, 30, TimeSpan.FromSeconds(0.2f));

					if (i < TotalHits - 1)
						await skill.Wait(DelayBetweenHits);
				}

				Send.ZC_SKILL_DISABLE(caster);
			}
			catch (Exception ex)
			{
				Log.Error("Multishot: Attack failed. Error: {0}", ex);
			}
		}
	}
}
