using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server
{
	public class Player
	{
		public Guid PlayerGuid { get; set; }
		public Board PlayerBoard { get; set; }
		public string PlayerName { get; set; }

		public Player(Board board, string playerName)
		{
			PlayerGuid = Guid.NewGuid();
			PlayerBoard = board;
			PlayerName = playerName;
		}

		private Player()
		{
		}


		internal static Player ConvertFrom(GameRepository.PlayerBoard playerBoard)
		{
			return new Player
			{
				PlayerGuid = playerBoard.AssignedGuid,
				PlayerBoard = Board.ConvertFrom(playerBoard.GameBoard),
				PlayerName = playerBoard.Player.Name,
			};
		}
	}
}
