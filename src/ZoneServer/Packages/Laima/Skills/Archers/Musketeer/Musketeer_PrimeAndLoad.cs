using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Prime And Load.
	/// Resets the cooldown of the Musketeer's attack skills and clears the
	/// Sniper Exposed stacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_PrimeAndLoad)]
	public class Musketeer_PrimeAndLoadOverride : ISelfSkillHandler
	{
		private static readonly SkillId[] ResetSkillIds = [SkillId.Musketeer_CoveringFire, SkillId.Musketeer_PenetrationShot, SkillId.Musketeer_Volleyfire];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var farPos = new Position(originPos);
			farPos.X += 100;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			var cooldowns = caster.Components.Get<CooldownComponent>();

			foreach (var skillId in ResetSkillIds)
			{
				if (caster.TryGetSkill(skillId, out var resetSkill))
					cooldowns.Remove(resetSkill.Data.CooldownGroup);
			}

			caster.StopBuff(BuffId.Musketeer_Snipe_UseStack_Buff);
		}
	}
}
