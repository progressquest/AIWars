using AIWars.Battleship.GameRepository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server.Tests
{
	[TestFixture]
	class AttackTests
	{
		private static Coordinates HitCoordinates = new Coordinates { X = 4, Y = 4 };
		private static Coordinates MissCoordinates = new Coordinates { X = 0, Y = 0 };
		private const int BattleshipStartX = 6;
		private const int BattleshipStartY = 2;
		private const int BattleshipEndX = 6;
		private const int BattleshipEndY = 5;
		private static ShipCoordinates Battleship = new ShipCoordinates { Start = new Coordinates { X = BattleshipStartX, Y = BattleshipStartY }, End = new Coordinates { X = BattleshipEndX, Y = BattleshipEndY } };

		[TestCase(BattleshipStartX, BattleshipStartY)]
		[TestCase(BattleshipEndX, BattleshipEndY)]
		public void GameManager_Attack_Hit_Success(int x, int y)
		{
			var coordinates = new Coordinates { X = x, Y = y };
			var gameManager = new GameManagerHelper().GetGameManager();

			var result = gameManager.Attack(gameManager.Player1.PlayerGuid, coordinates);
			
			Assert.AreEqual(true, result.Hit);
			Assert.AreEqual(ShipTypes.None, result.Sunk);
		}

		[Test]
		public void GameManager_Attack_Miss_Success()
		{
			var gameManager = new GameManagerHelper().GetGameManager();

			var result = gameManager.Attack(gameManager.Player1.PlayerGuid, MissCoordinates);

			Assert.AreEqual(false, result.Hit);
			Assert.AreEqual(ShipTypes.None, result.Sunk);
		}

		[Test]
		public void GameManager_Attack_WrongGuid()
		{
			var gameManager = new GameManagerHelper().GetGameManager();

			var result = gameManager.Attack(new Guid(), HitCoordinates);

			Assert.AreEqual(false, result.Hit);
			Assert.AreEqual(ShipTypes.None, result.Sunk);
		}

		[Test]
		public void GameMangager_Attack_Sink()
		{
			List<Coordinates> allHits = new List<Coordinates>();
			for (int x = BattleshipStartX; x <= BattleshipEndX; x++)
			{
				for (int y = BattleshipStartY; y <= BattleshipEndY; y++)
				{
					allHits.Add(new Coordinates { X = x, Y = y });
				}
			}

			var gameManager = new GameManagerHelper().GetGameManager();

			AttackResult result = null;
			int hitCount = 0;
			foreach (var hit in allHits)
			{
				result = gameManager.Attack(gameManager.Player1.PlayerGuid, hit);
				Assert.AreEqual(true, result.Hit, string.Format("Game manager reported miss on ({0}, {1}).  Expectet hit.", hit.X, hit.Y));
				if (++hitCount != allHits.Count)
					Assert.AreEqual(ShipTypes.None, result.Sunk, string.Format("Game manager reported sink on {0}th hit.", hitCount));
			}
			Assert.AreEqual(ShipTypes.Battleship, (result.Sunk | ShipTypes.Battleship));
		}

		[Test]
		public void GameManager_Attack_IsReported()
		{
			var gameManager = new GameManagerHelper().GetGameManager();
			
			AttackResult result = null;
			string playerName = null;

			gameManager.PlayerAttackedEvent += (name, attackResult) =>
			{
				result = attackResult;
				playerName = name;
			};

			gameManager.Attack(gameManager.Player1.PlayerGuid, new Coordinates { X = 0, Y = 0 });

			Assert.AreEqual(gameManager.Player1.PlayerName, playerName);
			Assert.AreEqual(false, result.Hit);
			Assert.AreEqual(ShipTypes.None, result.Sunk);
		}

	}
}
