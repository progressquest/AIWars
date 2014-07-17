using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server
{
	[DataContract]
	public class AttackResult
	{
		[DataMember]
		public bool Hit { get; set; }
		[DataMember]
		public ShipTypes Sunk { get; set; }
	}
}
