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

	public override void _Ready()
	{
		init();
	}

	public void SetPlayerStats(PlayerStats playerStats)
	{
		this.playerStats = playerStats;
	}
}

