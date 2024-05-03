using Godot;
using System;

public partial class VFXRedFlash : CanvasLayer
{
	[Export] ColorRect colorRect;
	[Export] Timer timer;

    public override void _Ready()
    {
        EventManager.instance.PlayerHit += OnPlayerHit;
		timer.Timeout += OnTimerTimeout;
    }

	public void OnPlayerHit()
	{
		colorRect.Color = new Color(colorRect.Color, 0.2f);
		timer.Start();
	}

	public void OnTimerTimeout()
	{
		colorRect.Color = new Color(colorRect.Color, 0.0f);
	}
}
