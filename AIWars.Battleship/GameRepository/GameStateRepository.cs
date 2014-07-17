using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Battleship.GameRepository
{
	public interface IGameStateRepository
	{
		Game GetGame(Guid playerGuid);
		Game GetGameWithNextPlayer(Guid nextPlayerGuid);
		List<Game> GetOldGames(DateTime lastAttackSince);

		void SaveState(Guid playerGuid, Board board);
		Guid RegisterPlayer(Player player);
		Guid BeginGame(Guid playerGuid);
	}

	public class GameStateRepository : IGameStateRepository
	{
		public Game GetGame(Guid playerGuid)
		{
			using (var gameState = new GameState())
			{
				return gameState.Games.FirstOrDefault(g => g.Player1Board.AssignedGuid == playerGuid || g.Player2Board.AssignedGuid == playerGuid);
			}
		}

		public Game GetGameWithNextPlayer(Guid nextPlayerGuid)
		{
			using (var gameState = new GameState())
			{
				return gameState.Games.FirstOrDefault(g => g.NextPlayerGuid == nextPlayerGuid);
			}
		}

		public List<Game> GetOldGames(DateTime lastAttackSince)
		{
			using (var gameState = new GameState())
			{
				return gameState.Games.Where(g => g.Player1Board.LastAttack < lastAttackSince && g.Player2Board.LastAttack < lastAttackSince).ToList();
			}
		}

		public void SaveState(Guid playerGuid, Board board)
		{
			using (var gameState = new GameState())
			{
				var game = gameState.Games.FirstOrDefault(g => g.NextPlayerGuid == playerGuid);
				var isPlayer1Guid = game.Player1Board.AssignedGuid == playerGuid;
				game.NextPlayerGuid = isPlayer1Guid ? game.Player2Board.AssignedGuid : game.Player1Board.AssignedGuid;
				if (isPlayer1Guid)
				{
					game.Player1Board.GameBoard = board;
					game.Player1Board.LastAttack = DateTimeOffset.Now;
				}
				else
				{
					game.Player2Board.GameBoard = board;
					game.Player2Board.LastAttack = DateTimeOffset.Now;
				}

				gameState.SaveChanges();
			}
		}

		public Guid RegisterPlayer(Player player)
		{
			using (var gameState = new GameState())
			{
				var currentPlayer = gameState.Players.FirstOrDefault(p => p.Name == player.Name);
				if (currentPlayer != null)
				{
					return new Guid();
				}
				player.AbandonedGames = 0;
				player.ActiveGames = 0;
				player.Losses = 0;
				player.Wins = 0;

				gameState.Players.Add(player);
				gameState.SaveChanges();

				return player.Guid;
			}
		}

		public Guid BeginGame(Guid playerGuid)
		{
			Player player;
			Player[] players;
			using (var gameState = new GameState())
			{
				player = gameState.Players.FirstOrDefault(p => p.Guid == playerGuid);
				if (player == null) return new Guid();
				// First get players with fewest active games
				players = gameState.Players.Where(p => p.Guid != playerGuid).OrderBy(p => p.ActiveGames).Take(30).ToArray();
				if (players == null || players.Length == 0) return new Guid();
			}
			
			// Next get the player with the closest record
			var playerScore = GetPlayerScore(player);
			var closestScore = 0.0;
			var opponentGuid = new Guid();
			foreach (var opponent in players)
			{
				var opponentScore = GetPlayerScore(opponent);
				closestScore = Math.Abs(playerScore - opponentScore) < Math.Abs(playerScore - closestScore) ? opponentScore : closestScore;
				opponentGuid = opponent.Guid;
			}
			if (opponentGuid == new Guid()) return new Guid();

			var player1Board = new Board{}
		}

		private double GetPlayerScore(Player player)
		{
			return player.Wins * 2 - player.Losses * 1.5;
		}

	}
}
