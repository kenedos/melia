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
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Plague Doctor skill Incineration, which sets enemies
	/// suffering from debuffs on fire, burning them longer and harder for
	/// every debuff they carry.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PlagueDoctor_Incineration)]
	public class PlagueDoctor_IncinerationOverride : IGroundSkillHandler
	{
		private const float PadDistance = 35f;
		private const float PadRange = 80f;
		private const float DamageBonusPerDebuff = 0.10f;
		private const float MaxDebuffDamageBonus = 0.50f;
		private const float SteamDamageBonusPerLevel = 0.05f;
		private static readonly TimeSpan IgniteDelay = TimeSpan.FromMilliseconds(700);
		private static readonly TimeSpan PadLifeTime = TimeSpan.FromSeconds(1);

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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Ignite(skill, caster));
		}

		/// <summary>
		/// Sets the burning ground in front of the caster and ignites the
		/// debuffed enemies on it.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Ignite(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(IgniteDelay);

			if (caster.IsDead)
				return;

			var position = caster.Position.GetRelative(caster.Direction, PadDistance);

			var pad = new Pad(PadName.PlagueDoctor_Incineration, caster, skill, new Circle(position, PadRange));
			pad.Position = position;
			pad.Trigger.LifeTime = PadLifeTime;
			caster.Map.AddPad(pad);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, position, PadRange)
				.Where(a => PlagueDoctorSkillHelper.CountDebuffs(a, BuffId.Incineration_Debuff) > 0)
				.Take(maxTargets);

			foreach (var target in targets)
				this.ApplyIncineration(skill, caster, target);
		}

		/// <summary>
		/// Starts Incineration on the target, its damage and duration raised
		/// by the debuffs the target carries.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		private void ApplyIncineration(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var debuffCount = PlagueDoctorSkillHelper.CountDebuffs(target, BuffId.Incineration_Debuff);

			var modifier = new SkillModifier();
			modifier.FinalDamageMultiplier += Math.Min(MaxDebuffDamageBonus, debuffCount * DamageBonusPerDebuff);

			if (target.TryGetBuff(BuffId.PlagueVapours_Debuff, out var steam))
				modifier.FinalDamageMultiplier += steam.NumArg1 * SteamDamageBonusPerLevel;

			var damage = SCR_SkillHit(caster, target, skill, modifier).Damage;
			if (damage <= 0)
				return;

			var duration = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio2) + debuffCount);
			target.StartBuff(BuffId.Incineration_Debuff, skill.Level, damage, duration, caster, skill.Id);
		}
	}
}
