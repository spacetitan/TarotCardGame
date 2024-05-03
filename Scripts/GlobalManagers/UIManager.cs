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

	private CanvasLayer battle;
	private CanvasLayer VFX;


	public override void _Ready()
	{
		GetSceneNodes();

		SetBattleUIVisibility(true);
	}

	private void GetSceneNodes()
	{
		this.battle = GetNode<CanvasLayer>("%CanvasLayerBattle");
		this.VFX = GetNode<CanvasLayer>("%CanvasLayerVFX");
	}

	public void SetBattleUIVisibility(bool value)
	{
		this.battle.Visible = value;
	}
}

