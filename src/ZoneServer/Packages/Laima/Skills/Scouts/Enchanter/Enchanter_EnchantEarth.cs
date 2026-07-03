using System;
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
	[SkillHandler(SkillId.Enchanter_EnchantEarth)]
	public class Enchanter_EnchantEarthOverride : ISelfSkillHandler
	{
		private static readonly TimeSpan Duration = TimeSpan.FromMinutes(5);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.ApplyBuff(caster, skill));
		}

		private async Task ApplyBuff(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(350));

			caster.StartBuff(
				BuffId.EnchantEarth_Buff,
				skill.Level,
				0f,
				Duration,
				caster,
				skill.Id);

			caster.SetAttackState(false);
		}
	}
}
