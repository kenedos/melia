using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Monk
{
	/// <summary>
	/// Handler for the Monk skill Double Punch.
	/// Channeled skill that repeatedly punches enemies in front of
	/// the caster every 300ms (2 hits per cycle) while held.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Monk_DoublePunch)]
	public class Monk_DoublePunchOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var endTime = DateTime.Now.AddMilliseconds(3500);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			await skill.Wait(TimeSpan.FromMilliseconds(200));

			while (caster.IsCasting(skill))
			{
				if (endTime <= DateTime.Now)
					break;

				if (!caster.TrySpendSp(skill))
				{
					caster.ServerMessage(Localization.Get("Not enough SP."));
					break;
				}

				await this.Attack(caster, skill);

				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}

			Send.ZC_SKILL_DISABLE(caster);
		}

		private async Task Attack(ICombatEntity caster, Skill skill)
		{
			var targetArea = caster.Position.GetRelative(caster.Direction, 30);
			var casterPos = caster.Position;

			var splashParam = skill.GetSplashParameters(caster, casterPos, targetArea, length: 25, width: 20, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			await SkillAttack(caster, skill, splashArea, 30, 50);

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			splashParam = skill.GetSplashParameters(caster, casterPos, targetArea, length: 25, width: 20, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			await SkillAttack(caster, skill, splashArea, 30, 80);

			caster.StartBuff(BuffId.DoublePunch_Buff, skill.Level, 0, TimeSpan.FromSeconds(10), caster);
		}
	}
}
