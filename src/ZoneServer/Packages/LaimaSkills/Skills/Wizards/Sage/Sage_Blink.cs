using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Wizards.Sage
{
	/// <summary>
	/// Handler for the Sage skill Blink, which leaves an apparition behind
	/// that draws the nearby monsters' attention and teleports the Sage to a
	/// random spot within the skill's distance.
	/// </summary>
	/// <remarks>
	/// Blink: Looming doubles the attention the apparition draws.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Sage_Blink)]
	public class Sage_BlinkOverride : IGroundSkillHandler
	{
		private const float AttentionRange = 300f;
		private const int Hate = 999;
		private static readonly TimeSpan InvincibleDuration = TimeSpan.FromSeconds(1);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var apparition = character.Clone(caster.Position);
			apparition.StartBuff(BuffId.Blink_ColorBlned, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);

			var hate = caster.IsAbilityActive(AbilityId.Sage14) ? Hate * 2 : Hate;
			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, AttentionRange))
				enemy.InsertHate(apparition, hate);

			var distance = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var destination = caster.Map.Ground.GetLastValidPosition(caster.Position, caster.Position.GetRandomInRange2D(distance / 2, distance));

			caster.SetPosition(destination);
			caster.StartBuff(BuffId.Skill_NoDamage_Buff, InvincibleDuration);
		}
	}
}
