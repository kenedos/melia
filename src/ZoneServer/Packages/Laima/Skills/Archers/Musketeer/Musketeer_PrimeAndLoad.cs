using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Prime And Load.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_PrimeAndLoad)]
	public class Musketeer_PrimeAndLoadOverride : ISelfSkillHandler, IDynamicCasted
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

			var farPos = originPos.GetRelative(dir, 100f);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			this.ReloadMusketeerSkills(caster);
		}

		private void ReloadMusketeerSkills(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			foreach (var targetSkill in character.Skills.GetList())
			{
				if (targetSkill.Id == SkillId.Musketeer_PrimeAndLoad)
					continue;

				if (!targetSkill.Data.Tags.Has(SkillTag.UseMusketSkill))
					continue;

				targetSkill.StartCooldown(TimeSpan.Zero);
			}

			character.StopBuff(BuffId.Musketeer_Snipe_UseStack_Buff);
		}
	}
}
