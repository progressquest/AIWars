using AIWars.Battleship.GameRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRepositoryTests
{
	class GameStateRepositoryHelper
	{
		private IGameStateRepository _Repository;
		private Game _ValidGame;
		private Guid Player1Guid = new Guid("{11111111-1111-1111-1111-111111111111}");
		private Guid Player2Guid = new Guid("{22222222-2222-2222-2222-222222222222}");
		private PlayerBoard _ValidBoard;
		private ShipCoordinates _CarrierCoordinates;
		private ShipCoordinates _BattleshipCoordinates;
		private ShipCoordinates _DestroyerCoordinates;
		private ShipCoordinates _SubmarineCoordinates;
		private ShipCoordinates _PatrolCoordinates;
		private List<Coordinates> _Hits;
		private List<Coordinates> _Misses;


		public List<Coordinates> Hits
		{
			get
			{
				return _Hits ?? (_Hits = new List<Coordinates>());
			}
		}
		public List<Coordinates> Misses
		{
			get
			{
				return _Misses ?? (_Misses = new List<Coordinates>());
			}
		}

		public ShipCoordinates CarrierCoordinates
		{
			get
			{
				return _CarrierCoordinates ?? (_CarrierCoordinates = new ShipCoordinates
				{
					Id = 1,
					Start = new Coordinates
					{
						Id = 1,
						X = 0,
						Y = 0,
					},
					End = new Coordinates
					{
						Id = 2,
						X = 4,
						Y = 0,
					},
				});
			}
		}
		public ShipCoordinates BattleshipCoordinates
		{
			get
			{
				return _BattleshipCoordinates ?? (_BattleshipCoordinates = new ShipCoordinates
				{
					Id = 2,
					Start = new Coordinates
					{
						Id = 3,
						X = 0,
						Y = 1,
					},
					End = new Coordinates
					{
						Id = 4,
						X = 3,
						Y = 1,
					},
				});
			}
		}
		public ShipCoordinates DestroyerCoordinates
		{
			get
			{
				return _DestroyerCoordinates ?? (_DestroyerCoordinates = new ShipCoordinates
				{
					Id = 3,
					Start = new Coordinates
					{
						Id = 5,
						X = 0,
						Y = 2,
					},
					End = new Coordinates
					{
						Id = 6,
						X = 2,
						Y = 2,
					},
				});
			}
		}
		public ShipCoordinates SubmarineCoordinates
		{
			get
			{
				return _SubmarineCoordinates ?? (_SubmarineCoordinates = new ShipCoordinates
				{
					Id = 4,
					Start = new Coordinates
					{
						Id = 7,
						X = 0,
						Y = 3,
					},
					End = new Coordinates
					{
						Id = 8,
						X = 2,
						Y = 3,
					},
				});
			}
		}
		public ShipCoordinates PatrolCoordinates
		{
			get
			{
				return _PatrolCoordinates ?? (_PatrolCoordinates = new ShipCoordinates
				{
					Id = 5,
					Start = new Coordinates
					{
						Id = 9,
						X = 0,
						Y = 3,
					},
					End = new Coordinates
					{
						Id = 10,
						X = 1,
						Y = 3,
					},
				});
			}
		}

		public PlayerBoard ValidBoard
		{
			get
			{
				return _ValidBoard ?? (_ValidBoard = new PlayerBoard
					{
						AssignedGuid = Player1Guid,
						GameBoard = new Board
						{
							CarrierCoordinates = CarrierCoordinates,
							BattleshipCoordinates = BattleshipCoordinates,
							DestroyerCoordinates = DestroyerCoordinates,
							SubmarineCoordinates = SubmarineCoordinates,
							PatrolCoordinates = PatrolCoordinates,
							Hits = Hits,
							Misses = Misses,
						}
					});
			}
		}

		public Game ValidGame
		{
			get
			{
				return _ValidGame ?? (_ValidGame = new Game
				{
					Id = 1,
					NextPlayerGuid = Player1Guid,
					Player1Board = ValidBoard,
					Player2Board = ValidBoard,
				});
			}
		}

		public IGameStateRepository Repository
		{
			get
			{
				return _Repository ?? (_Repository = GetMockRepository());
			}
			set
			{
				_Repository = value;
			}
		}

		private IGameStateRepository GetMockRepository()
		{
			var mockRepository = new Mock<IGameStateRepository>();
			mockRepository.Setup(x => x.GetGame(It.IsAny<Guid>())).Returns(ValidGame);

			return mockRepository.Object;
		}

		public IGameStateRepository GetRepository(out Guid nextPlayerGuid)
		{
			nextPlayerGuid = Player1Guid;
			return GetRepository();
		}

		public IGameStateRepository GetRepository()
		{
			return Repository;
		}

	}
}
