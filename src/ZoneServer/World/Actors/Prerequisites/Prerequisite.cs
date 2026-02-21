using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Zone.World.Actors.Prerequisites
{
	public abstract class Prerequisite
	{
		/// <summary>
		/// Returns true if the actor meets this preresquisite.
		/// </summary>
		/// <param name="actor"></param>
		/// <returns></returns>
		public abstract bool Met(IActor actor);

		public NotPrerequisite Not(params Prerequisite[] prerequisite)
		{
			return new NotPrerequisite(prerequisite);
		}
	}

	public class NotPrerequisite : Prerequisite
	{
		/// <summary>
		/// Returns a list of prerequisites of which one must be met.
		/// </summary>
		public Prerequisite[] Prerequisites { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="prerequisites"></param>
		public NotPrerequisite(params Prerequisite[] prerequisites)
		{
			this.Prerequisites = prerequisites;
		}

		/// <summary>
		/// Returns true if the character meets at least one of the
		/// prerequisites.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public override bool Met(IActor actor)
		{
			for (var i = 0; i < this.Prerequisites.Length; i++)
			{
				var prerequisite = this.Prerequisites[i];
				if (prerequisite.Met(actor))
					return false;
			}

			return true;
		}
	}
}
