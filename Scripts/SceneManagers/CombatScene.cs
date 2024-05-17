using System;
using System.Collections.Generic;
using Godot;

public partial class CombatScene : Node2D
{
	[Export] private AudioStream BGM;
	private PlayerStats playerStats;
	[Export] private EnemyStats[] enemies;
	private Player player;
	const String ENEMY_SCENE = "res://Scenes/Character/Enemy.tscn";
	private Node2D enemiesParent;
	private WinPrize totalPrize = new WinPrize();

	public bool battleOver { get; private set; } = false;

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
		EventManager.instance.PlayerHandDiscarded += StartEnemyTurn;
		EventManager.instance.PlayerDied += OnPlayerDied;
		EventManager.instance.EnemyActionCompleted += OnEnemyActionCompleted;
		EventManager.instance.EnemyTurnEnded += OnEnemyTurnEnded;
		EventManager.instance.EnemyDied += OnEnemyDeath;
	}

	private void DisconnectEventSignals()
	{
		EventManager.instance.PlayerTurnEnded -= OnPlayerTurnEnded;
		EventManager.instance.PlayerHandDiscarded -= StartEnemyTurn;
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

		foreach (EnemyStats enemy in enemyList)
		{
			var scene = GD.Load<PackedScene>(ENEMY_SCENE);
			var newEnemy = scene.Instantiate();
			this.enemiesParent.AddChild(newEnemy);

			Enemy spawn = newEnemy as Enemy;
			spawn.SetEnemyStats(enemy);
		}

		AudioManager.instance.musicPlayer.Stop();
		AudioManager.instance.musicPlayer.Play(this.BGM);
		this.player.StartBattle(this.playerStats);

		UIManager.instance.SetBattleUIVisibility(true);

		StartPlayerPhase();
	}

	public void StartPlayerPhase()
	{
		PlayerStartUpgrades();
		PlayerStartEffects();
		StartPlayerTurn();
	}

	void PlayerStartUpgrades()
	{
		
	}

	void PlayerStartEffects()
	{

	}

	void StartPlayerTurn()
	{
		if(this.battleOver) {return;}
		this.player.StartTurn();
	}

	void OnPlayerTurnEnded()
	{
		this.player.hand.DisableHand();
		EndTurnUpgrades();
		EndTurnEffects();
		EndPlayerTurn();
	}

	void EndTurnUpgrades()
	{

	}

	void EndTurnEffects()
	{

	}

	void EndPlayerTurn()
	{
		this.player.DiscardHand();
	}

	void StartEnemyTurn()
	{
		if(this.enemiesParent.GetChildCount() == 0 || battleOver) {return;}

		Enemy firstEnemy = this.enemiesParent.GetChild(0) as Enemy;
		firstEnemy.TakeTurn();
	}

	void OnEnemyActionCompleted(int index)
	{
		if(index == this.enemiesParent.GetChildCount() - 1 || battleOver)
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.EnemyTurnEnded);
			return;
		}

		Enemy nextEnemy = this.enemiesParent.GetChild(index + 1) as Enemy;
		nextEnemy.TakeTurn();
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
	void OnEnemyDeath(EnemyStats enemy)
	{
		this.totalPrize.AddPrize(enemy.winPrize);
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
		this.battleOver = true;
		EventManager.instance.EmitSignal(EventManager.SignalName.BattleEnded, false, this.totalPrize);
	}
}
