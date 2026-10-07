using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for the Shinobi skill Bunshin no Jutsu, which creates two
	/// clones that fight beside the Shinobi and copy their jutsu while the
	/// Shinobi's stamina lasts.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Shinobi_Bunshin_no_jutsu)]
	public class Shinobi_BunshinNoJutsuOverride : IGroundSkillHandler
	{
		private const int CloneCount = 2;
		private const float CloneDistance = 25f;
		private static readonly TimeSpan SummonDelay = TimeSpan.FromMilliseconds(800);

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

			skill.Run(this.Summon(skill, character));
		}

		/// <summary>
		/// Replaces any clones the Shinobi has with two new ones.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="character"></param>
		/// <returns></returns>
		private async Task Summon(Skill skill, Character character)
		{
			await skill.Wait(SummonDelay);

			if (character.IsDead)
				return;

			character.StopBuff(BuffId.Bunshin_Debuff);
			ShinobiSkillHelper.RemoveClones(character);

			var duration = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio));

			for (var i = 0; i < CloneCount; i++)
			{
				var position = character.Position.GetRelative(character.Direction.AddDegreeAngle(i == 0 ? 90 : -90), CloneDistance);
				if (!character.Map.Ground.TryGetNearestValidPosition(position, out position))
					position = character.Position;

				var clone = character.Clone(position);
				clone.StartBuff(BuffId.Bunshin_Buff, skill.Level, 0, duration, character, skill.Id);
				clone.Components.Add(new AiComponent(clone, "Bunshin", character));
			}

			character.StartBuff(BuffId.Bunshin_Debuff, skill.Level, 0, duration, character, skill.Id);
		}
	}
}
