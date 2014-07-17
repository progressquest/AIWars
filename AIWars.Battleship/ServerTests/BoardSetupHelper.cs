using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AIWars.Battleship.GameRepository;

namespace AIWars.Battleship.Server.Tests
{
	class BoardSetupHelper
	{
		private IBoardVerifier _BoardVerifier;
		private Board _Board;

		public IBoardVerifier BoardVerifier {
			get
			{
				if (_BoardVerifier == null)
				{
					var moqVerifier = new Mock<IBoardVerifier>();
					moqVerifier.Setup(x => x.VerifyBoard(It.IsAny<Board>()))
						.Returns(true);

					_BoardVerifier = moqVerifier.Object;
				}
				return _BoardVerifier;
			}
			set
			{
				_BoardVerifier = value;
			}
		}
		public Board Board
		{
			get
			{
				if (_Board == null)
				{
					_Board = new Board(BoardVerifier);
				}
				return _Board;
			}
			set
			{
				_Board = value;
			}
		}
		public List<ShipCoordinates> Ships { get; set; }

		public BoardSetupHelper GenerateShips(ShipTypes shipTypes)
		{
			if(Ships == null)
				Ships = new List<ShipCoordinates>();
			if ((shipTypes & ShipTypes.Carrier) == ShipTypes.Carrier)
			{
				Ships.Add(new ShipCoordinates { Start = new Coordinates { X = 4, Y = 2 }, End = new Coordinates { X = 4, Y = 6 } });
			}
			if ((shipTypes & ShipTypes.Battleship) == ShipTypes.Battleship)
			{
				Ships.Add(new ShipCoordinates { Start = new Coordinates { X = 6, Y = 2 }, End = new Coordinates { X = 6, Y = 5 } });
			}
			if ((shipTypes & ShipTypes.Destroyer) == ShipTypes.Destroyer)
			{
				Ships.Add(new ShipCoordinates { Start = new Coordinates { X = 5, Y = 3}, End = new Coordinates { X = 5, Y = 5 } });
			}
			if ((shipTypes & ShipTypes.Submarine) == ShipTypes.Submarine)
			{
				Ships.Add(new ShipCoordinates { Start = new Coordinates { X = 3, Y = 3 }, End = new Coordinates { X = 3, Y = 5 } });
			}
			if ((shipTypes & ShipTypes.Patrol) == ShipTypes.Patrol)
			{
				Ships.Add(new ShipCoordinates { Start = new Coordinates { X = 5, Y = 6 }, End = new Coordinates { X = 6, Y = 6 } });
			}

			return this;
		}

		public BoardSetupHelper AddShip(ShipCoordinates ship)
		{
			Ships.Add(ship);
			return this;
		}

		public Board GetBoard()
		{
			if (Ships == null)
			{
				GenerateShips(ShipTypes.Carrier | ShipTypes.Battleship | ShipTypes.Destroyer | ShipTypes.Submarine | ShipTypes.Patrol);
			}

			Board.Ships = Ships;
			return Board;
		}

		internal BoardSetupHelper SetVerifier(BoardVerifier boardVerifier)
		{
			BoardVerifier = boardVerifier;
			return this;
		}
	}
}
