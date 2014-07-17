using System;
using NUnit.Framework;
using AIWars.Battleship.Server;
using System.Collections.Generic;
using AIWars.Battleship.GameRepository;

namespace AIWars.Battleship.Server.Tests
{
	[TestFixture]
	public class BoardSetupTests
	{
		[Test]
		public void BoardVerifier_PlaceShip_Success()
		{
			var board = new BoardSetupHelper()
				.SetVerifier(new BoardVerifier())
				.GetBoard();

			var result = board.VerifyBoard();

			Assert.AreEqual(true, result);
		}

		[TestCase(-1, 0, 1, 0)]
		[TestCase(9, 10, 1, 0)]
		[TestCase(0, 1, -1, 0)]
		[TestCase(0, 1, 9, 10)]
		public void BoardVerifier_PlaceShip_OffBoard_Fail(int x1, int y1, int x2, int y2)
		{
			var board = new BoardSetupHelper()
				.GenerateShips(ShipTypes.Carrier | ShipTypes.Battleship | ShipTypes.Destroyer | ShipTypes.Submarine)
				.AddShip(new ShipCoordinates{
						Start = new Coordinates{X=x1, Y=y1},
						End = new Coordinates{X=x2, Y=y2}})
				.SetVerifier(new BoardVerifier())
				.GetBoard();

			var result = board.VerifyBoard();

			Assert.AreEqual(false, result);
		}

		[Test]
		public void BoardVerifier_PlaceShips_TooMany_Fail()
		{
			var board = new BoardSetupHelper()
				.GenerateShips(ShipTypes.Carrier | ShipTypes.Battleship | ShipTypes.Destroyer | ShipTypes.Submarine | ShipTypes.Patrol)
				.AddShip(new ShipCoordinates
				{
					Start = new Coordinates { X = 0, Y = 0 },
					End = new Coordinates { X = 0, Y = 1 }
				})
				.SetVerifier(new BoardVerifier())
				.GetBoard();

			var result = board.VerifyBoard();

			Assert.AreEqual(false, result);
		}

		[Test]
		public void BoardVerifier_PlaceShips_TooFew_Fail()
		{
			var board = new BoardSetupHelper()
				.GenerateShips(ShipTypes.Carrier | ShipTypes.Battleship | ShipTypes.Destroyer | ShipTypes.Submarine)
				.SetVerifier(new BoardVerifier())
				.GetBoard();

			var result = board.VerifyBoard();

			Assert.AreEqual(false, result);
		}

		[Test]
		public void BoardVerifier_PlaceShips_WrongMakeup_Fail()
		{
			var board = new BoardSetupHelper()
				.GenerateShips(ShipTypes.Carrier | ShipTypes.Battleship | ShipTypes.Destroyer | ShipTypes.Submarine)
				.AddShip(new ShipCoordinates
				{
					Start = new Coordinates { X = 0, Y = 0 },
					End = new Coordinates { X = 0, Y = 2 }
				})
				.SetVerifier(new BoardVerifier())
				.GetBoard();

			var result = board.VerifyBoard();

			Assert.AreEqual(false, result);
		}

		[TestCase(0, 0, 1, 1)]
		[TestCase(0, 0, 0, 0)]
		[TestCase(0, 0, 0, 10)]
		public void BoardVerifier_PlaceShips_InvalidXY_Fail(int x1, int y1, int x2, int y2)
		{
			var board = new BoardSetupHelper()
				.GenerateShips(ShipTypes.Carrier | ShipTypes.Battleship | ShipTypes.Destroyer | ShipTypes.Submarine)
				.AddShip(new ShipCoordinates
				{
					Start = new Coordinates { X = x1, Y = y1 },
					End = new Coordinates { X = x2, Y = y2 }
				})
				.SetVerifier(new BoardVerifier())
				.GetBoard();

			var result = board.VerifyBoard();

			Assert.AreEqual(false, result);
		}

		[TestCase(0, 0, 0, 2, 0, 0, 2, 0)]
		[TestCase(0, 0, 0, 2, 0, 1, 2, 1)]
		[TestCase(1, 0, 1, 2, 0, 1, 2, 1)]
		public void BoardVerifier_PlaceShips_Overlap_Fail(int xStart1, int yStart1, int xEnd1, int yEnd1, int xStart2, int yStart2, int xEnd2, int yEnd2)
		{
			var board = new BoardSetupHelper()
				.GenerateShips(ShipTypes.Carrier | ShipTypes.Battleship | ShipTypes.Patrol)
				.AddShip(new ShipCoordinates
				{
					Start = new Coordinates { X = xStart1, Y = yStart1 },
					End = new Coordinates { X = xEnd1, Y = yEnd1 }
				})
				.AddShip(new ShipCoordinates
				{
					Start = new Coordinates { X = xStart2, Y = yStart2 },
					End = new Coordinates { X = xEnd2, Y = yEnd2 }
				})
				.SetVerifier(new BoardVerifier())
				.GetBoard();

			var result = board.VerifyBoard();

			Assert.AreEqual(false, result);
		}

		[Test]
		public void PrintBoard_Success()
		{
			var board = new BoardSetupHelper().GetBoard();
			var printedBoard = board.ToString();

			Console.WriteLine(printedBoard);
		}

	}
}
