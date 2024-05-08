using Godot;
using System;
using System.Diagnostics;

public partial class BattleUI : CanvasLayer
{
	public Hand hand { get; private set; }
	public ManaUI manaUI { get; private set; }

	public PlayerStats playerStats { get; private set; }

	public override void _Ready()
	{
		GetSceneNodes();
	}

	private void GetSceneNodes()
	{
		this.hand = GetNode<Hand>("%HBoxContainerHand");
		this.manaUI = GetNode<ManaUI>("%ManaUI");
	}

	public void SetPlayerStats(PlayerStats value)
	{
		playerStats = value;
		hand.SetPlayerStats(value);
		manaUI.SetPlayerStats(value);
	}
}
