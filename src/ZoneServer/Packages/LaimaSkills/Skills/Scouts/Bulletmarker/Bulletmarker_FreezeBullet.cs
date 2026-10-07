using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for the Bullet Marker skill Freeze Bullet, which gives the
	/// Bullet Marker's basic pistol attacks a chance to freeze.
	/// </summary>
	/// <remarks>
	/// [Arts] Freeze Bullet: Fog also leaves a chilling fog around them.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Bulletmarker_FreezeBullet)]
	public class Bulletmarker_FreezeBulletOverride : ISelfSkillHandler
	{
		private const float FogRange = 50f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			caster.StartBuff(BuffId.FreezeBullet_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);

			if (!caster.IsAbilityActive(AbilityId.Bulletmarker16))
				return;

			var pad = new Pad(PadName.Bulletmarker_FreezeBullet, caster, skill, new Circle(caster.Position, FogRange));
			pad.Position = caster.Position;
			caster.Map.AddPad(pad);
		}
	}
}
