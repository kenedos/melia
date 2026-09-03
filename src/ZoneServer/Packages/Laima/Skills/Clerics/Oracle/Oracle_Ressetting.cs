using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Ressetting.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_Ressetting)]
	public class Oracle_RessettingOverride : IGroundSkillHandler
	{
		private const string GroundEffectName = "F_cleric_Ressetting_ground";
		private const float GroundEffectDuration = 3000;
		private const float CenterDistance = 35f;
		private const float Radius = 70f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var centerPos = caster.Position.GetRelative(caster.Direction, CenterDistance);
			var targets = SkillSelectEnemiesInCircle(caster, centerPos, Radius, OracleSkillHelper.GetTargetCount(skill));

			if (caster is not Character character)
				return;

			OracleSkillHelper.HideDropPreviews(character);

			foreach (var skillTarget in targets)
			{
				if (skillTarget is not Mob monster)
					continue;

				var dropStacks = monster.RerollDrops(character);
				OracleSkillHelper.ShowDropPreview(character, monster, dropStacks);

				await caster.PlayEffectToGround(GroundEffectName, monster.Position, 1f, GroundEffectDuration);
			}
		}
	}
}
