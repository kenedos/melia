using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Rogue skill Evasion.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rogue_Evasion)]
	public class Rogue_EvasionOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.HandleSkill(skill, caster));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			caster.StartBuff(BuffId.Evasion_Buff, skill.Level, 0f, TimeSpan.FromSeconds(6), caster);
			caster.StartBuff(BuffId.Sprint_Buff, skill.Level, 0f, TimeSpan.FromSeconds(6), caster);
		}
	}
}
