using Godot;
using System;

public partial class CombatScene : Node2D
{
	[Export] private PlayerStats playerStats;
	[Export] private BattleUI battleUI;
	private Player player;

	public override void _Ready()
	{
		GetSceneNodes();

		PlayerStats newStats = playerStats.CreateInstance();
		UIManager.instance.battle.SetPlayerStats(newStats);
		player.SetPlayerStats(newStats);

		//EventManager.instance.PlayerTurnEnded += playerHandler.EndTurn;
		//EventManager.instance.PlayerHandDiscarded += enemyHandler.StartTurn;
		EventManager.instance.EnemyTurnEnded += OnEnemyTurnEnded;
		EventManager.instance.PlayerDied += OnPlayerDied;

		StartBattle(newStats);
	}

	private void GetSceneNodes()
	{
		this.player = GetNode<Player>("%Player");

		//this.battleUI = UIManager.instance.battle;
	}

	public void StartBattle(PlayerStats playerStats)
	{
		player.StartBattle(playerStats);
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
