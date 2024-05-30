using Godot;
using System;

public partial class StatusUI : Control
{
	[Export] public TextureRect icon { get; private set; }
	[Export] public Label label { get; private set; }
	public Status status { get; private set; }

    public override void _Ready()
    {
        GetSceneNodes();
    }

	private void GetSceneNodes()
	{
		this.icon = GetNode<TextureRect>("%TextureRectIcon");
		this.label = GetNode<Label>("%LabelStack");
	}

	public void SetUI(Status status)
	{
		this.icon.Texture = status.statusIcon;
		this.label.Text = status.desc;
		this.status = status;
	}
}
