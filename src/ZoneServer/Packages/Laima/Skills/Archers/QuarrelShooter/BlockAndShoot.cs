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
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.QuarrelShooter
{
	/// <summary>
	/// Handler for the QuarrelShooter skill Block And Shoot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.QuarrelShooter_BlockAndShoot)]
	public class QuarrelShooterBlockAndShootOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float BaseBlockBonus = 100f;
		private const float BlockBonusPerLevel = 10f;
		private const float PdefBlockBonusPercentPerLevel = 0.02f;

		private bool _isCasting;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			var blockBonus = this.CalculateBlockBonus(skill, caster);
			caster.StartBuff(BuffId.BlockAndShoot_Buff, skill.Level, blockBonus, TimeSpan.Zero, caster);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
			_isCasting = true;
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.RemoveBuff(BuffId.BlockAndShoot_Buff);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
			_isCasting = false;
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

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			while (_isCasting)
			{
				var position = caster.Position.GetRelative(caster.Direction, 30);
				_ = MonsterSkillPadFrontMissile(caster, skill, position, PadName.QuarrelShooter_BlockAndShoot, 300f, 1, 200f, 0f, 0f, 0);
				await skill.Wait(TimeSpan.FromMilliseconds(500));
			}
		}

		private float CalculateBlockBonus(Skill skill, ICombatEntity caster)
		{
			var baseBonus = BaseBlockBonus + (skill.Level * BlockBonusPerLevel);
			var pdefBonus = caster.Properties.GetFloat(PropertyName.DEF) * PdefBlockBonusPercentPerLevel * skill.Level;
			return (float)Math.Ceiling(baseBonus + pdefBonus);
		}
	}
}
