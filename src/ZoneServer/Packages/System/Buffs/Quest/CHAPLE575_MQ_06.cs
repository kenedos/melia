using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Laima.Quest
{
	/// <summary>
	/// Handler for CHAPLE575_MQ_06, the Namott Holy Water, which hides its
	/// bearer from the demons of the Tenet Church B1 and cannot be active
	/// anywhere else.
	/// </summary>
	[Package("system")]
	[BuffHandler(BuffId.CHAPLE575_MQ_06)]
	public class CHAPLE575_MQ_06Override : BuffHandler
	{
		private const string MapClassName = "d_chapel_57_5";
		private const string Effect = "I_smoke038_blue";

		public CHAPLE575_MQ_06Override()
		{
			ConditionalCloaking.Register(BuffId.CHAPLE575_MQ_06, (observer, target) => observer.Faction == FactionType.Monster);
		}

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target.Map?.ClassName != MapClassName)
			{
				buff.Target.StopBuff(BuffId.CHAPLE575_MQ_06);
				return;
			}

			buff.Target.AttachEffect(Effect, 1f, EffectLocation.Bottom);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.DetachEffect(Effect);
		}
	}
}
