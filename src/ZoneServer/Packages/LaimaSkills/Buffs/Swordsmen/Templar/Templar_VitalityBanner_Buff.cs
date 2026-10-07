using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Flag of Vitality buff, which restores a share of
	/// the target's max HP every 2 seconds.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.VitalityBanner_Buff)]
	public class Templar_VitalityBanner_BuffOverride : BuffHandler
	{
		private const int HealInterval = 2000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(HealInterval);
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;
			if (target.IsDead)
				return;

			target.Heal(target.MaxHp * GetCaptionRatio(buff, 1) / 100f, 0);
		}
	}
}
