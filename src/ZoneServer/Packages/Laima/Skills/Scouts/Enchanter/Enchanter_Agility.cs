using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Enchanter
{
	[Package("laima")]
	[SkillHandler(SkillId.Enchanter_Agility)]
	public class Enchanter_AgilityOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private static readonly TimeSpan Duration = TimeSpan.FromMinutes(5);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var targets = new List<ICombatEntity>();

			if (target != null)
				targets.Add(target);

			this.Cast(skill, caster, originPos, farPos, targets);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.ApplyBuffs(caster, skill, targets));
		}

		private async Task ApplyBuffs(ICombatEntity caster, Skill skill, IList<ICombatEntity> targets)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(350));

			this.ApplyAgilityBuff(caster, skill);

			foreach (var target in targets.Where(target => target != null && !target.IsDead))
			{
				if (target == caster)
					continue;

				this.ApplyAgilityBuff(target, skill);
			}

			caster.SetAttackState(false);
		}

		private void ApplyAgilityBuff(ICombatEntity target, Skill skill)
		{
			target.StartBuff(
				BuffId.Agility_Buff,
				skill.Level,
				0f,
				Duration,
				skill.Owner,
				skill.Id);
		}
	}
}
