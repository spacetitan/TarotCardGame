using Godot;

public partial class CombatScene : Node2D
{
	[Export] private PlayerStats playerStats;
	private Player player;
	private Node enemiesParent;

	public override void _Ready()
	{
		GetSceneNodes();

		PlayerStats newStats = playerStats.CreateInstance();
		UIManager.instance.battle.SetPlayerStats(newStats);
		this.player.SetPlayerStats(newStats);

		EventManager.instance.PlayerTurnEnded += OnPlayerTurnEnded;
		EventManager.instance.PlayerHandDiscarded += StartEnemyTurn;
		EventManager.instance.EnemyActionCompleted += OnEnemyActionCompleted;
		EventManager.instance.EnemyTurnEnded += OnEnemyTurnEnded;
		EventManager.instance.PlayerDied += OnPlayerDied;

		StartBattlePlayer(this.player.stats);
	}

	private void GetSceneNodes()
	{
		this.player = GetNode<Player>("%Player");
		this.enemiesParent = GetNode("%Enemies");
	}

	public void StartBattlePlayer(PlayerStats playerStats)
	{
		this.player.StartBattle(playerStats);
		StartBattleEnemies();
	}

	public void StartBattleEnemies()
	{
		StartPlayerTurn();
	}

	void StartPlayerTurn()
	{
		this.player.StartTurn();
	}

	void OnPlayerTurnEnded()
	{
		this.player.hand.DisableHand();
		this.player.DiscardHand();
	}

	void StartEnemyTurn()
	{
		if(this.enemiesParent.GetChildCount() == 0) {return;}

		Enemy firstEnemy = this.enemiesParent.GetChild(0) as Enemy;
		firstEnemy.TakeTurn();
	}

	void OnEnemyActionCompleted(int index)
	{
		if(index == this.enemiesParent.GetChildCount() - 1)
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.EnemyTurnEnded);
			return;
		}

		Enemy nextEnemy = GetChild(index + 1) as Enemy;
		nextEnemy.TakeTurn();
	}

	void OnEnemyTurnEnded()
	{
		ResetEnemyActions();
		StartPlayerTurn();
	}

	private void ResetEnemyActions()
	{
		foreach(Enemy child in this.enemiesParent.GetChildren())
		{
			child.SetCurrentAction(null);
			child.UpdateEnemy();
		}
	}

	void OnEnemiesChildOrderChanged()
	{
		if(this.enemiesParent.GetChildCount() == 0)
		{
			//EventManager.instance.EmitSignal(EventManager.SignalName.BattleOverScreenRequested,"Victory!", true);
		}
	}

	void OnPlayerDied()
	{
		// EventManager.instance.EmitSignal(EventManager.SignalName.BattleOverScreenRequested, "Game Over!", false);
	}
}
