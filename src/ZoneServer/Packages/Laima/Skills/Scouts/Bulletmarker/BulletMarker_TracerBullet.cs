using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Bullet Marker skill Tracer Bullet.
	/// SkillId: 51109
	/// Passive effect applied automatically after learning the skill.
	/// Accuracy: +3% per level.
	/// Critical Rate: +3% per level.
	/// Lv10: +30% Accuracy and +30% Critical Rate.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_TracerBullet)]
	public class BulletMarker_TracerBullet : IPassiveSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster)
		{
			if (caster is not Character character || skill.Level <= 0)
				return;

			if (character.TryGetBuff(BuffId.TracerBullet_Buff, out var currentBuff))
			{
				if ((int)currentBuff.NumArg1 == skill.Level)
					return;

				character.StopBuff(BuffId.TracerBullet_Buff);
			}

			character.StartBuff(BuffId.TracerBullet_Buff, skill.Level, 0f, TimeSpan.Zero, character, skill.Id);
		}
	}
}
