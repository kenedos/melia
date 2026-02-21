using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Shared.Game.Const
{
	public enum HandlerPriority
	{
		None = 0,
		Low = 50,
		Normal = 100,
		Override = 200,
	}
}
