namespace AIWars.Battleship.GameRepository
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Runtime.Serialization;

	public class GameState : DbContext
	{
		// Your context has been configured to use a 'GameState' connection string from your application's 
		// configuration file (App.config or Web.config). By default, this connection string targets the 
		// 'AIWars.Battleship.GameRepository.GameState' database on your LocalDb instance. 
		// 
		// If you wish to target a different database and/or database provider, modify the 'GameState' 
		// connection string in the application configuration file.
		public GameState()
			: base("name=GameState")
		{
		}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.


		public virtual DbSet<Game> Games { get; set; }
		public virtual DbSet<Player> Players { get; set; }
	}

	public class Game
	{
		public int Id { get; set; }
		public PlayerBoard Player1Board { get; set; }
		public PlayerBoard Player2Board { get; set; }
		public Guid NextPlayerGuid { get; set; }
	}

	public class PlayerBoard
	{
		public Player Player { get; set; }
		public Guid AssignedGuid { get; set; }
		public Board GameBoard { get; set; }
		public DateTimeOffset LastAttack { get; set; }
	}

	public class Board
	{
		public ShipCoordinates CarrierCoordinates { get; set; }
		public ShipCoordinates BattleshipCoordinates { get; set; }
		public ShipCoordinates DestroyerCoordinates { get; set; }
		public ShipCoordinates SubmarineCoordinates { get; set; }
		public ShipCoordinates PatrolCoordinates { get; set; }
		public List<Coordinates> Hits { get; set; }
		public List<Coordinates> Misses { get; set; }
	}

	public class Player
	{
		public int Id { get; set; }
		public Guid Guid { get; set; }
		public string Name { get; set; }
		public int Wins { get; set; }
		public int Losses { get; set; }
		public int AbandonedGames { get; set; }
		public int ActiveGames { get; set; }
	}

	[DataContract]
	public class ShipCoordinates
	{
		public int Id;
		[DataMember]
		public Coordinates Start;
		[DataMember]
		public Coordinates End;
	}

	[DataContract]
	public class Coordinates
	{
		public int Id;
		[DataMember]
		public int X;
		[DataMember]
		public int Y;
	}
}