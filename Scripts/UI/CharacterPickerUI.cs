using Godot;

public partial class CharacterPickerUI : Node
{
	[Export] PlayerStats testStats;
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

		if(testStats != null)
		{
			SetUI(this.testStats);
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
		this.backgroundColorRect.Color = ToolsManager.instance.GetCharColor(playerStats.type);
		this.charSprite.Texture = playerStats.art;
		this.healthLabel.Text = playerStats.maxHealth.ToString();
		this.nameLabel.Text = playerStats.name;
		this.abilityNameLabel.Text = playerStats.ability.name;
		this.abilityDescLabel.Text = playerStats.ability.desc;
	}
}

