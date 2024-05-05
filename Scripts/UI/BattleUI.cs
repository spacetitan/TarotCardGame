using Godot;
using System;
using System.Diagnostics;

public partial class BattleUI : CanvasLayer
{
	public Hand hand { get; private set; }

	public PlayerStats playerStats{ get; private set; }

	public override void _Ready()
	{
		GetSceneNodes();
	}

	private void GetSceneNodes()
	{
		this.hand = GetNode<Hand>("%HBoxContainerHand");
	}

	public void SetPlayerStats(PlayerStats player)
	{
		playerStats = player;
		hand.SetPlayerStats(player);
		//manaUI.SetCharStats(characterStats);
	}
}
