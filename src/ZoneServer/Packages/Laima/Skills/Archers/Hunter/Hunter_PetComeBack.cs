using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Pet Come Back.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_PetComeBack)]
	public class Hunter_PetComeBackOverride : IMeleeGroundSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			if (companion.Components.TryGet<AiComponent>(out var ai))
				ai.Script.QueueEventAlert(new HateResetAlert());

			var targetPosition = caster.Position.GetRandomInRange2D(15, 30);
			var map = companion.Map;

			if (map?.Ground != null && map.Ground.TryGetNearestValidPosition(targetPosition, 10f, out var validPosition))
			{
				companion.SetPosition(validPosition);
			}
			else
			{
				companion.SetPosition(caster.Position);
			}

			companion.PlayGroundEffect("F_buff_basic008_blue", 1);

			// Hunter23: Increases Pet_Heal duration to 12 seconds
			var healDuration = caster.IsAbilityActive(AbilityId.Hunter23) ? 12 : 8;
			companion.StartBuff(BuffId.Pet_Heal, skill.Level, 0, TimeSpan.FromSeconds(healDuration), caster);

			// Hunter22: Cure companion of all negative debuffs
			if (caster.IsAbilityActive(AbilityId.Hunter22))
				this.RemoveDebuffs(companion);
		}

		/// <summary>
		/// Removes all debuffs from the target.
		/// </summary>
		private void RemoveDebuffs(ICombatEntity target)
		{
			var buffComponent = target.Components.Get<BuffComponent>();
			var buffs = buffComponent.GetList();

			foreach (var buff in buffs)
			{
				if (buff.Data.Type == BuffType.Debuff)
					buffComponent.Remove(buff.Id);
			}
		}
	}
}
