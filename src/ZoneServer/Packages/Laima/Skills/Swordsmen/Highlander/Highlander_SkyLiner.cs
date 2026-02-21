using System;
using System.Collections.Generic;
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
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handler for the Highlander skill SkyLiner.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Highlander_SkyLiner)]
	public class Highlander_SkyLinerOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Applies bonus damage if target is bleeding
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private SkillHitResult ModifySkillHitResult(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult result)
		{
			if (target.IsBuffActiveByKeyword(BuffTag.Wound))
				result.Damage *= 2.0f;

			return result;
		}

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			// I couldn't really understand why this skill freezes the client
			// sometimes when casting while the character is taking damage.
			// The biggest culprit seems to be when ZC_SKILL_MELEE_GROUND
			// is sent in relation to ZC_SKILL_READY. If it's sent too close or
			// BEFORE the ready packet, the client freezes almost always.
			//
			// If the ZC_SKILL_MELEE_GROUND is sent a long time after
			// ZC_SKILL_READY, the freezing seems to stop. 
			//
			// The current solution adds an arbitrary delay between these two
			// packets. Perhaps one of the commented out packets are the key
			// to this mystery, but commenting them out seems to make little
			// to no difference right now.
			//
			// To replicate this issue, summon 30+ monsters and have them
			// attack you while you cast Skyliner. This also freezes other
			// clients in screen range.
			//
			// var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			// var skillHandle2 = ZoneServer.Instance.World.CreateSkillHandle();
			// 
			// if (caster is Character character)
			// {
			// 	Send.ZC_OBJECT_PROPERTY(character, PropertyName.NormalASPD);
			// 	Send.ZC_OBJECT_PROPERTY(character, PropertyName.NormalASPD_BM);
			// }

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);

			// Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, Position.Zero);
			// Send.ZC_SYNC_START(caster, skillHandle, 1);
			// Send.ZC_SYNC_END(caster, skillHandle, 0);
			// Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle);
			// Send.ZC_SYNC_START(caster, skillHandle2, 1);
			// Send.ZC_SYNC_END(caster, skillHandle2, 0);
			// Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle2, TimeSpan.FromMilliseconds(150));

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var remainingCastTime = TimeSpan.FromMilliseconds(2500f);
			var endTime = DateTime.Now.Add(remainingCastTime);

			var delay = TimeSpan.FromMilliseconds(300f);
			await skill.Wait(delay);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			while (caster.IsCasting(skill))
			{
				if (endTime <= DateTime.Now)
					break;

				var skillCastDelay = TimeSpan.FromMilliseconds(600f / skill.Properties.GetFloat(PropertyName.SklSpdRate));
				var doubleHitDelay = TimeSpan.FromMilliseconds(150f / skill.Properties.GetFloat(PropertyName.SklSpdRate));

				await this.Attack(caster, skill, doubleHitDelay);

				if (remainingCastTime <= TimeSpan.Zero)
				{
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}

				await skill.Wait(skillCastDelay);

				remainingCastTime = remainingCastTime.Subtract(skillCastDelay);
				remainingCastTime = remainingCastTime.Subtract(doubleHitDelay);
			}

			Send.ZC_SKILL_DISABLE(caster);
		}

		/// <summary>
		/// Performs a double slash
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		public async Task Attack(ICombatEntity caster, Skill skill, TimeSpan doubleHitDelay)
		{
			var targetArea = caster.Position.GetRelative(caster.Direction, 30);
			var casterPos = caster.Position;

			var splashParam = skill.GetSplashParameters(caster, casterPos, targetArea, length: 40, width: 30, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 30;
			var damageDelay = 50;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, modifySkillHitResult: this.ModifySkillHitResult);

			await skill.Wait(doubleHitDelay);

			splashParam = skill.GetSplashParameters(caster, casterPos, targetArea, length: 40, width: 30, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 30;
			damageDelay = 80;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, modifySkillHitResult: this.ModifySkillHitResult);
		}
	}
}
