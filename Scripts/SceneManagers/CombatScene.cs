using Godot;

public partial class CombatScene : Node2D
{
	[Export] private PlayerStats playerStats;
	[Export] private AudioStream BGM;
	[Export] private EnemyStats[] enemies;
	private Player player;
	private PackedScene enemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Character/Enemy.tscn");
	private Node enemiesParent;
	private WinPrize totalPrize = new WinPrize();

	public bool battleOver { get; private set; } = false;

	public override void _Ready()
	{
		GetSceneNodes();

		PlayerStats newStats = playerStats.CreateInstance();
		UIManager.instance.battle.SetPlayerStats(newStats);
		this.player.SetPlayerStats(newStats);

		ConnectEventSignals();

		StartBattlePlayer(this.player.stats);
	}

	private void GetSceneNodes()
	{
		this.player = GetNode<Player>("%Player");
		this.enemiesParent = GetNode("%Enemies");
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

	public void StartBattlePlayer(PlayerStats playerStats)
	{
		AudioManager.instance.musicPlayer.Play(this.BGM);
		this.player.StartBattle(playerStats);
		StartBattleEnemies();
	}

	public void StartBattleEnemies()
	{
		StartPlayerTurn();
	}

	void StartPlayerTurn()
	{
		if(this.battleOver) {return;}
		this.player.StartTurn();
	}

	void OnPlayerTurnEnded()
	{
		this.player.hand.DisableHand();
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
