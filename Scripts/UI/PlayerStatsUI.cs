using Godot;

[GlobalClass]
public partial class PlayerStatsUI : Node
{
	TextureRect armor;
	Label armorLabel;
	TextureRect health;
	Label healthLabel;

    public override void _Ready()
    {
        GetSceneNodes();
    }

	private void GetSceneNodes()
	{
		this.armor = GetNode<TextureRect>("%TextureRectArmor");
		this.armorLabel = GetNode<Label>("%LabelArmor");
		this.health = GetNode<TextureRect>("%TextureRectHealth");
		this.healthLabel = GetNode<Label>("%LabelHealth");
	}

    public void UpdateStats(PlayerStats playerStats)
	{
		this.armorLabel.Text = playerStats.armor.ToString();
		this.healthLabel.Text = playerStats.health.ToString();

		this.armor.Visible = playerStats.armor > 0;
		this.armorLabel.Visible = playerStats.armor > 0;
		// health.Visible = stats.health > 0;
	}
}

