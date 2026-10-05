using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_FreezeBullet)]
	public class BulletMarker_FreezeBullet : ISelfSkillHandler
	{
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(900);
		private static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(500);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster is not Character character || caster.IsDead)
				return;

			if (!caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				character.ServerMessage(Localization.Get("Double Gun Stance must be active."));
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				this.CancelSkill(skill, caster);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, dir, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.ApplyBuffAfterAnimation(skill, caster, character));
		}

		private async Task ApplyBuffAfterAnimation(Skill skill, ICombatEntity caster, Character character)
		{
			try
			{
				await skill.Wait(AnimationDuration);

				if (caster.IsDead)
					return;

				caster.StopBuff(BuffId.FreezeBullet_Buff);
				caster.StartBuff(BuffId.FreezeBullet_Buff, skill.Level, 0f, Duration, caster, skill.Id);
				BulletMarkerOverheatingHelper.AddSkillStacks(character, skill);
			}
			finally
			{
				this.FinishSkill(skill, caster);
			}
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}

		private void FinishSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}
	}
}
