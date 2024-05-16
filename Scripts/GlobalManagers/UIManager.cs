using Godot;

public partial class UIManager : Node
{
	public static UIManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public BattleUI battle { get; private set; }
	public BattleEndUI battleEndUI { get; private set; }
	public StartPageUI startPageUI { get; private set; }
	public ChooseCharacterUI chooseCharacterUI { get; private set; }
	private CanvasLayer VFX;
	private Player player;

	public override void _Ready()
	{
		init();
		GetSceneNodes();

		SetBattleUIVisibility(false);
		SetBattleOverUIVisibility(false);
		SetChooseCharacterVisisbility(false);

		SetStartPageVisibilty(true);
	}

	private void GetSceneNodes()
	{
		this.battle = GetNode<BattleUI>("%CanvasLayerBattle");
		this.battleEndUI = GetNode<BattleEndUI>("%CanvasLayerEndBattle");
		this.startPageUI = GetNode<StartPageUI>("%CanvasLayerStartPage");
		this.chooseCharacterUI = GetNode<ChooseCharacterUI>("%CanvasLayerChooseCharacter");
		this.VFX = GetNode<CanvasLayer>("%CanvasLayerVFX");
	}

	public void SetBattleUIVisibility(bool value)
	{
		if(value)
		{
			this.battle.Show();
		}
		else
		{
			this.battle.Hide();
		}
	}
	public void SetBattleOverUIVisibility(bool value)
	{
		if(value)
		{
			this.battleEndUI.Show();
		}
		else
		{
			this.battleEndUI.Hide();
		}
	}
	public void SetStartPageVisibilty(bool value)
	{
		if(value)
		{
			this.startPageUI.Show();
		}
		else
		{
			this.startPageUI.Hide();
		}
	}
	public void SetChooseCharacterVisisbility(bool value)
	{
		if(value)
		{
			this.chooseCharacterUI.Show();
		}
		else
		{
			this.chooseCharacterUI.Hide();
		}
	}
}

