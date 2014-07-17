using AIWars.Battleship.GameRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server
{
	public interface IBoardVerifier
	{
		bool VerifyBoard(Board board);
		ShipTypes GetShipType(ShipCoordinates ship);
	}

	public class BoardVerifier : IBoardVerifier
	{
		public bool VerifyBoard(Board board)
		{
			return CorrectNumberOfShips(board.Ships) 
				&& ShipMakeupIsCorrect(board.Ships) 
				&& ShipsAreOnBoard(board.Ships)
				&& ShipsDontOverlap(board.Ships);
		}

		private bool ShipsDontOverlap(List<ShipCoordinates> ships)
		{
			int[,] shipBoard = new int[Board.BOARD_SIZE, Board.BOARD_SIZE];

			foreach (var ship in ships)
			{
				var minX = Math.Min(ship.Start.X, ship.End.X);
				var maxX = Math.Max(ship.Start.X, ship.End.X);

				var minY = Math.Min(ship.Start.Y, ship.End.Y);
				var maxY = Math.Max(ship.Start.Y, ship.End.Y);

				for (int x = minX; x <= maxX; x++)
				{
					for (int y = minY; y <= maxY; y++)
					{
						if (shipBoard[x, y] == 1)
							return false;
						shipBoard[x, y] = 1;
					}
				}
			}

			return true;
		}

		private bool ShipMakeupIsCorrect(List<ShipCoordinates> ships)
		{
			ShipTypes shipTypes = ShipTypes.None;
			foreach (var ship in ships)
			{
				var shipType = GetShipType(ship);
				if(shipType == ShipTypes.Destroyer)
					if((shipTypes & ShipTypes.Destroyer) == ShipTypes.Destroyer)
						shipType = ShipTypes.Submarine;
				shipTypes |= shipType;
			}
			return ((ShipTypes[]) Enum.GetValues(typeof(ShipTypes))).All(st=> (st & shipTypes) == st);
		}

		public ShipTypes GetShipType(ShipCoordinates ship)
		{
			var width = Math.Abs(ship.End.X - ship.Start.X);
			var height = Math.Abs(ship.End.Y - ship.Start.Y);
			if (width != 0 && height != 0)
				return ShipTypes.None;
			var shipLength = Math.Max(width, height);
			switch (shipLength)
			{
				case 1:
					return ShipTypes.Patrol;
				case 2:
					return ShipTypes.Destroyer;
				case 3:
					return ShipTypes.Battleship;
				case 4:
					return ShipTypes.Carrier;
				default:
					return ShipTypes.None;
			}
		}

		private static bool CorrectNumberOfShips(List<ShipCoordinates> ships)
		{
			return ships.Count == 5;
		}

		private static bool ShipsAreOnBoard(List<ShipCoordinates> ships)
		{
			return ships.All(s => s.Start.X >= 0 && s.Start.Y >= 0 && s.Start.X < Board.BOARD_SIZE && s.Start.Y < Board.BOARD_SIZE
				&& s.End.X >= 0 && s.End.Y >= 0 && s.End.X < Board.BOARD_SIZE && s.End.Y < Board.BOARD_SIZE);
		}
	}
}
