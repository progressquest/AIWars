using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using AIWars.Battleship.GameRepository;
using AIWars.Battleship.Server;

namespace AIWars.Battleship.BattleshipService
{
	[DataContract]
	public class BoardDto
	{
		[DataMember]
		public List<ShipCoordinates> Ships { get; set; }
		[DataMember]
		public List<Coordinates> Hits { get; set; }
		[DataMember]
		public List<Coordinates> Misses { get; set; }
		[DataMember]
		public ShipTypes ShipsSunk { get; set; }
		[DataMember]
		public bool IsPlayerTurn { get; set; }

		internal static BoardDto ConvertFrom(Server.Board gameStatus)
		{
			return new BoardDto
			{
				Ships = gameStatus.Ships,
				Hits = gameStatus.Hits,
				Misses = gameStatus.Misses,
				ShipsSunk = gameStatus.ShipsSunk,
			};
		}
	}
}
