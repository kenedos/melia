using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Cryomancer
{
	/// <summary>
	/// Handler for the Cryomancer skill Ice Blast.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cryomancer_IceBlast)]
	public class Cryomancer_IceBlastOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int DebuffDurationSeconds = 20;
		private const int DebuffUpdateTimeMilliseconds = 1000;

		/// <summary>
		/// Start casting.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(450));

			var targetList = SkillSelectEnemiesInCircle(caster, caster.Position, 250);
			foreach (var currentTarget in targetList)
			{
				if (!currentTarget.TryGetBuff(BuffId.Cryomancer_Freeze, out var freezeBuff))
					continue;

				// Don't stack same debuff
				if (currentTarget.TryGetBuff(BuffId.IceBlast_Debuff, out var iceBlastBuff))
					continue;

				var skillHitResult = SCR_SkillHit(caster, currentTarget, skill);

				var debuff = currentTarget.StartBuff(BuffId.IceBlast_Debuff, skill.Level, skillHitResult.Damage, TimeSpan.FromSeconds(DebuffDurationSeconds), caster);
				debuff?.SetUpdateTime(DebuffUpdateTimeMilliseconds);
			}
		}
	}
}
