using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Howling.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_Howling)]
	public class Hunter_HowlingOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TryGetActiveCompanion(out var companion))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, companion));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Companion companion)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			companion.PlayEffect("F_archer_howling_symbol", scale: 0.25f, heightOffset: EffectLocation.Top);
			companion.PlayEffect("F_archer_howling_ground");
			var targets = caster.Map.GetAttackableEnemiesInPosition(companion, companion.Position, 150)
				.Take(10);
			foreach (var target in targets)
			{
				target.StartBuff(BuffId.Howling_Debuff, skill.Level, 0, TimeSpan.FromSeconds(15), companion);
			}
		}
	}
}
