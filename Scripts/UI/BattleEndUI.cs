using Godot;
using System;

public partial class BattleEndUI : CanvasLayer
{
	public Label resultLabel { get; private set; }
	public Label EXPLabel { get; private set; }
	public Label moneyLabel { get; private set; }
	public Button continueButton { get; private set; }
	public Button restartButton { get; private set; }
	public Button quitButton { get; private set; }

	public override void _Ready()
	{
		GetSceneNodes();

		this.continueButton.Pressed += () => {GetTree().Quit();};
		this.restartButton.Pressed += () => 
		{
			GetTree().ReloadCurrentScene();
		};
		this.quitButton.Pressed += () => {GetTree().Quit();};

		EventManager.instance.BattleEnded += BattleOver;

		Hide();
	}

	private void GetSceneNodes()
	{
		this.resultLabel = GetNode<Label>("%LabelResult");
		this.EXPLabel = GetNode<Label>("%LabelEXP");
		this.moneyLabel = GetNode<Label>("%LabelMoney");
		this.continueButton = GetNode<Button>("%ButtonContinue");
		this.restartButton = GetNode<Button>("%ButtonRestart");
		this.quitButton = GetNode<Button>("%ButtonQuit");
	}

	private void BattleOver(bool hasWon)
	{
		if(hasWon)
		{
			this.resultLabel.Text = "Victory!";
		}
		else
		{
			this.resultLabel.Text = "Game Over!";
		}

		this.continueButton.Visible = hasWon;
		this.restartButton.Visible = !hasWon;

		Show();
	}

}
