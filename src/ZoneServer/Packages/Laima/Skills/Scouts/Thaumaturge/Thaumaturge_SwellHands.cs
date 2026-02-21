using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[SkillHandler(SkillId.Thaumaturge_SwellHands)]
	public class Thaumaturge_SwellHandsOverride : IMeleeGroundSkillHandler
	{
		private const float BuffRange = 300;
		private const int BuffDurationSeconds = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets?.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, caster.Position, 150f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			await skill.Wait(TimeSpan.FromMilliseconds(200));

			var casterInt = caster.Properties.GetFloat(PropertyName.INT);

			caster.StartBuff(BuffId.SwellHands_Buff, skill.Level, casterInt, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

			if (caster is Character character)
			{
				var party = character.Connection.Party;
				if (party != null)
				{
					var members = caster.Map.GetPartyMembersInRange(character, BuffRange, true);
					foreach (var member in members)
					{
						if (member == caster)
							continue;
						member.StartBuff(BuffId.SwellHands_Buff, skill.Level, casterInt, TimeSpan.FromSeconds(BuffDurationSeconds), caster);
					}
				}
			}
		}
	}
}
