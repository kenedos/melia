using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for the Miko skill Kagura, a dance held for up to 15 seconds
	/// that blesses the allies around the Miko when it ends, more the longer
	/// it lasted.
	/// </summary>
	/// <remarks>
	/// Kagura: Ken turns it into a 5 second dance that strikes the enemies
	/// around the Miko every second and once more at the end, harder the
	/// longer it lasted.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Miko_KaguraDance)]
	public class Miko_KaguraDanceOverride : IDynamicCasted
	{
		private const float BlessRange = 130f;
		private const float StrikeRange = 100f;
		private const string StartVar = "Melia.Miko.KaguraStart";
		private const string PadVar = "Melia.Miko.KaguraPad";
		private static readonly TimeSpan BlessingDuration = TimeSpan.FromSeconds(15);
		private static readonly TimeSpan WeakenDuration = TimeSpan.FromSeconds(15);
		private static readonly TimeSpan StrikeInterval = TimeSpan.FromSeconds(1);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var duration = skill.Properties.CaptionTime;
			caster.StartBuff(BuffId.Skill_SuperArmor_Buff, duration);

			skill.Vars.Set(StartVar, GameClock.LocalNow);

			if (caster.IsAbilityActive(AbilityId.Miko18))
				Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);

			var pad = new Pad(PadName.Miko_KaguraDance, caster, skill, new Circle(caster.Position, BlessRange));
			pad.Position = caster.Position;
			pad.Trigger.LifeTime = duration;
			pad.FollowsTarget(caster);
			caster.Map.AddPad(pad);
			skill.Vars.Set(PadVar, pad);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Miko8, out var nightingaleLevel))
			{
				foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, BlessRange))
					enemy.StartBuff(BuffId.Kagura_Crtdr_Debuff, skill.Level, nightingaleLevel, WeakenDuration, caster, skill.Id);
			}

			if (caster.IsAbilityActive(AbilityId.Miko18))
				skill.Run(this.Strike(skill, caster));
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.Skill_SuperArmor_Buff);

			if (skill.Vars.TryGet<Pad>(PadVar, out var pad))
				pad.Destroy();

			if (!skill.Vars.TryGet<DateTime>(StartVar, out var start))
				return;

			skill.Vars.Remove(StartVar);

			var elapsed = GameClock.LocalNow - start;
			var progress = Math.Min(1f, (float)(elapsed / skill.Properties.CaptionTime));

			if (caster.IsAbilityActive(AbilityId.Miko18))
			{
				this.Hit(skill, caster, Math.Max(1, (int)elapsed.TotalSeconds));
				return;
			}

			var minBonus = skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var maxBonus = skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			var bonus = minBonus + (maxBonus - minBonus) * progress;

			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, BlessRange))
				ally.StartBuff(BuffId.KaguraDance_Buff, skill.Level, bonus, BlessingDuration, caster, skill.Id);
		}

		/// <summary>
		/// Strikes the enemies around the caster every second while they
		/// dance.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Strike(Skill skill, ICombatEntity caster)
		{
			while (true)
			{
				await skill.Wait(StrikeInterval);

				if (caster.IsDead || !caster.IsCasting(skill))
					return;

				this.Hit(skill, caster, 1);
			}
		}

		/// <summary>
		/// Strikes the enemies around the caster with the given number of
		/// hits.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="hitCount"></param>
		private void Hit(Skill skill, ICombatEntity caster, int hitCount)
		{
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio3);
			var hits = new List<SkillHitInfo>();

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, StrikeRange).Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(hitCount));
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
