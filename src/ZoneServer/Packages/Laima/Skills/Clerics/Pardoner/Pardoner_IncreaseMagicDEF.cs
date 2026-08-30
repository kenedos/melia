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

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Increase Magic DEF.
	/// Temporarily increases the Magic Defense of caster and party members.
	/// The increase applies proportionally to the caster's SPR.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_IncreaseMagicDEF)]
	public class Pardoner_IncreaseMagicDEFOverride : IGroundSkillHandler
	{
		private const float BuffRange = 150f;
		private const int MaxTargets = 50;

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
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var buffDuration = skill.Properties.CaptionTime;

			var skillTargets = SkillSelectAlliesInCircle(caster, caster.Position, BuffRange, MaxTargets);
			if (!skillTargets.Contains(caster))
				skillTargets.Add(caster);

			await skill.Wait(TimeSpan.FromMilliseconds(90));

			SkillTargetBuff(skill, caster, skillTargets, BuffId.IncreaseMagicDEF_Buff, skill.Level, 0f, buffDuration, skill.Id);
		}
	}
}
