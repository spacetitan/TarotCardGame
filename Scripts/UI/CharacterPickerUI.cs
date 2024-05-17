using Godot;

public partial class CharacterPickerUI : Node
{
	[Export] PlayerStats playerStats;
	public ColorRect backgroundColorRect;
	public TextureRect charSprite;
	public Label healthLabel;
	public Label nameLabel;
	public Label abilityNameLabel;
	public Label abilityDescLabel;
	public Button chooseButton;
	public override void _Ready()
	{
		GetSceneNodes();

		if(playerStats != null)
		{
			SetUI(this.playerStats);
		}
	}

	private void GetSceneNodes()
	{
		this.backgroundColorRect = GetNode<ColorRect>("%ColorRectBackground");
		this.charSprite = GetNode<TextureRect>("%TextureRectCharacterSprite");
		this.healthLabel = GetNode<Label>("%LabelHealth");
		this.nameLabel = GetNode<Label>("%LabelName");
		this.abilityNameLabel = GetNode<Label>("%LabelAbilityName");
		this.abilityDescLabel = GetNode<Label>("%LabelAbilityDescription");
		this.chooseButton = GetNode<Button>("%ButtonChoose");
	}

	public void SetUI(PlayerStats playerStats)
	{
		this.playerStats = playerStats;

		this.backgroundColorRect.Color = ToolsManager.GetCharColor(this.playerStats.type);
		this.charSprite.Texture = this.playerStats.art;
		this.healthLabel.Text = this.playerStats.maxHealth.ToString();
		this.nameLabel.Text = this.playerStats.name;
		this.abilityNameLabel.Text = "Ability: " + this.playerStats.ability.name;
		this.abilityDescLabel.Text = this.playerStats.ability.desc;

		this.backgroundColorRect.Color = ToolsManager.GetCharColor(this.playerStats.type);

		this.chooseButton.Pressed += () =>
		{
			GameManager.instance.SetPlayerStats(this.playerStats);
			UIManager.instance.SetChooseCharacterVisisbility(false);

			GetTree().ChangeSceneToPacked(GameManager.COMBAT_SCENE);
		};
	}
}

