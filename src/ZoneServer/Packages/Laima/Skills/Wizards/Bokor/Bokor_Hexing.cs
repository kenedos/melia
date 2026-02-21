using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Combat;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Hexing.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Hexing)]
	public class Bokor_HexingOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int DebuffDurationMilliseconds = 20000;

		/// <summary>
		/// Starts the dynamic casting of the skill.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Ends the dynamic casting of the skill.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill behavior
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = caster.Position.GetRelative(caster.Direction, distance: 75);
			var circle = new CircleF(targetPos, 100f);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, circle);
			await skill.Wait(TimeSpan.FromMilliseconds(350));

			var character = caster as Character;
			var summons = character?.Summons.GetSummons();

			// Target count: 2 + SkillLevel
			var targetCount = 2 + skill.Level;
			foreach (var currentTarget in targets)
			{
				if (targetCount == 0)
					break;

				Send.ZC_NORMAL.SkillTargetAttachForce(caster, currentTarget, TimeSpan.FromSeconds(0.25), 1, "I_cleric_hexing_force_dark", 0.5f, EffectLocation.Top);

				await skill.Wait(TimeSpan.FromMilliseconds(150));

				currentTarget.StartBuff(BuffId.CurseOfWeakness_Debuff, skill.Level, 0, TimeSpan.FromMilliseconds(DebuffDurationMilliseconds), caster);

				if (summons != null)
				{
					foreach (var summon in summons)
					{
						summon.InsertHate(currentTarget, 300);
					}
				}

				targetCount--;
			}
		}
	}
}
