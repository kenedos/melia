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
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for the Onmyoji skill Soul Fox Shikigami, which summons a fox
	/// that hovers beside the Onmyoji, joins its fights and throws an orb at
	/// the enemies it fights.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Onmyoji_FireFoxShikigami)]
	public class Onmyoji_FireFoxShikigamiOverride : IGroundSkillHandler, IDynamicCasted
	{
		public const string FoxVar = "Melia.Onmyoji.Fox";
		private const string FoxClassName = "pcskill_FireFoxShikigami";
		private const string FoxAiName = "PC_Summon_FireFox";
		private const string FoxProperties = "Faction#Summon#FIXMSPD_BM#140";
		private const string FollowNode = "Dummy_RU_armband";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character)
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

			caster.StopBuff(BuffId.FireFoxShikigami_Buff);

			var duration = skill.Properties.CaptionTime;
			var name = "!@#${Auto_1}_of_{Auto_2}$*$Auto_1$*$" + caster.Name + "$*$Auto_2$*$@dicID_^*$ETC_20150317_000235$*^#@!";

			var fox = MonsterSkillCreateMob(skill, caster, FoxClassName, caster.Position, 0, name, "None", 0, (float)duration.TotalSeconds, "None", FoxProperties);
			if (fox == null)
				return;

			fox.Tendency = TendencyType.Aggressive;
			fox.Components.Add(new AiComponent(fox, FoxAiName, caster));
			fox.Components.Add(new FollowToActorComponent(fox, caster, FollowNode, 10f, 30f, 0f, 1, 0.1f));
			fox.StartBuff(BuffId.Invincible, duration);
			fox.StartBuff(BuffId.Ability_buff_PC_FireFox_Summon, skill.Level, 0, TimeSpan.Zero, fox, skill.Id);

			var buff = caster.StartBuff(BuffId.FireFoxShikigami_Buff, skill.Level, 0, duration, caster, skill.Id);
			buff?.Vars.Set(FoxVar, fox);
		}
	}
}
