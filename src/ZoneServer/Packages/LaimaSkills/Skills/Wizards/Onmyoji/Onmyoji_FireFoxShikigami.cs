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
using Melia.Zone.World.Maps;
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
		private const string FoxAnimation = "ONMYOJI_FIREFOXSHIKIGAMI";
		private static readonly TimeSpan FoxEnterDelay = TimeSpan.FromMilliseconds(400);

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

			var fox = MonsterSkillCreateMob(skill, caster, FoxClassName, caster.Position, 0, name, "None", 0, (float)duration.TotalSeconds, "None", FoxProperties, immediate: true);
			if (fox == null)
				return;

			fox.Tendency = TendencyType.Aggressive;
			fox.Vars.SetString("Melia.Summon.PlayAnimation", FoxAnimation);
			fox.Components.Add(new AiComponent(fox, FoxAiName, caster));

			var viewers = fox.Map.GetCharacters(c => c.Position.InRange2D(fox.Position, Map.VisibleRange));
			foreach (var viewer in viewers)
				viewer.LookAround();

			var syncKey = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SYNC_START(caster, syncKey, 1);

			Send.ZC_MOVE_STOP(fox, fox.Position);
			fox.DelayEnterWorld();
			fox.EnterDelayedActor();

			var follow = new FollowToActorComponent(fox, caster, FollowNode, 10f, 30f, 0f, 1, 0.1f);
			fox.Components.Add(follow);
			foreach (var viewer in viewers)
				Send.ZC_FOLLOW_TO_ACTOR(viewer.Connection, fox, caster, follow.NodeName, follow.F1, follow.F2, follow.F3, follow.B1, follow.F4);

			fox.StartBuff(BuffId.Ability_buff_PC_FireFox_Summon, skill.Level, 0, TimeSpan.Zero, fox, skill.Id);

			Send.ZC_SYNC_END(caster, syncKey, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, syncKey, FoxEnterDelay);

			var buff = caster.StartBuff(BuffId.FireFoxShikigami_Buff, skill.Level, 0, duration, caster, skill.Id);
			buff?.Vars.Set(FoxVar, fox);
		}
	}
}
