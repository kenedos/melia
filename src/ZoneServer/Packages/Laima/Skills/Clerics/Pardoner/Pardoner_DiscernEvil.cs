using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Discern Evil.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_DiscernEvil)]
	public class Pardoner_DiscernEvilOverride : IGroundSkillHandler
	{
		private const float TargetDistance = 40f;
		private const float TargetRadius = 30f;
		private const float BuffDurationMs = 10000f;
		private const float ExtensionBase = 25f;
		private const float ExtensionPerLevel = 5f;
		private static readonly TimeSpan MinExtension = TimeSpan.FromSeconds(5);
		private const string VarExtended = "Melia.Skill.DiscernEvil.Extended";

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
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var targetPos = originPos.GetRelative(farPos, TargetDistance);
			var maxTargets = 1 + caster.GetAbilityLevel(AbilityId.Pardoner2);

			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, TargetRadius, maxTargets));

			var skillTargets = caster.GetTargets();

			foreach (var skillTarget in skillTargets)
				this.ExtendDebuffs(skillTarget, skill.Level);

			SkillTargetBuff(skill, caster, skillTargets, BuffId.DiscernEvil_Buff, skill.Level, skill.Level, TimeSpan.FromMilliseconds(BuffDurationMs), skill.Id);
		}

		/// <summary>
		/// Extends the remaining duration of the target's debuffs.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="skillLevel"></param>
		private void ExtendDebuffs(ICombatEntity target, int skillLevel)
		{
			var rate = (ExtensionBase + ExtensionPerLevel * skillLevel) / 100f;

			foreach (var buff in target.Components.Get<BuffComponent>().GetAll(a => a.Data.Type == BuffType.Debuff && a.HasDuration))
			{
				if (buff.Vars.GetBool(VarExtended))
					continue;

				var extension = buff.RemainingDuration * rate;
				if (extension < MinExtension)
					extension = MinExtension;

				buff.IncreaseDuration(extension);
				buff.Vars.SetBool(VarExtended, true);
			}
		}
	}
}
