using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;

namespace Melia.Zone.Buffs.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid's shape shift, which gives the Druid the shape
	/// and skills of the monster.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Monster class id
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.transform)]
	public class Druid_Transform_BuffOverride : BuffHandler, ITransformationBuff
	{
		private const string EffectName = "Melia.Druid.Transform";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			DruidSkillHelper.AddFormSkills(buff, character, DruidSkillHelper.GetShapeSkills(character), 1);
			character.AddEffect(EffectName, new TransmuteEffect((int)buff.NumArg2, BuffId.transform));
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			character.RemoveEffect(EffectName);
			DruidSkillHelper.PlayFormEndEffect(character);
			DruidSkillHelper.RemoveFormSkills(buff, character);
		}
	}
}
