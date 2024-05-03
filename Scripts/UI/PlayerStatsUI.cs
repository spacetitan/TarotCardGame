using Godot;

[GlobalClass]
public partial class PlayerStatsUI : Node
{
	HBoxContainer armor;
	Label armorLabel;
	HBoxContainer health;
	Label healthLabel;

    public override void _Ready()
    {
        GetSceneNodes();
    }

	private void GetSceneNodes()
	{

	}

    public void UpdateStats()
	{
		// armorLabel.Text = stats.armor.ToString();
		// healthLabel.Text = stats.health.ToString();

		// armor.Visible = stats.armor > 0;
		// health.Visible = stats.health > 0;
	}
}

