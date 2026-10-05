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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Exorcist
{
	[Package("laima")]
	[SkillHandler(SkillId.Exorcist_AquaBenedicta)]
	public class Exorcist_AquaBenedicta : IGroundSkillHandler, IDynamicCasted
	{
		private const string AquaBenedictaPadName = "Exorcist_AquaBenedicta";

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("No target location specified."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

			skill.Run(this.CreateAquaBenedicta(skill, character, targetPos));
		}

		private async Task CreateAquaBenedicta(Skill skill, Character caster, Position targetPosition)
		{
			try
			{
				await MissilePadThrow(skill, caster, targetPosition, new MissileConfig
				{
					Effect = EffectConfig.None,
					EndEffect = EffectConfig.None,
					DotEffect = EffectConfig.None,
					GroundEffect = EffectConfig.None,
					Range = 60f,
					FlyTime = 0f,
					DelayTime = 0f,
					Gravity = 0f,
					Speed = 0f,
					HitTime = 0f,
					HitCount = 0,
					GroundDelay = 0f,
					EffectMoveDelay = 0f,
				}, 0f, AquaBenedictaPadName);
			}
			finally
			{
				caster.SetAttackState(false);
			}
		}
	}
}
