using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Miko
{
	/// <summary>
	/// Clap.
	/// Increases the damage dealt by the caster and nearby allies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Miko_Kasiwade)]
	public class Miko_Kasiwade : IGroundSkillHandler
	{
		private const float EffectRange = 100f;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(15);
		private static readonly TimeSpan CastDelay = TimeSpan.FromMilliseconds(700);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.ApplyClap(skill, character));
		}

		private async Task ApplyClap(Skill skill, Character caster)
		{
			await skill.Wait(CastDelay);

			var allies = caster.Map.GetCharacters(character => character != null && !character.IsDead && character.Layer == caster.Layer && character.IsAlly(caster) && caster.Position.Get2DDistance(character.Position) <= EffectRange).ToList();

			if (!allies.Contains(caster))
				allies.Add(caster);

			foreach (var ally in allies)
				ally.StartBuff(BuffId.Kasiwade_Buff, skill.Level, 0f, BuffDuration, caster, skill.Id);

			caster.SetAttackState(false);
		}
	}
}
