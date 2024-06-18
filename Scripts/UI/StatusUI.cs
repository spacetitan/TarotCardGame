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

	public void SetStatus(Status status)
	{
		if(this.status != null)
		{
			this.status.StatusApplied -= SetStatus;
			this.status.StatusChanged -= UpdateUI;
		}

		this.status = status;
		this.status.StatusApplied += SetStatus;
		this.status.StatusChanged += UpdateUI;

		UpdateUI();
	}

	public void UpdateUI()
	{
		this.icon.Texture = this.status.statusIcon;

		if(!this.status.canExpire) { return; }

		if (status.stackType == StatusStackType.INTENSITY)
		{
			this.label.Text = this.status.stacks.ToString();

			if(this.status.stacks < 1)
			{
				DestroyUI();
			}
		}
		else if (status.stackType == StatusStackType.DURATION)
		{
			this.label.Text = this.status.duration.ToString();

			if(this.status.duration < 1)
			{
				DestroyUI();
			}
		}
	}

	void DestroyUI()
	{
		this.status.StatusApplied -= SetStatus;
		this.status.StatusChanged -= UpdateUI;
		QueueFree();
	}
}
