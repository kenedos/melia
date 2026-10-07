using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Plague Doctor skill Fumigate, which sprays an
	/// antidote that cures allies of up to 3 removable debuffs each.
	/// </summary>
	/// <remarks>
	/// Fumigate: Perfusion replaces the cure with a 10 second vapor around
	/// the caster that damages enemies and strips their buffs.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PlagueDoctor_Fumigate)]
	public class PlagueDoctor_FumigateOverride : IGroundSkillHandler
	{
		private const float SprayDistance = 30f;
		private const float PurificationDistance = 50f;
		private const float SprayRange = 40f;
		private const float PurificationRange = 80f;
		private const int MaxCuredDebuffs = 3;
		private const float SanitizeHealRate = 0.10f;
		private static readonly TimeSpan SprayDelay = TimeSpan.FromMilliseconds(700);
		private static readonly TimeSpan CureEffectDuration = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan PurificationDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan PerfusionDuration = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			if (caster.IsAbilityActive(AbilityId.PlagueDoctor29))
			{
				caster.StartBuff(BuffId.Fumigate_Buff, skill.Level, 1, PerfusionDuration, caster, skill.Id);
				return;
			}

			skill.Run(this.Spray(skill, caster));
		}

		/// <summary>
		/// Sprays the antidote in front of the caster, curing the allies in
		/// it and, with Fumigate: Purification, leaving a purifying area.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Spray(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(SprayDelay);

			if (caster.IsDead)
				return;

			var hasPurification = caster.IsAbilityActive(AbilityId.PlagueDoctor6);
			var position = caster.Position.GetRelative(caster.Direction, hasPurification ? PurificationDistance : SprayDistance);
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, position, SprayRange).Take(maxTargets))
				this.Cure(skill, caster, ally);

			if (!hasPurification)
				return;

			var pad = new Pad(PadName.PlagueDoctor_Fumigate_abil, caster, skill, new Circle(position, PurificationRange));
			pad.Position = position;
			pad.Trigger.LifeTime = PurificationDuration;
			caster.Map.AddPad(pad);
		}

		/// <summary>
		/// Removes up to 3 curable debuffs from the ally, healing them for
		/// each one with Fumigate: Sanitize.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="ally"></param>
		private void Cure(Skill skill, ICombatEntity caster, ICombatEntity ally)
		{
			ally.StartBuff(BuffId.Fumigate_Buff, skill.Level, 0, CureEffectDuration, caster, skill.Id);

			if (!ally.Components.TryGet<BuffComponent>(out var buffComponent))
				return;

			var debuffs = buffComponent.GetList().Where(a => PlagueDoctorSkillHelper.IsCurableDebuff(a.Id)).Take(MaxCuredDebuffs).ToList();
			foreach (var debuff in debuffs)
				buffComponent.Remove(debuff.Id);

			if (debuffs.Count > 0 && caster.IsAbilityActive(AbilityId.PlagueDoctor22))
				ally.Heal(ally.MaxHp * SanitizeHealRate * debuffs.Count, 0);
		}
	}
}
