using Godot;

public partial class GameManager : Node
{
	public static GameManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	[Export] public PlayerStats playerStats { get; private set; }

	public static PackedScene COMBAT_SCENE { get; private set; } = ResourceLoader.Load<PackedScene>("res://Scenes/GamePlay/CombatScene.tscn");

	public override void _Ready()
	{
		init();
	}

	public void SetPlayerStats(PlayerStats playerStats)
	{
		this.playerStats = playerStats;
	}
}

