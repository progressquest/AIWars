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
	[ServiceContract]
	public interface IBattleshipService
	{
		[OperationContract]
		[WebGet]
		BoardDto GetPlayerBoard(Guid gameGuid);

		[OperationContract]
		[WebInvoke]
		AttackResult Attack(Guid playerGuid, Coordinates coordinates);

		[OperationContract]
		[WebInvoke]
		Guid RegisterPlayer(AIWars.Battleship.GameRepository.Player player);

		[OperationContract]
		[WebInvoke]
		Guid BeginGame(Guid playerGuid);
	}
}
