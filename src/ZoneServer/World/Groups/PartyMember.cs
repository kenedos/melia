using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Groups
{
	public class GuildMember : GroupMember
	{
		public Properties Properties { get; set; } = new Properties("GuildMember");

		public int Contribution
		{
			get
			{
				return (int)this.Properties[PropertyName.Contribution];
			}
			set
			{
				this.Properties[PropertyName.Contribution] = value;
			}
		}
	}
	public class PartyMember : GroupMember
	{
		public Properties Properties { get; set; } = new Properties("PartyMember");

		public static PartyMember ToMember(Character character)
		{
			var member = new PartyMember()
			{
				DbId = character.DbId,
				AccountId = character.AccountDbId,
				Gender = character.Gender,
				Hair = character.Hair,
				Handle = character.Handle,
				Hp = character.Hp,
				JobLevel = character.Job?.Level ?? 1001,
				Sp = character.Sp,
				Level = character.Level,
				MapId = character.MapId,
				TeamName = character.TeamName,
				MaxHp = character.MaxHp,
				MaxSp = character.MaxSp,
				Name = character.Name,
				Position = character.Position,
				Stance = character.Stance,
				IsOnline = character.Connection?.LoggedIn ?? false,
			};
			var i = 0;
			foreach (var job in character.Jobs.GetList())
			{
				member.VisualJobId = job.Id;
				switch (i)
				{
					case 0:
						member.FirstJobId = job.Id;
						break;
					case 1:
						member.SecondJobId = job.Id;
						break;
					case 2:
						member.ThirdJobId = job.Id;
						break;
					case 3:
						member.FourthJobId = job.Id;
						break;
				}
				i++;
			}
			return member;
		}
	}
}
