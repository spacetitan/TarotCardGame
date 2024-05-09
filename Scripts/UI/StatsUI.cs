using Godot;

[GlobalClass]
public partial class StatsUI : Node
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

    public void UpdateStats(CharacterStats stats)
	{
		this.armorLabel.Text = stats.armor.ToString();
		this.healthLabel.Text = stats.health.ToString();

		this.armor.Visible = stats.armor > 0;
		this.armorLabel.Visible = stats.armor > 0;
		// health.Visible = stats.health > 0;
	}
}

