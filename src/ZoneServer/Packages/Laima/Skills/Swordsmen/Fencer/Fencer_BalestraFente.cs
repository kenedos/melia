using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Balestra Fente.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_BalestraFente)]
	public class Fencer_BalestraFenteOverride : IGroundSkillHandler
	{
		private const float DashDistance = 80f;
		private const float TargetDistance = 120f;
		private const float TargetWidth = 25f;
		private const int MaxTargets = 10;
		private const int SelectDelay = 300;
		private const int DamageDelay = 100;

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

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			var startPos = caster.Position;

			await skill.Wait(TimeSpan.FromMilliseconds(SelectDelay));

			var maxTargets = skill.GetPVPValue(MaxTargets);
			var skillTargets = SkillSelectEnemiesInSquare(caster, startPos, 0f, TargetDistance, TargetWidth, maxTargets);

			await skill.Wait(TimeSpan.FromMilliseconds(DamageDelay));

			if (skillTargets.Count > 0)
				SkillTargetDamage(skill, caster, skillTargets);

			var dashPos = caster.Position.GetRelative(caster.Direction, DashDistance);
			if (!caster.Map.Ground.TryGetNearestValidPosition(dashPos, out var validPosition))
				return;

			caster.Position = validPosition;
			Send.ZC_MOVE_STOP(caster, validPosition, 1);
		}
	}
}
