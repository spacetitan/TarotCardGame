using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CombatScene : Node2D
{
	[Export] private AudioStream BGM;
	private PlayerStats playerStats;
	[Export] private EnemyStats[] enemies;
	private List<Enemy> currentEnemies = new List<Enemy>();
	private List<Enemy> actingEnemies = new List<Enemy>();
	private Player player;
	const String ENEMY_SCENE = "res://Scenes/Character/Enemy.tscn";
	private Node2D enemiesParent;
	private WinPrize totalPrize = new WinPrize();

	public bool battleOver { get; private set; } = false;
	public bool playerTurn { get; private set; } = false;

	public override void _Ready()
	{
		GetSceneNodes();
		ConnectEventSignals();

		//temporary
		List<EnemyStats> temp  = new List<EnemyStats>();
		foreach(EnemyStats enemy in enemies)
		{
			temp.Add(enemy);
		}
		//

		this.InitializeBattle(GameManager.instance.playerStats, temp);
	}

	private void GetSceneNodes()
	{
		this.player = GetNode<Player>("%Player");
		this.enemiesParent = GetNode<Node2D>("%Enemies");
	}

	private void ConnectEventSignals()
	{
		EventManager.instance.PlayerTurnEnded += OnPlayerTurnEnded;
		EventManager.instance.PlayerDied += OnPlayerDied;
		EventManager.instance.EnemyActionCompleted += OnEnemyActionCompleted;
		EventManager.instance.EnemyTurnEnded += OnEnemyTurnEnded;
		EventManager.instance.EnemyDied += OnEnemyDeath;
	}

	private void DisconnectEventSignals()
	{
		EventManager.instance.PlayerTurnEnded -= OnPlayerTurnEnded;
		EventManager.instance.PlayerDied -= OnPlayerDied;
		EventManager.instance.EnemyActionCompleted -= OnEnemyActionCompleted;
		EventManager.instance.EnemyTurnEnded -= OnEnemyTurnEnded;
		EventManager.instance.EnemyDied -= OnEnemyDeath;
	}

	public void InitializeBattle(PlayerStats playerStats, List<EnemyStats> enemyList)
	{
		PlayerStats newStats = playerStats.CreateInstance();
		this.playerStats = newStats;
		UIManager.instance.battle.SetPlayerStats(this.playerStats);
		this.player.SetPlayerStats(this.playerStats);

		int count = 0;
		foreach (EnemyStats enemy in enemyList)
		{
			var scene = GD.Load<PackedScene>(ENEMY_SCENE);
			var newEnemy = scene.Instantiate();
			this.enemiesParent.AddChild(newEnemy);

			Enemy spawn = newEnemy as Enemy;
			spawn.SetEnemyStats(enemy);
			this.currentEnemies.Add(spawn);
			spawn.Position = new Vector2(spawn.Position.X - 100 * count, spawn.Position.Y + 100 * count);
			count++;
		}

		AudioManager.instance.musicPlayer.Stop();
		AudioManager.instance.musicPlayer.Play(this.BGM);

		this.player.StartBattle(this.playerStats);
		this.player.statusManager.StatusesApplied += OnPlayerStatusApplied;

		UIManager.instance.SetBattleUIVisibility(true);

		StartPlayerPhase();
	}

	public void StartPlayerPhase()
	{
		this.playerTurn = true;
		PlayerStartUpgrades();
	}

	void PlayerStartUpgrades()
	{
		PlayerStartEffects();
	}

	void PlayerStartEffects()
	{
		player.statusManager.ApplyStatusByType(StatusType.STARTOFTURN);
	}

	void StartPlayerTurn()
	{
		if(this.battleOver) {return;}
		this.player.StartTurn();
	}

	void OnPlayerTurnEnded()
	{
		this.playerTurn = false;
		this.player.hand.DisableHand();
		this.player.DiscardHand();
		PlayerEndTurnUpgrades();
		PlayerEndTurnEffects();
	}

	void PlayerEndTurnUpgrades()
	{

	}

	void PlayerEndTurnEffects()
	{
		player.statusManager.ApplyStatusByType(StatusType.ENDOFTURN);
	}

	void OnPlayerStatusApplied(StatusType type)
	{
		switch(type)
		{
			case StatusType.STARTOFTURN:
			StartPlayerTurn();
			break;

			case StatusType.ENDOFTURN:
			StartEnemyPhase();
			break;

			case StatusType.EVENT:
			break;
		}
	}

	void StartEnemyPhase()
	{
		if(this.currentEnemies != null && this.currentEnemies.Any())
		{
			this.actingEnemies.Clear();
			foreach(Enemy enemy in this.currentEnemies)
			{
				this.actingEnemies.Add(enemy);
			}
			
			EnemyStartUpgrades(this.actingEnemies[0]);
		}
	}

	void StartEnemyPhase(Enemy enemy)
	{
		if(this.actingEnemies != null && this.actingEnemies.Any())
		{
			EnemyStartUpgrades(enemy);
		}
	}

	void EnemyStartUpgrades(Enemy enemy)
	{
		EnemyStartEffects(enemy);
	}

	void EnemyStartEffects(Enemy enemy)
	{
		enemy.statusManager.ApplyStatusByType(StatusType.STARTOFTURN);
		StartEnemyTurn(enemy);
	}

	void StartEnemyTurn(Enemy enemy)
	{
		enemy.TakeTurn();
		this.actingEnemies.Remove(enemy);
	}

	void EnemyEndTurnUpgrades()
	{

	}

	void OnEnemyActionCompleted(Enemy enemy)
	{
		EnemyEndTurnEffects(enemy);
	}

	void EnemyEndTurnEffects(Enemy enemy)
	{
		enemy.statusManager.ApplyStatusByType(StatusType.ENDOFTURN);

		if(this.actingEnemies != null && this.actingEnemies.Any())
		{
			StartEnemyPhase(this.actingEnemies[0]);
		}
		else
		{
			OnEnemyTurnEnded();
		}
	}

	void OnEnemyTurnEnded()
	{
		ResetEnemyActions();
		if(this.player != null)
		{
			StartPlayerTurn();
		}
	}

	private void ResetEnemyActions()
	{
		foreach(Enemy child in this.enemiesParent.GetChildren())
		{
			child.SetCurrentAction(null);
			child.UpdateEnemy();
		}
	}

	//before enemy dies
	void OnEnemyDeath(Enemy enemy)
	{
		this.totalPrize.AddPrize(enemy.stats.winPrize);
		this.currentEnemies.Remove(enemy);
		
		if(this.actingEnemies != null && this.actingEnemies.Any())
		{
			this.actingEnemies.Remove(enemy);

			if(this.actingEnemies.Any())
			{
				EnemyStartUpgrades(this.actingEnemies[0]);
			}
			else
			{
				OnEnemyTurnEnded();
			}
		}

		if(this.currentEnemies != null && this.currentEnemies.Any() && !this.playerTurn)
		{
			OnEnemyTurnEnded();
		}
	}

	void OnEnemyStatusApplied(StatusType type, Enemy enemy)
	{
		switch(type)
		{
			case StatusType.STARTOFTURN:
			StartEnemyTurn(enemy);
			break;

			case StatusType.ENDOFTURN:
			this.actingEnemies.Remove(enemy);

			if(this.actingEnemies.Any())
			{
				EnemyStartUpgrades(this.actingEnemies[0]);
			}
			break;

			case StatusType.EVENT:
			break;
		}
	}

	//after enemy dies
	void OnEnemiesChildOrderChanged()
	{
		if(this.enemiesParent.GetChildCount() == 0)
		{
			this.battleOver = true;
			EventManager.instance.EmitSignal(EventManager.SignalName.BattleEnded, true, this.totalPrize);
		}
	}

	void OnPlayerDied()
	{
		this.player.statusManager.StatusesApplied -= OnPlayerStatusApplied;
		this.battleOver = true;
		EventManager.instance.EmitSignal(EventManager.SignalName.BattleEnded, false, this.totalPrize);
	}
}
