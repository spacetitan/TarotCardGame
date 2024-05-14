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
	private CanvasLayer VFX;

	private Player player;

	public override void _Ready()
	{
		init();
		GetSceneNodes();

		SetBattleUIVisibility(true);
	}

	private void GetSceneNodes()
	{
		this.battle = GetNode<BattleUI>("%CanvasLayerBattle");
		this.battleEndUI = GetNode<BattleEndUI>("%CanvasLayerEndBattle");
		this.VFX = GetNode<CanvasLayer>("%CanvasLayerVFX");
	}

	#region Battle UI
	public void SetBattleUIVisibility(bool value)
	{
		this.battle.Visible = value;
	}
	#endregion
}

