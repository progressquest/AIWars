using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Battleship.Server.Tests
{
	class GameManagerHelper
	{
		private Board _Board1;
		private Board _Board2;
		private GameManager _GameManager;
		private BoardSetupHelper _BoardSetupHelper;
		private Player _Player1;
		private Player _Player2;

		public Board Board1
		{
			get
			{
				if (_Board1 == null)
				{
					_Board1 = _BoardSetupHelper.SetVerifier(new BoardVerifier()).GetBoard();
				}
				return _Board1;
			}
			set
			{
				_Board1 = value;
			}
		}

		public Board Board2
		{
			get
			{
				if (_Board2 == null)
				{
					_Board2 = _BoardSetupHelper.SetVerifier(new BoardVerifier()).GetBoard();
				}
				return _Board2;
			}
			set
			{
				_Board2 = value;
			}
		}

		public Player Player1
		{
			get
			{
				if (_Player1 == null)
				{
					_Player1 = new Player(Board1, "Player 1");
				}
				return _Player1;
			}
			set
			{
				_Player1 = value;
			}
		}

		public Player Player2
		{
			get
			{
				if (_Player2 == null)
				{
					_Player2 = new Player(Board2, "Player 2");
				}
				return _Player2;
			}
			set
			{
				_Player2 = value;
			}
		}

		public GameManager GameManager
		{
			get
			{
				if (_GameManager == null)
				{
					_GameManager = new GameManager { Player1 = Player1, Player2 = Player2 };
				}
				return _GameManager;
			}
			set
			{
				_GameManager = value;
			}
		}

		public GameManagerHelper()
		{
			_BoardSetupHelper = new BoardSetupHelper();
		}

		public GameManager GetGameManager()
		{
			return GameManager;
		}
	}
}
