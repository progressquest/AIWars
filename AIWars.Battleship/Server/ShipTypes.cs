using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server
{
	[DataContract]
	[Flags]
	public enum ShipTypes
	{
		[DataMember]
		None = 0,
		[DataMember]
		Patrol = 1,
		[DataMember]
		Destroyer = 2,
		[DataMember]
		Submarine = 4,
		[DataMember]
		Battleship = 8,
		[DataMember]
		Carrier = 16
	}
}
