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
		player.SetPlayerStats(newStats);

		EventManager.instance.PlayerTurnEnded += OnPlayerTurnEnded;
		EventManager.instance.PlayerTurnEnded += StartBattleEnemies;
		EventManager.instance.EnemyTurnEnded += OnEnemyTurnEnded;
		EventManager.instance.PlayerDied += OnPlayerDied;

		StartBattlePlayer(newStats);
	}

	private void GetSceneNodes()
	{
		this.player = GetNode<Player>("%Player");
		this.enemiesParent = GetNode("%Enemies");
	}

	public void StartBattlePlayer(PlayerStats playerStats)
	{
		this.player.StartBattle(playerStats);
	}

	public void StartBattleEnemies()
	{
		//player.StartBattle(playerStats);
	}

	void OnPlayerTurnEnded()
	{

	}

	void OnEnemyTurnEnded()
	{
		// enemyHandler.ResetEnemyActions();
		// playerHandler.StartTurn();
	}

	void OnEnemiesChildOrderChanged()
	{
		// if(enemyHandler.GetChildCount() == 0)
		// {
		// 	EventManager.instance.EmitSignal(EventManager.SignalName.BattleOverScreenRequested,"Victory!", true);
		// }
	}

	void OnPlayerDied()
	{
		// EventManager.instance.EmitSignal(EventManager.SignalName.BattleOverScreenRequested, "Game Over!", false);
	}
}
