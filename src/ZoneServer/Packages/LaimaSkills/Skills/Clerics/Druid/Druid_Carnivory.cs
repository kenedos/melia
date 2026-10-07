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
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid skill Carnivory, which plants carnivorous
	/// plants on up to 6 enemies around the Druid that eat at them every
	/// second for 10 seconds.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Druid_Carnivory)]
	public class Druid_CarnivoryOverride : IGroundSkillHandler
	{
		private const float Range = 100f;
		private const int MaxTargets = 6;
		private static readonly TimeSpan PlantDelay = TimeSpan.FromMilliseconds(600);
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(10);

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

			skill.Run(this.Plant(skill, caster));
		}

		/// <summary>
		/// Plants the carnivorous plants on the enemies around the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Plant(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(PlantDelay);

			if (caster.IsDead)
				return;

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, Range).Take(MaxTargets))
			{
				var damage = SCR_SkillHit(caster, target, skill).Damage;
				if (damage > 0)
					target.StartBuff(BuffId.Carnivory_Debuff, skill.Level, damage, Duration, caster, skill.Id);
			}
		}
	}
}
