using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.Buffs;
using Melia.Shared.L10N;

namespace Melia.Zone.Skills.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handler for the Fletcher skill Cross Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fletcher_CrossFire)]
	public class Fletcher_CrossFireOverride : IPassiveSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster)
		{
			if (!skill.IsOnCooldown && !caster.IsBuffActive(BuffId.Fletcher_CrossFire_Buff))
				caster.StartBuff(BuffId.Fletcher_CrossFire_Buff, TimeSpan.Zero);
			skill.OnCooldownChanged += () => caster.StartBuff(BuffId.Fletcher_CrossFire_Buff, TimeSpan.Zero);
		}
	}
}
