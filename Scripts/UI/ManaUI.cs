using Godot;
using System;

public partial class ManaUI : Panel
{
	public PlayerStats playerStats;
	private Label manaLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetSceneNodes();
	}

	private void GetSceneNodes()
	{
		manaLabel = GetNode<Label>("%LabelMana");
	}

	public void SetPlayerStats(PlayerStats value)
	{
		this.playerStats = value;
		this.playerStats.StatsChanged -= OnStatsChanged;
		this.playerStats.StatsChanged += OnStatsChanged;
		OnStatsChanged();
	}

	public void OnStatsChanged()
	{
		manaLabel.Text = playerStats.mana.ToString();
	}
}
