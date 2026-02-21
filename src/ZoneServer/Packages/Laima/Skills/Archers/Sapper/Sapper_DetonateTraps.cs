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
using Melia.Zone.World.Actors.Characters.Components;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper skill Detonate Traps.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sapper_DetonateTraps)]
	public class Sapper_DetonateTrapsOverride : IMeleeGroundSkillHandler
	{
		private const int CastDelayMs = 400;
		private const int MaxTrapsToDetonate = 4;
		private const float DetonationRange = 100f;
		private const float ExplosionEffectScale = 0.5f;
		private const float ExplosionRange = 40f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(CastDelayMs));

			if (caster.Components.TryGet<SummonComponent>(out var summons))
			{
				var claymores = summons.GetSummons(MonsterId.Claymore).ToList();

				foreach (var claymore in claymores)
				{
					claymore.Vars.Set("DetonateTrapsSkill", skill);
					claymore.Kill(null);
					summons.RemoveSummon(claymore);
				}
			}
		}
	}
}
