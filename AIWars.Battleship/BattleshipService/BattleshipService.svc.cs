using AIWars.Battleship.GameRepository;
using AIWars.Battleship.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AIWars.Battleship.BattleshipService
{
	public class BattleshipService : IBattleshipService
	{
		private IGameManager GameManager { get; set; }

		public BattleshipService(IGameManager manager)
		{
			GameManager = manager;
		}

		public BattleshipService() : this(new GameManager()) { }

		public BoardDto GetPlayerBoard(Guid gameGuid)
		{
			bool isPlayerTurn;
			var gameStatus = GameManager.GetGameStatus(gameGuid, out isPlayerTurn);
			var boardDto = BoardDto.ConvertFrom(gameStatus);
			boardDto.IsPlayerTurn = isPlayerTurn;
			return boardDto;
		}

		public AttackResult Attack(Guid playerGuid, Coordinates coordinates)
		{
			GameManager.LoadGame(playerGuid);
			if (!GameManager.GameLoaded)
				return new AttackResult { Hit = false, Sunk = ShipTypes.None };

			return GameManager.Attack(playerGuid, coordinates);
		}

		public Guid RegisterPlayer(AIWars.Battleship.GameRepository.Player player)
		{
			return new GameStateRepository().RegisterPlayer(player);
		}

		public Guid BeginGame(Guid playerGuid)
		{
			return new GameStateRepository().BeginGame(playerGuid);
		}

	}
}
