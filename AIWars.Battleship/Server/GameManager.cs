using AIWars.Battleship.GameRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server
{
	public interface IGameManager
	{
		event PlayerAttackedEventHandler PlayerAttackedEvent;
		Player Player1 { get; set; }
		Player Player2 { get; set; }
		void LoadGame(Guid activePlayerGuid);
		AttackResult Attack(Guid guid, Coordinates coordinates);

		bool GameLoaded { get; }

		Board GetGameStatus(Guid playerGuid, out bool isPlayerTurn);
	}

	public delegate void PlayerAttackedEventHandler(string playerName, AttackResult results);

	public class GameManager : IGameManager
	{
		public bool GameLoaded { get; private set; }

		private IGameStateRepository GameStateRepository { get; set; }

		public event PlayerAttackedEventHandler PlayerAttackedEvent;

		public Player Player1 { get; set; }

		public Player Player2 { get; set; }

		public GameManager() : this(new GameStateRepository()) { }

		public GameManager(IGameStateRepository repository)
		{
			GameStateRepository = repository;
			GameLoaded = false;
		}

		public void LoadGame(Guid activePlayerGuid)
		{
			var gameState = GameStateRepository.GetGameWithNextPlayer(activePlayerGuid);

			if (gameState == null) return;

			Player1 = Player.ConvertFrom(gameState.Player1Board);
			Player1.PlayerBoard.ShipsSunk = GetAllSunk(Player1.PlayerBoard);

			Player2 = Player.ConvertFrom(gameState.Player2Board);
			Player2.PlayerBoard.ShipsSunk = GetAllSunk(Player2.PlayerBoard);

			GameLoaded = true;
		}

		public AttackResult Attack(Guid guid, Coordinates coordinates)
		{
			var attackingPlayer = (guid == Player1.PlayerGuid) ? Player1 : (guid == Player2.PlayerGuid) ? Player2 : null;
			if (attackingPlayer == null)
				return new AttackResult { Hit = false, Sunk = ShipTypes.None };

			var attackedPlayer = guid == Player1.PlayerGuid ? Player2 : Player1;

			var attackedBoard = (guid == Player1.PlayerGuid) ? Player2.PlayerBoard : (guid == Player2.PlayerGuid) ? Player1.PlayerBoard : null;
			
			var hit = CheckHit(attackedBoard, coordinates);
			if (hit)
				attackedBoard.Hits.Add(coordinates);
			else
				attackedBoard.Misses.Add(coordinates);

			var sunk = GetAllSunk(attackedBoard);

			var result = new AttackResult { Hit = hit, Sunk = sunk };

			GameStateRepository.SaveState(guid, Board.ConvertToRepository(attackedBoard));

			OnPlayerAttacked(attackingPlayer.PlayerName, result);

			return result;
		}

		public Board GetGameStatus(Guid playerGuid, out bool isPlayerTurn)
		{
			isPlayerTurn = false;
			var game = GameStateRepository.GetGame(playerGuid);
			if (game == null) return null;

			var playerBoard = game.Player1Board.AssignedGuid == playerGuid ? game.Player1Board : game.Player2Board;
			isPlayerTurn = playerGuid == playerBoard.AssignedGuid;

			return Board.ConvertFrom(playerBoard.GameBoard);
		}

		private ShipTypes GetAllSunk(Board board)
		{
			foreach (var ship in board.Ships)
			{
				ShipTypes shipType = board.BoardVerifier.GetShipType(ship);
				if ((board.ShipsSunk & shipType) == shipType)
					continue;

				if (ShipIsSunk(board.Hits, ship))
				{
					board.ShipsSunk |= shipType;
				}
			}
			return board.ShipsSunk;
		}

		private bool ShipIsSunk(List<Coordinates> hits, ShipCoordinates ship)
		{
			return CheckShip(ship, (x, y) => { return !hits.Any(c => c.X == x && c.Y == y); }, false);
		}

		private bool CheckHit(Board board, Coordinates coordinates)
		{
			foreach (var ship in board.Ships)
			{
				if (CheckShip(ship, (x, y) => { return coordinates.X == x && coordinates.Y == y; }, true))
					return true;
			}
			return false;
		}

		private bool CheckShip(ShipCoordinates ship, Func<int,int,bool> checkCondition, bool checkConditionTrue)
		{
			var minX = Math.Min(ship.Start.X, ship.End.X);
			var maxX = Math.Max(ship.Start.X, ship.End.X);

			var minY = Math.Min(ship.Start.Y, ship.End.Y);
			var maxY = Math.Max(ship.Start.Y, ship.End.Y);

			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					if (checkCondition(x, y))
						return checkConditionTrue;
				}
			}
			return !checkConditionTrue;
		}

		private void OnPlayerAttacked(string playerName, AttackResult results)
		{
			if (PlayerAttackedEvent != null)
				PlayerAttackedEvent(playerName, results);
		}

	}
}
