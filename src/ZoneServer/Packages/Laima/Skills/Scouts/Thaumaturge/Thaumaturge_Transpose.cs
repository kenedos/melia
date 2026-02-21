using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[SkillHandler(SkillId.Thaumaturge_Transpose)]
	public class Thaumaturge_TransposeOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster.IsBuffActive(BuffId.Transpose_Buff))
			{
				caster.StopBuff(BuffId.Transpose_Buff);
			}
			else
			{
				if (!caster.TrySpendSp(skill))
				{
					caster.ServerMessage(Localization.Get("Not enough SP."));
					return;
				}

				var buffDuration = TimeSpan.FromMinutes(5);
				caster.StartBuff(BuffId.Transpose_Buff, skill.Level, 0f, buffDuration, caster);
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
		}
	}
}
