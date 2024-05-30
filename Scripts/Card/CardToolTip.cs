using Godot;
using System;

public partial class CardToolTip : Panel
{
	private TextureRect tooltipIcon;
	private Label tooltipLabel;
	private ColorRect tooltipBackground;
	Tween tween;

	public override void _Ready()
	{
		GetSceneNodes();

		Modulate = Colors.Transparent;
		Hide();
		this.tooltipBackground.Hide();
	}

	private void GetSceneNodes()
	{
		this.tooltipIcon = GetNode<TextureRect>("%TextureRectTTIcon");
		this.tooltipLabel = GetNode<Label>("%LabelToolTip");
		this.tooltipBackground = GetNode<ColorRect>("%ColorRectTTCardType");
	}

	public void SetToolTip(CardStats stats)
	{
		this.tooltipIcon.Texture = stats.cardArt;
		this.tooltipLabel.Text = stats.cardDesc;
	}

	public void ShowTooltip()
	{
		if(tween != null)
		{
			tween.Kill();
		}

		tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenCallback(Callable.From(this.tooltipBackground.Show));
		tween.TweenCallback(Callable.From(Show));
		tween.TweenProperty(this, "modulate", Colors.White, .02f);
	}

	public void HideTooltip()
	{
		if(tween != null)
		{
			tween.Kill();
		}

		HideAnimation();
	}

	public void HideAnimation()
	{
		if(!this.IsInsideTree() || tween == null) { return;}
		tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "modulate", Colors.Transparent, .02f);
		tween.TweenCallback(Callable.From(this.tooltipBackground.Hide));
		tween.TweenCallback(Callable.From(Hide));
	}
}
