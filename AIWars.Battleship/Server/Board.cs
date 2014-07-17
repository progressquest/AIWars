using AIWars.Battleship.GameRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIWars.Battleship.Server
{
	public class Board
	{
		public const int BOARD_SIZE = 10;

		public IBoardVerifier BoardVerifier { get; set; }

		public List<ShipCoordinates> Ships { get; set; }

		public List<Coordinates> Hits { get; set; }

		public List<Coordinates> Misses { get; set; }

		public ShipTypes ShipsSunk { get; set; }

		public Board(IBoardVerifier boardVerifier)
		{
			BoardVerifier = boardVerifier;
			Hits = new List<Coordinates>();
		}

		private Board()
		{
		}

		public bool VerifyBoard()
		{
			if (BoardVerifier == null) return true;

			return BoardVerifier.VerifyBoard(this);
		}


		internal static Board ConvertFrom(GameRepository.Board board)
		{
			return new Board
			{
				BoardVerifier = null,
				Hits = board.Hits,
				Misses = board.Misses,
				Ships = new List<ShipCoordinates>{
					new ShipCoordinates{
						Start = board.CarrierCoordinates.Start,
						End = board.CarrierCoordinates.End,
					},
					new ShipCoordinates{
						Start = board.BattleshipCoordinates.Start,
						End = board.BattleshipCoordinates.End,
					},
					new ShipCoordinates{
						Start = board.DestroyerCoordinates.Start,
						End = board.DestroyerCoordinates.End,
					},
					new ShipCoordinates{
						Start = board.SubmarineCoordinates.Start,
						End = board.SubmarineCoordinates.End,
					},
					new ShipCoordinates{
						Start = board.PatrolCoordinates.Start,
						End = board.PatrolCoordinates.End,
					},
				}
			};
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder("\n");
			for (int x = 0; x < BOARD_SIZE; x++)
			{
				for (int y = 0; y < BOARD_SIZE; y++)
				{
					sb.Append(WriteShip(x, y));
				}
				sb.Append('\n');
			}
			return sb.ToString();
		}

		private char WriteShip(int x, int y)
		{
			foreach (var ship in Ships)
			{
				if (ship.Start.X == x && ship.Start.Y == y)
					return 'S';
				if (ship.End.X == x && ship.End.Y == y)
					return 'E';
				if (((x <= ship.Start.X && x >= ship.End.X) || (x >= ship.Start.X && x <= ship.End.X))
					&& ((y <= ship.Start.Y && y >= ship.End.Y) || y >= ship.Start.Y && y <= ship.End.Y))
					return 'X';
			}
			return 'O';
		}

		internal static GameRepository.Board ConvertToRepository(Board attackedBoard)
		{
			return new GameRepository.Board
			{
				CarrierCoordinates = FindShip(attackedBoard, 5),
				BattleshipCoordinates = FindShip(attackedBoard, 4),
				DestroyerCoordinates = FindShip(attackedBoard, 3),
				SubmarineCoordinates = FindShip(attackedBoard, 3, true),
				PatrolCoordinates = FindShip(attackedBoard, 2),
				Hits = attackedBoard.Hits,
				Misses = attackedBoard.Misses,
			};
		}

		private static ShipCoordinates FindShip(Board attackedBoard, int length, bool skipFirst = false)
		{
			foreach (var ship in attackedBoard.Ships)
			{
				if (Math.Max(Math.Abs(ship.End.X - ship.Start.X), Math.Abs(ship.End.Y - ship.End.X)) == length)
				{
					if (!skipFirst)
						return ship;
					skipFirst = false;
				}
			}
			throw new InvalidOperationException(string.Format("Ship of length {0} could not be found on board with Id {1}", length, attackedBoard.ToString()));
		}
	}

}
