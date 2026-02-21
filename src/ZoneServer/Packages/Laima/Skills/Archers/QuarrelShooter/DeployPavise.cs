using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.QuarrelShooter
{
	/// <summary>
	/// Handler for the QuarrelShooter skill Deploy Pavise.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.QuarrelShooter_DeployPavise)]
	public class QuarrelShooterDeployPavise : IMeleeGroundSkillHandler
	{
		private const float PaviseDurationSeconds = 30f;
		private const float PaviseBaseHealth = 500f;
		private const float PaviseHealthBonusPerLevel = 1.5f;
		private const float PaviseBaseBlock = 100f;
		private const float PaviseBlockBonusPerLevel = 0.5f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!this.CanUseSkill(caster, skill))
				return;

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			farPos = caster.Position.GetRelative(caster.Direction, 10);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var direction = caster.Direction;
			var time = this.GetPaviseDuration(caster);
			var paviseHealth = this.CalculatePaviseHealth(caster, skill);
			var paviseBlock = this.CalculatePaviseBlock(caster, skill);

			this.DeployPavises(caster, skill, farPos, direction, time, paviseHealth, paviseBlock);
		}

		private bool CanUseSkill(ICombatEntity caster, Skill skill)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return false;
			}

			if (caster.Map.IsCity)
			{
				caster.ServerMessage(Localization.Get("Cannot use this skill in cities."));
				return false;
			}

			return true;
		}

		private float GetPaviseDuration(ICombatEntity caster)
		{
			var time = PaviseDurationSeconds;
			if (caster.IsAbilityActive(AbilityId.QuarrelShooter24))
				time *= 0.5f;
			return time;
		}

		private float CalculatePaviseHealth(ICombatEntity caster, Skill skill)
		{
			return (float)Math.Max(0, PaviseBaseHealth + (caster.Properties.GetFloat("DEF") * PaviseHealthBonusPerLevel * skill.Level));
		}

		private float CalculatePaviseBlock(ICombatEntity caster, Skill skill)
		{
			return (float)Math.Max(0, PaviseBaseBlock + (caster.Properties.GetFloat("BLK") * PaviseBlockBonusPerLevel * skill.Level));
		}

		private void DeployPavises(ICombatEntity caster, Skill skill, Position centerPos, Direction direction, float time, float paviseHealth, float paviseBlock)
		{
			var angle = 45f;
			var leftPos = centerPos.GetRelative(direction.Left, 15);
			var pavisePos = leftPos.GetRelative(direction, 5);
			this.CreatePaviseMob(caster, skill, pavisePos, angle, time, paviseHealth, paviseBlock);

			angle = 0f;
			pavisePos = centerPos.GetRelative(direction, 15);
			this.CreatePaviseMob(caster, skill, pavisePos, angle, time, paviseHealth, paviseBlock);

			angle = -45f;
			var rightPos = centerPos.GetRelative(direction.Right, 15);
			pavisePos = rightPos.GetRelative(direction, 5);
			this.CreatePaviseMob(caster, skill, pavisePos, angle, time, paviseHealth, paviseBlock);
		}

		private void CreatePaviseMob(ICombatEntity caster, Skill skill, Position position, float angle, float time, float paviseHealth, float paviseBlock)
		{
			var paviseMob = MonsterSkillCreateMob(skill, caster, "pavise", position, angle, "", "", 1 + skill.Level, time, "None", "");
			if (paviseMob != null)
			{
				paviseMob.Properties.Modify(PropertyName.SDR_BM, 9999);
				paviseMob.Properties.Modify(PropertyName.CRTDR_BM, 9999);
				paviseMob.Properties.Modify(PropertyName.MHP_BM, paviseHealth);
				paviseMob.Properties.Modify(PropertyName.BLK_BM, paviseBlock);
				paviseMob.Heal(paviseHealth, 0);
			}
		}
	}
}
