using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Indulgentia.
	/// Applies the Indulgentia buff to nearby allies. While the buff is active,
	/// targets recover HP in regular intervals.
	/// The amount of HP recovered increases by 10% when the Guardian Saint buff is active.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_Indulgentia)]
	public class Pardoner_IndulgentiaOverride : IGroundSkillHandler
	{
		private const float TargetRange = 150f;

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
			var buffDuration = skill.Properties.CaptionTime;
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			var skillTargets = SkillSelectAlliesInCircle(caster, caster.Position, TargetRange, maxTargets);
			if (!skillTargets.Contains(caster))
				skillTargets.Add(caster);

			foreach (var target in skillTargets)
				target.StartBuff(BuffId.Indulgentia_Buff, skill.Level, 0f, buffDuration, caster, skill.Id);

			await skill.Wait(TimeSpan.FromMilliseconds(110));

			foreach (var target in skillTargets)
				this.RemoveDebuffsFromTarget(target);
		}

		/// <summary>
		/// Removes removable debuffs from the target.
		/// </summary>
		/// <param name="target"></param>
		private void RemoveDebuffsFromTarget(ICombatEntity target)
		{
			var debuffsToRemove = new List<BuffId>();

			foreach (var buff in target.Components.Get<BuffComponent>()
				.GetAll(buff => buff.Data.Type == BuffType.Debuff && buff.Data.Removable))
			{
				debuffsToRemove.Add(buff.Id);
			}

			foreach (var buffId in debuffsToRemove)
				target.RemoveBuff(buffId);
		}
	}
}
