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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Plague Doctor skill Black Death Steam, which poisons
	/// the enemies in front of the caster and lowers their critical
	/// resistance.
	/// </summary>
	/// <remarks>
	/// While wearing the White Mask, the caster also puffs poison around
	/// themselves for 7 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PlagueDoctor_PlagueVapours)]
	public class PlagueDoctor_PlagueVapoursOverride : IGroundSkillHandler
	{
		private const float PadDistance = 60f;
		private const float PadRange = 80f;
		private static readonly TimeSpan SteamDelay = TimeSpan.FromMilliseconds(500);
		private static readonly TimeSpan PadLifeTime = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan WhiteMaskDuration = TimeSpan.FromSeconds(7);

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

			if (caster.IsBuffActive(BuffId.WhiteBeakMask_Buff))
				caster.StartBuff(BuffId.WhiteBeakMask_Damage_Buff, skill.Level, 0, WhiteMaskDuration, caster, skill.Id);

			skill.Run(this.Spread(skill, caster));
		}

		/// <summary>
		/// Releases the steam in front of the caster and poisons the enemies
		/// in it.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Spread(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(SteamDelay);

			if (caster.IsDead)
				return;

			var position = caster.Position.GetRelative(caster.Direction, PadDistance);

			var pad = new Pad(PadName.PlagueDoctor_PlagueVapours, caster, skill, new Circle(position, PadRange));
			pad.Position = position;
			pad.Trigger.LifeTime = PadLifeTime;
			caster.Map.AddPad(pad);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			var duration = skill.Properties.CaptionTime;

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, position, PadRange).Take(maxTargets))
			{
				var damage = SCR_SkillHit(caster, target, skill).Damage;
				if (damage <= 0)
					continue;

				target.StartBuff(BuffId.PlagueVapours_Debuff, skill.Level, damage, duration, caster, skill.Id);
				target.StartBuff(BuffId.PlagueVapours_Crtdr_Debuff, skill.Level, 0, duration, caster, skill.Id);
			}
		}
	}
}
