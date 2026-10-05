using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Handler for Hamaya's Holy damage area.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Miko_Hamaya)]
	public class Miko_HamayaPadOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const string HitCountVariable = "Melia.Miko.Hamaya.HitCount";
		private const int MaximumHits = 10;
		private const int MaximumTargets = 10;
		private const float PadRange = 35f;
		private const float EnhancePerLevel = 0.005f;
		private static readonly TimeSpan PadDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(2);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(300);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);

			pad.SetRange(PadRange);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = PadDuration;
			pad.Variables.SetInt(HitCountVariable, 0);

			this.AttackTargets(pad, args.Creator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var hitCount = pad.Variables.GetInt(HitCountVariable);

			if (hitCount >= MaximumHits)
			{
				pad.Destroy();
				return;
			}

			this.AttackTargets(pad, args.Creator);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		private void AttackTargets(Pad pad, ICombatEntity creator)
		{
			var hitCount = pad.Variables.GetInt(HitCountVariable);

			if (hitCount >= MaximumHits)
				return;

			var skill = pad.Skill;
			var targets = creator.Map.GetAttackableEnemiesIn(creator, pad.Trigger.Area).Where(target => target != null && !target.IsDead).OrderBy(target => pad.Position.Get2DDistance(target.Position)).Take(MaximumTargets).ToList();

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(creator, target, skill);

				this.ApplyEnhanceAbility(creator, skillHitResult);

				target.TakeDamage(skillHitResult.Damage, creator);

				target.StartBuff(BuffId.Hamaya_TakeDamage, skill.Level, 0f, DebuffDuration, creator, skill.Id);

				var skillHit = new SkillHitInfo(creator, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
				Send.ZC_SKILL_FORCE_TARGET(creator, target, skill, skillHit);
			}

			pad.Variables.SetInt(HitCountVariable, hitCount + 1);
		}

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.Miko3, out var ability) || !ability.Active)
				return;

			var abilityLevel = Math.Min(ability.Level, 100);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
