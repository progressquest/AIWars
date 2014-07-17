using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server.Tests
{
	[TestFixture]
	class PlayerTests
	{
		[Test]
		public void CreatePlayer_Success()
		{
			Board board = new BoardSetupHelper().GetBoard();
			Player player = new Player(board, "Player 1");

			Assert.AreNotEqual(player.PlayerGuid, default(Guid));
			Assert.AreSame(board, player.PlayerBoard);
			Assert.AreEqual("Player 1", player.PlayerName);
		}
	}
}
