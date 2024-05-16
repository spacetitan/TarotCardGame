using Godot;
using System;

public partial class StartPageUI : CanvasLayer
{
	public Button newRunButton;
	public Button continueRunButton;
	public Button settingsButton;
	public Button quitGameButton;
	public override void _Ready()
	{
		GetSceneNodes();

		this.newRunButton.Pressed += () => 
		{
			UIManager.instance.SetStartPageVisibilty(false);
			UIManager.instance.SetChooseCharacterVisisbility(true);
		};
	}

	private void GetSceneNodes()
	{
		this.newRunButton = GetNode<Button>("%ButtonNewRun");
		this.continueRunButton = GetNode<Button>("%ButtonContinueRun");
		this.settingsButton = GetNode<Button>("%ButtonSettings");
		this.quitGameButton = GetNode<Button>("%ButtonSettings");
	}
}
