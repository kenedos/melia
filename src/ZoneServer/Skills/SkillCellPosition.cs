using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Zone.Skills
{
	/// <summary>
	/// Cell position (X,Y)
	/// </summary>
	public class SkillCellPosition
	{
		public int X { get; set; }
		public int Y { get; set; }

		public SkillCellPosition(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
