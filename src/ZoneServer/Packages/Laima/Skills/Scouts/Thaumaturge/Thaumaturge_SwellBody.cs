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
	[SkillHandler(SkillId.Thaumaturge_SwellBody)]
	public class Thaumaturge_SwellBodyOverride : IMeleeGroundSkillHandler
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
			var targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, distance: 15);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, (int)(3 + skill.Level * 0.5)));

			var casterInt = caster.Properties.GetFloat(PropertyName.INT);
			var flatMhpFromInt = casterInt * 4f;

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			caster.StartBuff(BuffId.SwellBody_Abil_Buff, skill.Level, flatMhpFromInt, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

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
						member.StartBuff(BuffId.SwellBody_Abil_Buff, skill.Level, flatMhpFromInt, TimeSpan.FromSeconds(BuffDurationSeconds), caster);
					}
				}
			}

			await skill.Wait(TimeSpan.FromMilliseconds(350));
			var position = GetRelativePosition(PosType.TargetRandom, caster, caster, distance: 15);
			Send.ZC_GROUND_EFFECT(caster, position, "F_circle011_pink", 1.1f, 0f, 0f, 0f);
		}
	}
}
